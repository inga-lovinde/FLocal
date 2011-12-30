@echo off
"C:\Program Files\7-Zip\7z.exe" a Resources.FLocal.Templates.7z ..\..\Resources\FLocal\templates\* -xr!.svn
"C:\Program Files\7-Zip\7z.exe" a Resources.FLocal.Static.7z ..\..\Resources\FLocal\static\* -xr!.svn
copy "C:\Program Files\7-Zip\7z.exe" 7z.exe
exit
