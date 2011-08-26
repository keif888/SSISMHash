REM The following batch file will execute the unit tests with Code Coverage enabled.
REM This ASSUMES that you have built an instrumented DLL, and deployed it to the GAC and other locations.
REM
REM This should be executed from a VS2010 command prompt, on a machine with SSIS 2008 installed.
REM
Start vsperfmon -coverage -output:MultipleHash.coverage
mstest /testcontainer:bin/UnitTest/MultipleHash2008Test.dll
"%programfiles%\Microsoft SQL Server\100\DTS\binn\dtexec" /File "..\SSIS2008TestMultipleHash\SimpleTextTest.dtsx"
"%programfiles%\Microsoft SQL Server\100\DTS\binn\dtexec" /File "..\SSIS2008TestMultipleHash\EveryDataType.dtsx"


vsperfcmd -shutdown