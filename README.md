Hello Azure expert!

This repo is the home for the MigAz. MigAz intent is to help you on Azure Resource Manager (ARM) deployments and migrations.

The base principal of MigAz tools is to work on top of ARM templates. This creates the foundations for you to create, change and migrate deployments with the power and flexibility of ARM templates deployment model. This is true if you are now starting to work with ARM templates and want to learn, or you already have advanced knowledge but need a tool to make you even more productive.

All versions (except ASM Native) work the same way. Connects to the source environment, allows you to select the objects you need to export and create an ARM template from it. With this ARM template you can now make changes, clone or migrate an environment.
Have a look to each ASM, ARM and AWS versions and see where it helps you.

Give us feedback so we continue to improve this tools and, specially, help us by submitting your own updates. Remember this are community tools that depend deeply on your feedback and code contributions.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/). For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

## Pre-requisites
1. Windows 8 or Windows Server 2012, or later
2. .Net Framework 4.0 or higher
3. Latest [Azure PowerShell Module](https://azure.microsoft.com/en-us/documentation/articles/powershell-install-configure)
4. Login credentials at source Azure subscription
5. Login credentials at destination Azure subscription

## Get it
Download the latest master repository and open the solution with Visual Studio 2017 or download compiled versions from the [MigAz GitHub Release](https://github.com/Azure/migAz/releases).  Note that Migaz AWS to ARM is not yet integrated into the new v2 project baseline and remains available via the AWS release folders ([AWS Release](aws/release)).

## Instructions
[Azure ASM to Azure ARM Migration](asm) - MigAz ASM to ARM will help you on your ASM to ARM migrations. Allows you to create an ARM template out of an Azure IaaS ASM deployment. It also provides an automation script for blobs copy.

[Azure ARM to Azure ARM Migration](arm) - MigAz ARM to ARM version allows you to create an ARM template out of an Azure IaaS ARM deployment. It also provides an automation script for blobs copy.

[AWS to Azure ARM Migration](aws) - MigAz AWS version will help you on your AWS to Azure migrations. MigAz AWS version allows you to create an ARM template out of an AWS EC2 deployment.

## Troubleshooting
The detailed logs and output of the REST API are captured in the location %USERPROFILE%\appdata\Local with the file name MigAz-&lt;YYYYMMDD&gt;.log and MigAz-XML-&lt;YYYYMMDD&gt;.log.
In case of any issues during the deployment of the export.JSON you need to troubleshoot the template properties and fix the invalid entries. Report any issue on the tool site.


## Release Notes
As of v2.4.0.0, all release notes and compiled versions will be made available via GitHub Releases.  Please visit the [MigAz GitHub Release](https://github.com/Azure/migAz/releases) page for all releases and compiled versions.

