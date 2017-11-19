# Parameters
param (
    [Parameter(Mandatory = $true)] 
    $ResourceGroupName,

    [Parameter(Mandatory = $true)] 
    $TemplateFile,

    [Parameter(Mandatory = $false)] 
    $TemplateParameterFile,

    [Parameter(Mandatory = $false)] 
    $BlobCopyFile,

    [ValidateSet("StartBlobCopy", "MonitorBlobCopy", "CancelBlobCopy", "ResourceGroupDeployment", "DeleteMigAzTempStorage")]
    [Parameter(Mandatory = $false)] 
    $StartType,

    $RefreshInterval = 10
)

# Check Azure.Storage version - minimum 2.0.1 required
if ((get-command New-AzureStorageContext).Version.ToString() -lt '2.0.1')
{
    Write-Host 'Please update Azure.Storage module to 2.0.1 or higher before running this script' -ForegroundColor Yellow
    Exit
}

# Check AzureRM.Storage version - minimum 2.0.1 required
if ((get-command Get-AzureRmStorageAccountKey).Version.ToString() -lt '2.0.1')
{
    Write-Host 'Please update AzureRM.Storage module to 2.0.1 or higher before running this script' -ForegroundColor Yellow
    Exit
}

# Load blob copy details file (ex: copyblobdetails.json)
$copyblobdetails = Get-Content -Path $BlobCopyFile -Raw | ConvertFrom-Json
$copyblobdetailsout = @()

$StartBlobCopySuccess = $false
$MonitorBlobCopySuccess = $false
$ResourceGroupDeploymentError = $false

