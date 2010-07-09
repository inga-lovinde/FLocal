@echo off
@rem Note that this script will only produce a .msi packet of your binaries and data. It will not compile your sources.
cd Builder
Builder IISMainHandler debug
move /Y IISMainHandler\product-debug.msi ..\IISMainHandler-debug.msi
Builder IISMainHandler release
move /Y IISMainHandler\product-release.msi ..\IISMainHandler-release.msi
rem Builder IISUploadHandler release
rem move /Y IISUploadHandler\product-release.msi ..\IISUploadHandler-release.msi