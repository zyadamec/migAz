﻿# Copyright (c) Microsoft Corporation. All rights reserved.
# Licensed under the MIT License.

# Parameters
param (
    [Parameter(Mandatory = $false)] 
    [string] $Environment = "AzureCloud",

    [Parameter(Mandatory = $true)] 
    [Guid] $TenantId,

    [Parameter(Mandatory = $true)] 
    [Guid] $SubscriptionId,

    [Parameter(Mandatory = $false)] 
    [string] $AccountId,

    [Parameter(Mandatory = $false)] 
    [string] $AccessToken,

    [Parameter(Mandatory = $true)] 
    [string] $ResourceGroupName,

    [Parameter(Mandatory = $true)] 
    [string] $ResourceGroupLocation,

    [Parameter(Mandatory = $true)] 
    [string] $TemplateFile,

    [Parameter(Mandatory = $false)] 
    [string] $TemplateParameterFile,

    [Parameter(Mandatory = $false)] 
    [string] $BlobCopyFile,

    [ValidateSet("CreateResourceGroup", "TestResourceGroupDeployment", "StartBlobCopy", "MonitorBlobCopy", "CancelBlobCopy", "ResourceGroupDeployment", "DeleteMigAzTempStorage", "RetryDeployment")]
    [Parameter(Mandatory = $false)] 
    [string] $StartType,

    $RefreshInterval = 10
)

# Check Az.Storage version - minimum 4.0.1 required
if ((get-command New-AzStorageContext).Version.ToString() -lt '2.1.0')
{
    Write-Host 'Please update Az.Storage module to 2.1.0 or higher before running this script' -ForegroundColor Yellow
    Exit
}

# Check Az.Storage version - minimum 4.0.1 required
if ((get-command Get-AzStorageAccountKey).Version.ToString() -lt '2.1.0')
{
    Write-Host 'Please update Az.Storage module to 2.1.0 or higher before running this script' -ForegroundColor Yellow
    Exit
}

if ($AccountId -ne $null -and $AccountId.Length -gt 0 -and $AccessToken -ne $null -and $AccessToken.Length -gt 0)
{
	Add-AzAccount -Environment $Environment -TenantId $TenantId -SubscriptionId $SubscriptionId -AccountId $AccountId -AccessToken $AccessToken
}
else
{
	Add-AzAccount -Environment $Environment -TenantId $TenantId -SubscriptionId $SubscriptionId
}

If ($StartType -eq "" -or $StartType -eq "CreateResourceGroup")
{
	$CreateResourceGroupError = $false
	Write-Host "Getting Azure Resource Group '$($ResourceGroupName)'"
	$ResourceGroup = Get-AzResourceGroup -Name $ResourceGroupName -ErrorAction Continue -ErrorVariable GetResourceGroupError
	if ($GetResourceGroupError)
	{
		Write-Host " - Creating Azure Resource Group '$($ResourceGroupName)' in Location '$($ResourceGroupLocation)'"
		New-AzResourceGroup -Name $ResourceGroupName -Location $ResourceGroupLocation -ErrorAction Stop
	}
	else
	{
		Write-Host " - Azure Resource Group '$($ResourceGroupName)' already exists."
	}
}

