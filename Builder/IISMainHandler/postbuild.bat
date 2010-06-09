@echo off
del Templates.7z
del Static.7z
ping localhost -n 2 >nul
del 7z.exe