REM The following batch file will execute the unit tests with Code Coverage enabled.
REM This ASSUMES that you have built an instrumented DLL, and deployed it to the GAC and other locations.
REM
REM This should be executed from a VS2010 command prompt, on a machine with SSIS 2008 installed.
REM
Start vsperfmon -coverage -output:MultipleHash.coverage
mstest /testcontainer:bin/UnitTest/MultipleHash2008Test.dll
REM Test Normal Code
"%programfiles%\Microsoft SQL Server\100\DTS\binn\dtexec" /File "..\SSIS2008TestMultipleHash\EveryDataType.dtsx"
IF ERRORLEVEL 1 GOTO Failed
"%programfiles%\Microsoft SQL Server\100\DTS\binn\dtexec" /File "..\SSIS2008TestMultipleHash\EveryDataTypePrecisionTest.dtsx"
IF ERRORLEVEL 1 GOTO Failed
"%programfiles%\Microsoft SQL Server\100\DTS\binn\dtexec" /File "..\SSIS2008TestMultipleHash\TestStringGT1000Bytes.dtsx"
IF ERRORLEVEL 1 GOTO Failed
"%programfiles%\Microsoft SQL Server\100\DTS\binn\dtexec" /File "..\SSIS2008TestMultipleHash\EveryDataType_V1.3.1.dtsx"
IF ERRORLEVEL 1 GOTO Failed
"%programfiles%\Microsoft SQL Server\100\DTS\binn\dtexec" /File "..\SSIS2008TestMultipleHash\EveryDataType_MissingProperties.dtsx" > Testing.log
IF %ERRORLEVEL% EQU 0 GOTO Failed
FINDSTR /I /L  /C:"The count of SafeNullHandling properties is not valid" Testing.log
IF ERRORLEVEL 1 GOTO Failed
"%programfiles%\Microsoft SQL Server\100\DTS\binn\dtexec" /File "..\SSIS2008TestMultipleHash\EveryDataType_BadLineage.dtsx" > Testing.log
IF %ERRORLEVEL% EQU 0 GOTO Failed
FINDSTR /I /L  /C:"The output property InputColumnLineageIDs" Testing.log
IF ERRORLEVEL 1 GOTO Failed
"%programfiles%\Microsoft SQL Server\100\DTS\binn\dtexec" /File "..\SSIS2008TestMultipleHash\EveryDataType_BadRuntime.dtsx" > Testing.log
IF %ERRORLEVEL% EQU 0 GOTO Failed
FINDSTR /I /L  /C:"The version or pipeline version" Testing.log
IF ERRORLEVEL 1 GOTO Failed
ECHO Please Test other UI functions Manually Here!
type ManualTestCases.txt
"%programfiles%\Microsoft Visual Studio 9.0\Common7\IDE\DevEnv.exe"
Pause
ECHO DTExec Tests Passed!
GOTO Finish
:Failed
ECHO DTExec Tests Failed!
:Finish
vsperfcmd -shutdown