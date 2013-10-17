@echo off
if exist "c:\Program Files (x86)\Clippy" (goto 64) else (goto 86)

:64
pushd c:\Program Files\Clippy
goto :run

:86
pushd c:\Program Files (x86)\Clippy
goto :run


:run
Clippy.exe %*
popd
@echo on