# If Initiating the copy of all blobs
If (($StartType -eq "" -and !$CreateResourceGroupError) -or $StartType -eq "StartBlobCopy")
{
	If ($BlobCopyFile -eq $null -or $BlobCopyFile -eq "")
	{
		Write-Host "Skipping Blob Copy (no BlobCopyFile specified)"
		$StartBlobCopyError = $null
	}
	else
	{
		$StartBlobCopyError = $false
		$copyblobdetailsout = @()

		# Load blob copy details file (ex: copyblobdetails.json)
		$copyblobdetails = Get-Content -Path $BlobCopyFile -Raw | ConvertFrom-Json

		$sourceExpirationValid = $true
		Write-Host "Validating Blob Source Expiration"
		# Validate Source Expiration Date(s)
		foreach ($copyblobdetail in $copyblobdetails)
		{
			if ($copyblobdetail.SourceExpiration -ne $null)
			{
				$sourceExpirationDate = [datetime] $copyblobdetail.SourceExpiration
                $TimeDiff = New-TimeSpan (Get-Date) $sourceExpirationDate

				if ($TimeDiff.TotalMilliseconds -le 0)
				{
					$sourceExpirationValid = $false
					$StartBlobCopyError = $true
					Write-Host " - ERROR: '$($copyblobdetail.TargetBlob)' source access has expired ($($sourceExpirationDate) UTC)." -ForegroundColor Red
				}
				else
				{
					Write-Host " - WARNING: '$($copyblobdetail.TargetBlob)' source access expires in $($TimeDiff.Hours) hour(s) $($TimeDiff.Minutes) minute(s) $($TimeDiff.Seconds) seconds(s) at ($($sourceExpirationDate) UTC)." -ForegroundColor Yellow
				}
			}
		}

		Write-Host ""

		if ($sourceExpirationValid -eq $false)
		{
			throw "One or more Blob Copy sources have expired.  Unable to continue."
		}

		# Initiate the copy of all blobs
		foreach ($copyblobdetail in $copyblobdetails)
		{
			# Ensure the Storage Account Exists
			$targetStorageAccount = Get-AzStorageAccount -ResourceGroupName $copyblobdetail.TargetResourceGroup -Name $copyblobdetail.TargetStorageAccount -ErrorAction SilentlyContinue
			if ($targetStorageAccount -eq $null)
			{
				Write-Host "Target Storage Account '$($copyblobdetail.TargetStorageAccount)' not found.  Attempting to create."
				New-AzStorageAccount -ResourceGroupName $copyblobdetail.TargetResourceGroup -Name $copyblobdetail.TargetStorageAccount -Location $copyblobdetail.TargetLocation -SkuName $copyblobdetail.TargetStorageAccountType -ErrorAction Stop
			}

			# Create destination storage account context
			$copyblobdetail.TargetKey = (Get-AzStorageAccount -ResourceGroupName $copyblobdetail.TargetResourceGroup -Name $copyblobdetail.TargetStorageAccount | Get-AzStorageAccountKey).Value[0]
			$destination_context = New-AzStorageContext -StorageAccountName $copyblobdetail.TargetStorageAccount -StorageAccountKey $copyblobdetail.TargetKey -Endpoint $copyblobdetail.TargetEndpoint -ErrorAction Stop

			# Create destination container if it does not exist
			$destination_container = Get-AzStorageContainer -Context $destination_context -Name $copyblobdetail.TargetContainer -ErrorAction SilentlyContinue
			if ($destination_container -eq $null)
			{ 
				New-AzStorageContainer -Context $destination_context -Name $copyblobdetail.TargetContainer
			}

			if ($copyblobdetail.SourceAbsoluteUri -eq $null)
			{
				# Create source storage account context
				$source_context = New-AzStorageContext -StorageAccountName $copyblobdetail.SourceStorageAccount -StorageAccountKey $copyblobdetail.SourceKey -Environment $copyblobdetail.SourceEnvironment -ErrorAction Stop
    
				#Get a reference to a blob
				$blob = Get-AzStorageBlob -Context $source_context -Container $copyblobdetail.SourceContainer -Blob $copyblobdetail.SourceBlob -ErrorAction Stop
				#Create a snapshot of the blob
				$snap = $blob.ICloudBlob.CreateSnapshot()
				$copyblobdetail.SnapshotTime = $snap.SnapshotTime.DateTime.ToString()
				# Get just created snapshot
				$snapshot = Get-AzStorageBlob -Context $source_context -Container $copyblobdetail.SourceContainer -Prefix $copyblobdetail.SourceBlob | Where-Object  { $_.Name -eq $copyblobdetail.SourceBlob -and $_.SnapshotTime -eq $snap.SnapshotTime } 
            
				Write-Host " - Starting Blob Copy '$($copyblobdetail.TargetBlob)'." -ForegroundColor Green

				# Initiate blob snapshot copy job
				Start-AzStorageBlobCopy -Context $source_context -ICloudBlob $snapshot.ICloudBlob -DestContext $destination_context -DestContainer $copyblobdetail.TargetContainer -DestBlob $copyblobdetail.TargetBlob
			}
			else
			{
				# Initiate blob snapshot copy job
				Start-AzStorageBlobCopy -AbsoluteUri $copyblobdetail.SourceAbsoluteUri -DestContext $destination_context -DestContainer $copyblobdetail.TargetContainer -DestBlob $copyblobdetail.TargetBlob
			}

			# Updates $copyblobdetailsout array
			$copyblobdetail.StartTime = Get-Date
			$copyblobdetailsout += $copyblobdetail

			# Updates screen table
			# cls
			$copyblobdetails | select TargetStorageAccount, TargetContainer, TargetBlob, Status, BytesCopied, TotalBytes, StartTime, EndTime | Format-Table -AutoSize
		}

		# Updates file with data from $copyblobdetailsout
		$copyblobdetailsout | ConvertTo-Json -Depth 100 | Out-File $BlobCopyFile
	}

}

