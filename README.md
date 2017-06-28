# AzureWAF-chunked-upload-test

This is a minimal website to reporduce an error that occurs when chunking file uploads to an application that is behind an Azure Application Gateway + WAF

## What is the issue?

When uploading a large file using the JS File API to "chunk" the upload, passing through an Azure Application Gateway with a WAF in protection mode can sometimes cause a firewall rule to be hit. When this happens, one or more chunks of the file will faile to upload with the error `403 ModSecurity Action`. This issue does not happen for all chunks, but it is frequent enough that any 100mb file uploaded in 1 mb chunks is likely to fail.

The firewall rule enountered is `200003 MULTIPART_UNMATCHED_BOUNDARY`. There have been reports of false positives for this rule [as early as 2014](https://github.com/SpiderLabs/ModSecurity/issues/652), with many people complaining that their firewall providers do not let them configure this rule.  This rule is also not configurable in Azure Application Gateway, so if you encounter this issue, you are out of luck.

Tested in Chrome 55, IE 11

## How to reproduce the bug

These steps will skip a lot of the configuration involved in setting up the application gateway + VNET. If you are familiar with those technologies then you should not have issues with the configuration.

1. Create an Azure Application Gateway in a new resource group. The gateway needs to have WAF enabled and in protection mode.
1. In the same VNET as the Application Gateway, create a Windows Server 2016 VM, add IIS to the VM and install the [.Net Core Windows Server Hosting Runtime](https://www.microsoft.com/net/download/core#/runtime)
1. Clone this repository and publish the website to file.
1. Copy the published website to the VM, and replace the default IIS Application with this website.
1. Add a listener to the Application Gateway so that you can navigate to the website externally.
1. Launch the website.

At this point you will see upload forms. The top one uses chunked uploading (via [resumable.js](http://www.resumablejs.com/)) and the bottom one is a standard upload via a post action. This is provided only for comparison, I have not been able to reproduce the issue with post action-based uploads.

Upload any file you want to see the error. Not all file have the issue but the chances increase as the size of the file increases. An example file that does reliably cause the issue is the [.Net Microservices Ebook](http://aka.ms/MicroservicesEbook).

It is important to note that the server backend is mocked and all files uploaded are immediately discarded.

## Found the problem?

If anyone finds a way to either change the configuration of the javascript upload api, or of the WAF so that this issue is fixed, please raise an issue at [https://github.com/elexisvenator/AzureWAF-chunked-upload-test/issues](https://github.com/elexisvenator/AzureWAF-chunked-upload-test/issues) so that everyone can fix the issue for themselves.