@echo off
"C:\Program Files\7-Zip\7z.exe" a Templates.7z ..\..\templates\* -xr!.svn
copy "C:\Program Files\7-Zip\7z.exe" 7z.exe