# If waiting for all blobs to copy and get statistics
If (($StartType -eq "" -and !$StartBlobCopyError) -or $StartType -eq "MonitorBlobCopy")
{
	If ($BlobCopyFile -eq $null -or $BlobCopyFile -eq "")
	{
		Write-Host "Skipping Monitor Blob Copy (no BlobCopyFile specified)"
		$BlobCopyFailure = $null
	}
	else
	{
		# Waits for all blobs to copy and get statistics
		$ContinueBlobCopy = $true
		$BlobCopyFailure = $false

		while ($ContinueBlobCopy -eq $true -and $BlobCopyFailure -eq $false)
		{
			$ContinueBlobCopy = $false

			foreach ($copyblobdetail in $copyblobdetails)
			{
				if ($copyblobdetail.Status -ne "Success" -and $copyblobdetail.Status -ne "Failed")
				{
					$destination_context = New-AzStorageContext -StorageAccountName $copyblobdetail.TargetStorageAccount -StorageAccountKey $copyblobdetail.TargetKey -Endpoint $copyblobdetail.TargetEndpoint
					$status = Get-AzStorageBlobCopyState -Context $destination_context -Container $copyblobdetail.TargetContainer -Blob $copyblobdetail.TargetBlob

					$copyblobdetail.TotalBytes = "{0:N0} MB" -f ($status.TotalBytes / 1MB)
					$copyblobdetail.BytesCopied = "{0:N0} MB" -f ($status.BytesCopied / 1MB)
					$copyblobdetail.Status = $status.Status
					$copyblobdetail.StatusDescription = $status.StatusDescription

					$ContinueBlobCopy = $true

			        if ($copyblobdetail.SourceExpiration -ne $null)
			        {
                        $sourceExpirationDate = [datetime] $copyblobdetail.SourceExpiration
                        $TimeDiff = New-TimeSpan (Get-Date) $sourceExpirationDate
                        
                        if ($TimeDiff.Hours -eq 0 -and ($TimeDiff.Minutes -ge 1 -or $TimeDiff.Seconds -ge 1))
                        {
                        	Write-Host " - WARNING: '$($copyblobdetail.TargetBlob)' source access expires in $($TimeDiff.Minutes) minute(s) $($TimeDiff.Seconds) seconds(s) at ($($sourceExpirationDate) UTC)." -ForegroundColor Yellow
                        }
                    }

					if ($copyblobdetail.Status -eq "Success")
					{
						Write-Host " - Blob Copy '$($copyblobdetail.TargetBlob)' Completed" -ForegroundColor Green
						$copyblobdetail.EndTime = Get-Date

						if ($TemplateParameterFile -ne $null -and $copyblobdetail.OutputParameterName -ne $null)
						{
							$SourceUri = "$($destination_context.BlobEndPoint)$($copyblobdetail.TargetContainer)/$($copyblobdetail.TargetBlob)"
							Write-Host " - Replacing parameter placeholder '$($copyblobdetail.OutputParameterName)' with '$($SourceUri)' in '$($TemplateParameterFile)'" -ForegroundColor Green

							(Get-Content $TemplateParameterFile) -replace $copyblobdetail.OutputParameterName,$SourceUri | out-file $TemplateParameterFile
						}
					}
					elseif ($copyblobdetail.Status -eq "Failed")
					{
						Write-Host " - Blob Copy '$($copyblobdetail.TargetBlob)' Failed - '$($copyblobdetail.StatusDescription)'" -ForegroundColor Red
						Write-Host ""
						$BlobCopyFailure = $true
					}
				}
			}

			$copyblobdetails | ConvertTo-Json -Depth 100 | Out-File $BlobCopyFile

			if ($ContinueBlobCopy -eq $true -and $BlobCopyFailure -eq $false)
			{
				$copyblobdetails | ? { $_.Status -eq "Pending" } | Sort-Object TargetBlob | select TargetStorageAccount, TargetContainer, TargetBlob, Status, BytesCopied, TotalBytes, StartTime, EndTime | Format-Table -AutoSize
				Start-Sleep -Seconds $refreshinterval
			}
		}

		$copyblobdetails | Sort-Object Status, TargetBlob | select TargetStorageAccount, TargetContainer, TargetBlob, Status, BytesCopied, TotalBytes, StartTime, EndTime | Format-Table -AutoSize

		# Delete used snapshots
		foreach ($copyblobdetail in $copyblobdetails)
		{
			# Create source storage account context
			if ($copyblobdetail.SourceStorageAccount -ne $null)
			{
				$source_context = New-AzStorageContext -StorageAccountName $copyblobdetail.SourceStorageAccount -StorageAccountKey $copyblobdetail.SourceKey -Environment $copyblobdetail.SourceEnvironment

				$source_container = Get-AzStorageContainer -Context $source_context -Name $copyblobdetail.SourceContainer
				$blobs = $source_container.CloudBlobContainer.ListBlobs($copyblobdetail.SourceBlob, $true, "Snapshots") | Where-Object { $_.SnapshotTime -ne $null -and $_.SnapshotTime.DateTime.ToString() -EQ $copyblobdetail.SnapshotTime }

				if ($blobs.Count > 0)
				{
					$blobs[0].Delete()
				}
			}
		}
	}
}

