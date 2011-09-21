REM The following batch file will execute the unit tests with Code Coverage enabled.
REM This ASSUMES that you have built an instrumented DLL, and deployed it to the GAC and other locations.
REM
REM This should be executed from a VS2010 command prompt, on a machine with SSIS 2008 installed.
REM
Start vsperfmon -coverage -output:MultipleHash.coverage
rem mstest /testcontainer:bin/UnitTest/MultipleHash2008Test.dll
REM Test Normal Code
"%programfiles%\Microsoft SQL Server\100\DTS\binn\dtexec" /File "..\SSIS2008TestMultipleHash\EveryDataType.dtsx"
IF ERRORLEVEL 1 GOTO Failed
ECHO This should exit cleanly, without errors!
REM Test Upgrade Code
"%programfiles%\Microsoft SQL Server\100\DTS\binn\dtexec" /File "..\SSIS2008TestMultipleHash\EveryDataType_V1.3.1.dtsx"
IF ERRORLEVEL 1 GOTO Failed
ECHO This should exit cleanly, without errors!
"%programfiles%\Microsoft SQL Server\100\DTS\binn\dtexec" /File "..\SSIS2008TestMultipleHash\EveryDataType_MissingProperties.dtsx"
IF ERRORLEVEL 1 GOTO Failed
ECHO This should exit with errors!
"%programfiles%\Microsoft SQL Server\100\DTS\binn\dtexec" /File "..\SSIS2008TestMultipleHash\EveryDataType_BadLineage.dtsx" > BadLineage.log
IF %ERRORLEVEL% EQU 0 GOTO Failed
FINDSTR /I /L  /C:"The output property InputColumnLineageIDs" BadLineage.log
IF ERRORLEVEL 1 GOTO Failed
ECHO This should exit with errors!
"%programfiles%\Microsoft SQL Server\100\DTS\binn\dtexec" /File "..\SSIS2008TestMultipleHash\EveryDataType_BadRuntime.dtsx" > BadRuntime.log
IF %ERRORLEVEL% EQU 0 GOTO Failed
FINDSTR /I /L  /C:"The version or pipeline version" BadRuntime.log
IF ERRORLEVEL 1 GOTO Failed

ECHO DTExec Tests Passed!
GOTO Finish
:Failed
ECHO DTExec Tests Failed!
:Finish
vsperfcmd -shutdown