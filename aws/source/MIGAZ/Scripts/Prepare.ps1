# v1.0.0.1
# Parameters
param (
    [Parameter(Mandatory = $true)] 
    $ResourcegroupName,

    [Parameter(Mandatory = $true)] 
    $TemplateFilePath,

    [ValidateSet("StopVirtualMachines", "DeleteVirtualMachines", "UpdateTemplateDisks")]
    [Parameter(Mandatory = $true)] 
    $Action,

    $Sufix = ""
)

# If action is to update template disks
If ($Action -eq "UpdateTemplateDisks")
{
    $disks = @()

    $virtualmachines = Get-AzureRmVM -ResourceGroupName $ResourcegroupName
    foreach ($virtualmachine in $virtualmachines)
    {
        $vmname = $virtualmachine.Name
        $vmname = $vmname.Substring(0,$vmname.Length-$Sufix.Length)

        $disk = New-Object System.Object
        $disk | Add-Member -type NoteProperty -name vmname -value $vmname
        $disk | Add-Member -type NoteProperty -name lun -value "OS"
        $disk | Add-Member -type NoteProperty -name url -value $virtualmachine.StorageProfile.OsDisk.Vhd.Uri
        $disks += $disk

        foreach ($datadisk in $virtualmachine.StorageProfile.DataDisks)
        {
            $disk = New-Object System.Object
            $disk | Add-Member -type NoteProperty -name vmname -value $vmname
            $disk | Add-Member -type NoteProperty -name lun -value $datadisk.Lun
            $disk | Add-Member -type NoteProperty -name url -value $datadisk.Vhd.Uri
            $disks += $disk
        }
    }

    $template = Get-Content $TemplateFilePath | ConvertFrom-Json
    for($i=0; $i -lt $resources.resources.Count; $i++)
    {
        $resource = $template.resources[$i]
        if ($resource.type -eq 'Microsoft.Compute/virtualMachines')
        {
            $template.resources[$i].properties.storageProfile.osDisk.vhd.uri = GetDiskUrl -vmname $resource.name -lun 'OS'

            for($lunindex=0; $lunindex -lt $resource.properties.storageProfile.dataDisks.Count; $lunindex++)
            {
                $template.resources[$i].properties.storageProfile.dataDisks[$lunindex].vhd.uri = GetDiskUrl -vmname $resource.name -lun $lunindex.ToString()
            }
        }
    }

    $templatetext = $template | ConvertTo-Json -Depth 100
    $templatetext = $templatetext.Replace("\u0027", "'")
    $templatetext | Out-File $TemplateFilePath
}

# If action is to stop virtual machines
If ($Action -eq "StopVirtualMachines")
{

    $resources = Get-Content $TemplateFilePath | ConvertFrom-Json
    $virtualmachines = $resources.resources | ? type -EQ 'Microsoft.Compute/virtualMachines'
    foreach ($virtualmachine in $virtualmachines)
    {
        $vmname = $virtualmachine.Name+$Sufix
        Stop-AzureRmVM -ResourceGroupName $ResourcegroupName -Name $vmname -Force -StayProvisioned
    }
}

# If action is to delete virtual machines and network interfaces
If ($Action -eq "DeleteVirtualMachines")
{
    $resources = Get-Content $TemplateFilePath | ConvertFrom-Json
    $virtualmachines = $resources.resources | ? type -EQ 'Microsoft.Compute/virtualMachines'
    foreach ($virtualmachine in $virtualmachines)
    {
        $vmname = $virtualmachine.Name+$Sufix
        $vm = Get-AzureRmVM -ResourceGroupName $ResourcegroupName -Name ($vmname)
        $nics = $vm.NetworkProfile.NetworkInterfaces
        Remove-AzureRmVM -ResourceGroupName $ResourcegroupName -Name ($vmname) -Force -Verbose

        foreach ($nic in $nics)
        {
            $nicarray = $nic.Id.Split("/")
            $nicname = $nicarray[$nicarray.Count-1]
            Remove-AzureRmNetworkInterface -ResourceGroupName $ResourcegroupName -Name $nicname -Force -Verbose
        }
    }
}

Function GetDiskUrl($vmname, $lun)
{
    foreach($disk in $disks)
    {
        if ($disk.vmname -eq $vmname -and $disk.lun -eq $lun)
        {
            return $disk.url
        }
    }
}