# If to Cancel blobs copy
If (($StartType -eq "" -and $BlobCopyFailure) -or $StartType -eq "CancelBlobCopy")
{
	If ($BlobCopyFile -eq $null -or $BlobCopyFile -eq "")
	{
		Write-Host "Skipping Monitor Blob Cancel (no BlobCopyFile specified)"
		$BlobCopyFailure = $null
	}
	else
	{
    foreach ($copyblobdetail in $copyblobdetails)
    {
        if ($copyblobdetail.Status -ne "Success" -and $copyblobdetail.Status -ne "Failed")
        {
			Write-Host " - Stopping Blob Copy '$($copyblobdetail.TargetBlob)'"

            $destination_context = New-AzStorageContext -StorageAccountName $copyblobdetail.TargetStorageAccount -StorageAccountKey $copyblobdetail.TargetKey
            Stop-AzStorageBlobCopy -Context $destination_context -Container $copyblobdetail.TargetContainer -Blob $copyblobdetail.TargetBlob -Force

            $copyblobdetail.Status = "Canceled"
            $copyblobdetail.EndTime = Get-Date
        }
    }

    $copyblobdetails | ConvertTo-Json -Depth 100 | Out-File $BlobCopyFile
    $copyblobdetails | Sort-Object Status, TargetBlob | select TargetStorageAccount, TargetContainer, TargetBlob, Status, BytesCopied, TotalBytes, StartTime, EndTime | Format-Table -AutoSize

    # Delete used snapshots
    foreach ($copyblobdetail in $copyblobdetails)
    {
		if ($copyblobdetail.SourceStorageAccount -ne $null)
		{
			# Create source storage account context
			$source_context = New-AzStorageContext -StorageAccountName $copyblobdetail.SourceStorageAccount -StorageAccountKey $copyblobdetail.SourceKey -Environment $copyblobdetail.SourceEnvironment

			$source_container = Get-AzStorageContainer -Context $source_context -Name $copyblobdetail.SourceContainer
			$blobs = $source_container.CloudBlobContainer.ListBlobs($copyblobdetail.SourceBlob, $true, "Snapshots") | Where-Object { $_.SnapshotTime -ne $null -and $_.SnapshotTime.DateTime.ToString() -EQ $copyblobdetail.SnapshotTime }
			if ($blobs.Count > 0)
			{
				$blobs[0].Delete()
			}
		}
    }
}
}

