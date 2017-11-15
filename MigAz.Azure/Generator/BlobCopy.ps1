# Parameters
param (
    [Parameter(Mandatory = $true)] 
    $DetailsFilePath,

    [ValidateSet("StartBlobCopy", "MonitorBlobCopy", "CancelBlobCopy")]
    [Parameter(Mandatory = $true)] 
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
$copyblobdetails = Get-Content -Path $DetailsFilePath -Raw | ConvertFrom-Json
$copyblobdetailsout = @()


# If Initiating the copy of all blobs
If ($StartType -eq "StartBlobCopy")
{
    # Initiate the copy of all blobs
    foreach ($copyblobdetail in $copyblobdetails)
    {
        # Ensure the Storage Account Exists
        $targetStorageAccount = Get-AzureRmStorageAccount -ResourceGroupName $copyblobdetail.TargetResourceGroup -Name $copyblobdetail.TargetStorageAccount -ErrorAction SilentlyContinue
        if ($targetStorageAccount -eq $null)
        {
            Write-Host "Target Storage Account '$($copyblobdetails.TargetStorageAccount)' not found.  Attempting to create."
            New-AzureRmStorageAccount -ResourceGroupName $copyblobdetail.TargetResourceGroup -Name $copyblobdetail.TargetStorageAccount -Location $copyblobdetail.TargetLocation -SkuName $copyblobdetail.TargetStorageAccountType
        }

		# Create destination storage account context
		$copyblobdetail.TargetKey = (Get-AzureRmStorageAccount -ResourceGroupName $copyblobdetail.TargetResourceGroup -Name $copyblobdetail.TargetStorageAccount | Get-AzureRmStorageAccountKey).Value[0]
		$destination_context = New-AzureStorageContext -StorageAccountName $copyblobdetail.TargetStorageAccount -StorageAccountKey $copyblobdetail.TargetKey

		# Create destination container if it does not exist
		$destination_container = Get-AzureStorageContainer -Context $destination_context -Name $copyblobdetail.TargetContainer -ErrorAction SilentlyContinue
		if ($destination_container -eq $null)
		{ 
			New-AzureStorageContainer -Context $destination_context -Name $copyblobdetail.TargetContainer
		}

		if ($copyblobdetail.SourceAbsoluteUri -eq $null)
		{
			# Create source storage account context
			$source_context = New-AzureStorageContext -StorageAccountName $copyblobdetail.SourceStorageAccount -StorageAccountKey $copyblobdetail.SourceKey -Environment $copyblobdetail.SourceEnvironment
    
			#Get a reference to a blob
			$blob = Get-AzureStorageBlob -Context $source_context -Container $copyblobdetail.SourceContainer -Blob $copyblobdetail.SourceBlob
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
		$copyblobdetail.StartTime = Get-Date -Format u
		$copyblobdetailsout += $copyblobdetail

        # Updates screen table
        # cls
        $copyblobdetails | select TargetStorageAccount, TargetContainer, TargetBlob, Status, BytesCopied, TotalBytes, StartTime, EndTime | Format-Table -AutoSize
    }

    # Updates file with data from $copyblobdetailsout
    $copyblobdetailsout | ConvertTo-Json -Depth 100 | Out-File $DetailsFilePath
}

# If waiting for all blobs to copy and get statistics
If ($StartType -eq "MonitorBlobCopy")
{
    # Waits for all blobs to copy and get statistics
    $continue = $true
    while ($continue)
    {
        $continue = $false
        foreach ($copyblobdetail in $copyblobdetails)
        {
            if ($copyblobdetail.Status -ne "Success" -and $copyblobdetail.Status -ne "Failed")
            {
                $destination_context = New-AzureStorageContext -StorageAccountName $copyblobdetail.TargetStorageAccount -StorageAccountKey $copyblobdetail.TargetKey
                $status = Get-AzureStorageBlobCopyState -Context $destination_context -Container $copyblobdetail.TargetContainer -Blob $copyblobdetail.TargetBlob

                $copyblobdetail.TotalBytes = "{0:N0} MB" -f ($status.TotalBytes / 1MB)
                $copyblobdetail.BytesCopied = "{0:N0} MB" -f ($status.BytesCopied / 1MB)
                $copyblobdetail.Status = $status.Status
                $copyblobdetail.EndTime = Get-Date -Format u

                $continue = $true
            }
        }

        $copyblobdetails | ConvertTo-Json -Depth 100 | Out-File $DetailsFilePath
        # cls
        $copyblobdetails | select TargetStorageAccount, TargetContainer, TargetBlob, Status, BytesCopied, TotalBytes, StartTime, EndTime | Format-Table -AutoSize

        Start-Sleep -Seconds $refreshinterval
    }

    # Delete used snapshots
    foreach ($copyblobdetail in $copyblobdetails)
    {
        # Create source storage account context
        if ($copyblobdetail.SourceStorageAccount -ne $null)
        {
            $source_context = New-AzureStorageContext -StorageAccountName $copyblobdetail.SourceStorageAccount -StorageAccountKey $copyblobdetail.SourceKey -Environment $copyblobdetail.SourceEnvironment

            $source_container = Get-AzureStorageContainer -Context $source_context -Name $copyblobdetail.SourceContainer
            $blobs = $source_container.CloudBlobContainer.ListBlobs($copyblobdetail.SourceBlob, $true, "Snapshots") | Where-Object { $_.SnapshotTime -ne $null -and $_.SnapshotTime.DateTime.ToString() -EQ $copyblobdetail.SnapshotTime }
            $blobs[0].Delete()
        }
    }
}


# If to Cancel blobs copy
If ($StartType -eq "CancelBlobCopy")
{
    foreach ($copyblobdetail in $copyblobdetails)
    {
        if ($copyblobdetail.Status -ne "Success" -and $copyblobdetail.Status -ne "Failed")
        {
            $destination_context = New-AzureStorageContext -StorageAccountName $copyblobdetail.TargetStorageAccount -StorageAccountKey $copyblobdetail.TargetKey
            Stop-AzureStorageBlobCopy -Context $destination_context -Container $copyblobdetail.TargetContainer -Blob $copyblobdetail.TargetBlob -Force -Verbose

            $copyblobdetail.Status = "Canceled"
            $copyblobdetail.EndTime = Get-Date -Format u
        }
    }

    $copyblobdetails | ConvertTo-Json -Depth 100 | Out-File $DetailsFilePath
    # cls
    $copyblobdetails | select TargetStorageAccount, TargetContainer, TargetBlob, Status, BytesCopied, TotalBytes, StartTime, EndTime | Format-Table -AutoSize

    # Delete used snapshots
    foreach ($copyblobdetail in $copyblobdetails)
    {
        # Create source storage account context
        $source_context = New-AzureStorageContext -StorageAccountName $copyblobdetail.SourceStorageAccount -StorageAccountKey $copyblobdetail.SourceKey -Environment $copyblobdetail.SourceEnvironment

        $source_container = Get-AzureStorageContainer -Context $source_context -Name $copyblobdetail.SourceContainer
        $blobs = $source_container.CloudBlobContainer.ListBlobs($copyblobdetail.SourceBlob, $true, "Snapshots") | Where-Object { $_.SnapshotTime -ne $null -and $_.SnapshotTime.DateTime.ToString() -EQ $copyblobdetail.SnapshotTime }
        $blobs[0].Delete()
    }
}
