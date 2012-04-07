@echo off
@pause Press Ctrl+C, this is build-all script
@rem Note that this script will only produce a .msi packet of your binaries and data. It will not compile your sources.
cd Builder
Builder IISMainHandler release
move /Y IISMainHandler\product-release.msi ..\IISMainHandler-release.msi
Builder IISMainHandler debug
move /Y IISMainHandler\product-debug.msi ..\IISMainHandler-debug.msi
rem Builder IISUploadHandler release
rem move /Y IISUploadHandler\product-release.msi ..\IISUploadHandler-release.msi