if ($StartType -eq "" -and $BlobCopyFailure)
{
	Write-Host " - Skipping Resource Group Depoyment due to unsuccessful Blob Copy result" -ForegroundColor Yellow
}
elseif (($StartType -eq "" -and !$BlobCopyFailure) -or $StartType -eq "ResourceGroupDeployment" -or $StartType -eq "RetryDeployment")
{
	$ResourceGroupDeploymentError = $null

	Write-Host "Beginning Azure Resource Group Deployment" -ForegroundColor Green
	Write-Host " * ResourceGroupName '$($ResourceGroupName)'"
	Write-Host " * TemplateFile '$($TemplateFile)'"

	if ($TemplateParameterFile -eq $null -or $TemplateParameterFile -eq "")
	{
		Write-Host ""

		New-AzResourceGroupDeployment -ResourceGroupName $ResourceGroupName -TemplateFile $TemplateFile -ErrorAction Continue -ErrorVariable ResourceGroupDeploymentError
	}
	else
	{
		Write-Host " * TemplateParameterFile '$($TemplateParameterFile)'"
		Write-Host ""

		New-AzResourceGroupDeployment -ResourceGroupName $ResourceGroupName -TemplateFile $TemplateFile -TemplateParameterFile $TemplateParameterFile -ErrorAction Continue -ErrorVariable ResourceGroupDeploymentError
	}
}

If (($StartType -eq "" -and $ResourceGroupDeploymentError) -or $StartType -eq "TestResourceGroupDeployment")
{
	$TestResourceGroupDeploymentError = $false
	Write-Host "Resource Group Failed.  Testing Azure Resource Group Deployment for detailed results."

	if ($TemplateParameterFile -eq $null -or $TemplateParameterFile -eq "")
	{
		Write-Host ""

		Test-AzResourceGroupDeployment -ResourceGroupName $ResourceGroupName -TemplateFile $TemplateFile -Debug -ErrorAction Continue -ErrorVariable TestResourceGroupDeploymentError
	}
	else
	{
		Write-Host " * TemplateParameterFile '$($TemplateParameterFile)'"
		Write-Host ""

		Test-AzResourceGroupDeployment -ResourceGroupName $ResourceGroupName -TemplateFile $TemplateFile -TemplateParameterFile $TemplateParameterFile -Debug -ErrorAction Continue -ErrorVariable TestResourceGroupDeploymentError
	}
}

if (($StartType -eq "" -and !$ResourceGroupDeploymentError) -or $StartType -eq "DeleteMigAzTempStorage" -or $StartType -eq "RetryDeployment")
{
	Write-Host "- Removing MigAz Temporary Storage Account(s)"

	$distinctMigAzTempStorage = $copyblobdetails | ? { $_.TargetStorageAccount -like 'migaz*' } | select TargetStorageAccount -Unique
	foreach ($migAzTempStorage in $distinctMigAzTempStorage)
	{
		Write-Host " * Removing MigAz Temporary Storage Account '$($migAzTempStorage.TargetStorageAccount)'" -ForegroundColor Green
		Remove-AzStorageAccount -ResourceGroupName $ResourceGroupName -Name $migAzTempStorage.TargetStorageAccount -Confirm:$false
	}
}
