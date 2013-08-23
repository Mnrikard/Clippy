@echo off
c:\SourceCode\list\list\bin\Debug\list.exe
FOR /f "tokens=2,*" %%a IN ('reg query HKCU\Environment /v tempListDir') DO SET tempListDir=%%b

echo here
cd %tempListDir%
cls
@echo on
