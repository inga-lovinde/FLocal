@echo off
cd Builder
Builder IISMainHandler
move /Y IISMainHandler\product.msi ..\IISMainHandler.msi