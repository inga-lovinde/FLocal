@echo off
@rem Note that this script will only produce a .msi packet of your binaries and data. It will not compile your sources.
cd Builder
Builder IISMainHandler
move /Y IISMainHandler\product.msi ..\IISMainHandler.msi
Builder IISUploadHandler
move /Y IISUploadHandler\product.msi ..\IISUploadHandler.msi