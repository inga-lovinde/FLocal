@echo off
del Resources.FLocal.Templates.7z
del Resources.FLocal.Static.7z
ping localhost -n 2 >nul
del 7z.exe