# If Initiating the copy of all blobs
If ($StartType -eq $null -or $StartType -eq "StartBlobCopy")
{
	$StartBlobCopyError = $false
	$sourceExpirationValid = $true
	Write-Host "Validating Blob Source Expiration (if applicable)"

    # Validate Source Expiration Date(s)
    foreach ($copyblobdetail in $copyblobdetails)
    {
		if ($copyblobdetail.SourceExpiration -ne $null)
		{
			$sourceExpirationDate = [datetime] $copyblobdetail.SourceExpiration
			if ($sourceExpirationDate -le (Get-Date))
			{
				$sourceExpirationValid = $false
				$StartBlobCopyError = $true
				Write-Host " - ERROR: '$($copyblobdetail.TargetBlob)' source access has expired ($($sourceExpirationDate))." -ForegroundColor Red
			}
			else
			{
			$TimeDiff = New-TimeSpan (Get-Date) $sourceExpirationDate

				Write-Host " - WARNING: '$($copyblobdetail.TargetBlob)' source access expires in $($TimeDiff.Minutes) minute(s) ($($sourceExpirationDate))." -ForegroundColor Yellow
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
        $targetStorageAccount = Get-AzureRmStorageAccount -ResourceGroupName $copyblobdetail.TargetResourceGroup -Name $copyblobdetail.TargetStorageAccount -ErrorAction SilentlyContinue
        if ($targetStorageAccount -eq $null)
        {
            Write-Host "Target Storage Account '$($copyblobdetail.TargetStorageAccount)' not found.  Attempting to create."
            New-AzureRmStorageAccount -ResourceGroupName $copyblobdetail.TargetResourceGroup -Name $copyblobdetail.TargetStorageAccount -Location $copyblobdetail.TargetLocation -SkuName $copyblobdetail.TargetStorageAccountType -ErrorAction Stop
        }

		# Create destination storage account context
		$copyblobdetail.TargetKey = (Get-AzureRmStorageAccount -ResourceGroupName $copyblobdetail.TargetResourceGroup -Name $copyblobdetail.TargetStorageAccount | Get-AzureRmStorageAccountKey).Value[0]
		$destination_context = New-AzureStorageContext -StorageAccountName $copyblobdetail.TargetStorageAccount -StorageAccountKey $copyblobdetail.TargetKey -ErrorAction Stop

		# Create destination container if it does not exist
		$destination_container = Get-AzureStorageContainer -Context $destination_context -Name $copyblobdetail.TargetContainer -ErrorAction SilentlyContinue
		if ($destination_container -eq $null)
		{ 
			New-AzureStorageContainer -Context $destination_context -Name $copyblobdetail.TargetContainer
		}

		if ($copyblobdetail.SourceAbsoluteUri -eq $null)
		{
			# Create source storage account context
			$source_context = New-AzureStorageContext -StorageAccountName $copyblobdetail.SourceStorageAccount -StorageAccountKey $copyblobdetail.SourceKey -Environment $copyblobdetail.SourceEnvironment -ErrorAction Stop
    
			#Get a reference to a blob
			$blob = Get-AzureStorageBlob -Context $source_context -Container $copyblobdetail.SourceContainer -Blob $copyblobdetail.SourceBlob -ErrorAction Stop
			#Create a snapshot of the blob
			$snap = $blob.ICloudBlob.CreateSnapshot()
			$copyblobdetail.SnapshotTime = $snap.SnapshotTime.DateTime.ToString()
			# Get just created snapshot
			$snapshot = Get-AzureStorageBlob -Context $source_context -Container $copyblobdetail.SourceContainer -Prefix $copyblobdetail.SourceBlob | Where-Object  { $_.Name -eq $copyblobdetail.SourceBlob -and $_.SnapshotTime -eq $snap.SnapshotTime } 
			# Convert to CloudBlob object type
			$snapshot = [Microsoft.WindowsAzure.Storage.Blob.CloudBlob] $snapshot.ICloudBlob
            
			# Initiate blob snapshot copy job
			Start-AzureStorageBlobCopy -Context $source_context -ICloudBlob $snapshot -DestContext $destination_context -DestContainer $copyblobdetail.TargetContainer -DestBlob $copyblobdetail.TargetBlob
		}
		else
		{
			# Initiate blob snapshot copy job
			Start-AzureStorageBlobCopy -AbsoluteUri $copyblobdetail.SourceAbsoluteUri -DestContext $destination_context -DestContainer $copyblobdetail.TargetContainer -DestBlob $copyblobdetail.TargetBlob
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

	if ($StartBlobCopyError -eq $false)
	{
		$StartBlobCopySuccess = $true
	}
}

# If waiting for all blobs to copy and get statistics
If (($StartType -eq $null -and $StartBlobCopySuccess -eq $true) -or $StartType -eq "MonitorBlobCopy")
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
                $destination_context = New-AzureStorageContext -StorageAccountName $copyblobdetail.TargetStorageAccount -StorageAccountKey $copyblobdetail.TargetKey
                $status = Get-AzureStorageBlobCopyState -Context $destination_context -Container $copyblobdetail.TargetContainer -Blob $copyblobdetail.TargetBlob

                $copyblobdetail.TotalBytes = "{0:N0} MB" -f ($status.TotalBytes / 1MB)
                $copyblobdetail.BytesCopied = "{0:N0} MB" -f ($status.BytesCopied / 1MB)
                $copyblobdetail.Status = $status.Status
				$copyblobdetail.StatusDescription = $status.StatusDescription

                $ContinueBlobCopy = $true

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
            $source_context = New-AzureStorageContext -StorageAccountName $copyblobdetail.SourceStorageAccount -StorageAccountKey $copyblobdetail.SourceKey -Environment $copyblobdetail.SourceEnvironment

            $source_container = Get-AzureStorageContainer -Context $source_context -Name $copyblobdetail.SourceContainer
            $blobs = $source_container.CloudBlobContainer.ListBlobs($copyblobdetail.SourceBlob, $true, "Snapshots") | Where-Object { $_.SnapshotTime -ne $null -and $_.SnapshotTime.DateTime.ToString() -EQ $copyblobdetail.SnapshotTime }

			if ($blobs.Count > 0)
			{
				$blobs[0].Delete()
			}
        }
    }

	if ($BlobCopyFailure -eq $false)
	{
		$MonitorBlobCopySuccess = $true
	}
}


# If to Cancel blobs copy
If (($StartType -eq $null -and $MonitorBlobCopySuccess -eq $false) -or $StartType -eq "CancelBlobCopy")
{
    foreach ($copyblobdetail in $copyblobdetails)
    {
        if ($copyblobdetail.Status -ne "Success" -and $copyblobdetail.Status -ne "Failed")
        {
			Write-Host " - Stopping Blob Copy '$($copyblobdetail.TargetBlob)'"

            $destination_context = New-AzureStorageContext -StorageAccountName $copyblobdetail.TargetStorageAccount -StorageAccountKey $copyblobdetail.TargetKey
            Stop-AzureStorageBlobCopy -Context $destination_context -Container $copyblobdetail.TargetContainer -Blob $copyblobdetail.TargetBlob -Force

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
			$source_context = New-AzureStorageContext -StorageAccountName $copyblobdetail.SourceStorageAccount -StorageAccountKey $copyblobdetail.SourceKey -Environment $copyblobdetail.SourceEnvironment

			$source_container = Get-AzureStorageContainer -Context $source_context -Name $copyblobdetail.SourceContainer
			$blobs = $source_container.CloudBlobContainer.ListBlobs($copyblobdetail.SourceBlob, $true, "Snapshots") | Where-Object { $_.SnapshotTime -ne $null -and $_.SnapshotTime.DateTime.ToString() -EQ $copyblobdetail.SnapshotTime }
			if ($blobs.Count > 0)
			{
				$blobs[0].Delete()
			}
		}
    }
}

if ($StartType -eq $null -and $MonitorBlobCopySuccess -eq $false)
{
	Write-Host " - Skipping Resource Group Depoyment due to unsuccessful Blob Copy result" -ForegroundColor Yellow
}
elseif (($StartType -eq $null -and $MonitorBlobCopySuccess -eq $true) -or $StartType -eq "ResourceGroupDeployment")
{
	Write-Host " - Beginning Azure Resource Group Deployment" -ForegroundColor Green
	Write-Host " * ResourceGroupName '$($ResourceGroupName)'"
	Write-Host " * TemplateFile '$($TemplateFile)'"

	if ($TemplateParameterFile -eq $null)
	{
		Write-Host ""

		New-AzureRmResourceGroupDeployment -ResourceGroupName $ResourceGroupName -TemplateFile $TemplateFile -ErrorAction Stop
	}
	else
	{
		Write-Host " * TemplateParameterFile '$($TemplateParameterFile)'"
		Write-Host ""

		New-AzureRmResourceGroupDeployment -ResourceGroupName $ResourceGroupName -TemplateFile $TemplateFile -TemplateParameterFile $TemplateParameterFile -ErrorAction Stop
	}

	$ResourceGroupDeploymentError = $true
}

if (($StartType -eq $null -and $ResourceGroupDeploymentError -eq $false) -or $StartType -eq "DeleteMigAzTempStorage")
{
	Write-Host "- Removing MigAz Temporary Storage Account(s)"

	$distinctMigAzTempStorage = $copyblobdetails | ? { $_.TargetStorageAccount -like 'migaz*' } | select TargetStorageAccount -Unique
	foreach ($migAzTempStorage in $distinctMigAzTempStorage)
	{
		Write-Host " * Removing MigAz Temporary Storage Account '$($migAzTempStorage.TargetStorageAccount)'" -ForegroundColor Green
		Remove-AzureRmStorageAccount -ResourceGroupName $ResourceGroupName -Name $migAzTempStorage.TargetStorageAccount 
	}
}
