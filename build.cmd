@echo off
setlocal

set _args=%*
if "%~1"=="-?" set _args=-help
if "%~1"=="/?" set _args=-help

if ["%_args%"] == [""] (
    :: If perform restore and build, if no args are supplied.
    set _args=-restore -build
)

powershell -ExecutionPolicy ByPass -NoProfile -command "& """%~dp0eng\common\Build.ps1""" %_args%"
exit /b %ERRORLEVEL%