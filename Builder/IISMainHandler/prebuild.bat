@echo off
"C:\Program Files\7-Zip\7z.exe" a Templates.7z ..\..\templates\* -xr!.svn
"C:\Program Files\7-Zip\7z.exe" a Static.7z ..\..\static\* -xr!.svn
copy "C:\Program Files\7-Zip\7z.exe" 7z.exe