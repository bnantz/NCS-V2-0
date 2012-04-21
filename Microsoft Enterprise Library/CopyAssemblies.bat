@echo off
@REM  ----------------------------------------------------------------------------
@REM  CopyAssemblies.bat file
@REM
@REM  This batch file copies the Enterprise Library assemblies from their build
@REM  location to a common destination folder.
@REM  
@REM  Optional arguments for this batch file:
@REM   1 - The build output folder (Release, Debug, etc. Defaults to Debug)
@REM   2 - The destination folder (i.e. where the build will be dropped.
@REM       Defaults to ..\bin)
@REM  
@REM  ----------------------------------------------------------------------------

echo.
echo =========================================================
echo   CopyAssemblies                                         
echo      Copies EnterpriseLibrary assemblies to a single    
echo      destination directory                              
echo =========================================================
echo.

set solutionDir=.
set buildType=Debug
set binDir=bin
set pause=true

if "%1"=="/?" goto HELP

if not Exist %solutionDir%\EnterpriseLibrary.VSTS.sln goto HELP

@REM  ----------------------------------------------------
@REM  If the first parameter is /q, do not pause
@REM  at the end of execution.
@REM  ----------------------------------------------------

if /i "%1"=="/q" (
 set pause=false
 SHIFT
)

@REM  ------------------------------------------------------
@REM  User can override default build type by specifiying
@REM  a parameter to batch file (e.g. CopyAssemblies Release).
@REM  ------------------------------------------------------

if not "%1"=="" set buildType=%1

@REM  ---------------------------------------------------------------
@REM  User can override default destination directory by specifiying
@REM  a parameter to batch file (e.g. CopyAssemblies Debug c:\bin).
@REM  ---------------------------------------------------------------

if not "%2"=="" set binDir=%2

@REM  ----------------------------------------
@REM  Shorten the command prompt for output
@REM  ----------------------------------------
set savedPrompt=%prompt%
set prompt=*$g


@ECHO ----------------------------------------
@ECHO CopyAssemblies.bat Started
@ECHO ----------------------------------------
@ECHO.

@REM -------------------------------------------------------
@REM Change to the directory where the solution file resides
@REM -------------------------------------------------------

pushd %solutionDir%

@ECHO.
@ECHO ----------------------------------------
@ECHO Create destination folder 
@ECHO ----------------------------------------
@ECHO.

if not Exist %binDir% mkdir %binDir%

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy ObjectBuilder files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist Src\ObjectBuilder\bin\%buildType%\Microsoft.Practices.ObjectBuilder.dll copy /V Src\ObjectBuilder\bin\%buildType%\Microsoft.Practices.ObjectBuilder.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\ObjectBuilder\bin\%buildType%\Microsoft.Practices.ObjectBuilder.xml copy /V Src\ObjectBuilder\bin\%buildType%\Microsoft.Practices.ObjectBuilder.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Caching files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist Src\Caching\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.dll copy /V Src\Caching\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Caching\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.xml copy /V Src\Caching\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Caching\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.dll copy /V Src\Caching\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Caching\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.xml copy /V Src\Caching\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Caching\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.dll copy /V Src\Caching\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Caching\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.xml copy /V Src\Caching\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Caching\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.dll copy /V Src\Caching\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Caching\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.xml copy /V Src\Caching\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Caching\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.dll copy /V Src\Caching\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Caching\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.xml copy /V Src\Caching\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Caching\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.dll copy /V Src\Caching\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Caching\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.xml copy /V Src\Caching\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Common files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist Src\Common\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Common.dll copy /V Src\Common\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Common.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Common\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Common.xml copy /V Src\Common\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Common.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Configuration files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist Src\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.dll copy /V Src\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.xml copy /V Src\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Data Access files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist Src\Data\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.dll copy /V Src\Data\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Data\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.xml copy /V Src\Data\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Data\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.dll copy /V Src\Data\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Data\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.xml copy /V Src\Data\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Exception Handling files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist Src\ExceptionHandling\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.dll copy /V Src\ExceptionHandling\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\ExceptionHandling\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.xml copy /V Src\ExceptionHandling\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\ExceptionHandling\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.dll copy /V Src\ExceptionHandling\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\ExceptionHandling\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.xml copy /V Src\ExceptionHandling\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\ExceptionHandling\Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.dll copy /V Src\ExceptionHandling\Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\ExceptionHandling\Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.xml copy /V Src\ExceptionHandling\Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\ExceptionHandling\Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.dll copy /V Src\ExceptionHandling\Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\ExceptionHandling\Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.xml copy /V Src\ExceptionHandling\Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Logging files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist Src\Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.dll copy /V Src\Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.xml copy /V Src\Logging\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.dll copy /V Src\Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.xml copy /V Src\Logging\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Logging\MSMQDistributor\bin\%buildType%\MSMQDistributor.exe copy /V Src\Logging\MSMQDistributor\bin\%buildType%\MSMQDistributor.exe %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Logging\MSMQDistributor\bin\%buildType%\MsmqDistributor.exe.config copy /V Src\Logging\MSMQDistributor\bin\%buildType%\MsmqDistributor.exe.config %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Logging\MSMQDistributor\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.xml copy /V Src\Logging\MSMQDistributor\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Logging\TraceListeners\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.dll copy /V Src\Logging\TraceListeners\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Logging\TraceListeners\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.xml copy /V Src\Logging\TraceListeners\Database\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Logging\TraceListeners\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design.dll copy /V Src\Logging\TraceListeners\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Logging\TraceListeners\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design.xml copy /V Src\Logging\TraceListeners\Database\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO Copy Security files with verification
@ECHO ----------------------------------------
@ECHO.

if Exist Src\Security\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.dll copy /V Src\Security\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Security\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.xml copy /V Src\Security\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Security\AzMan\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.dll copy /V Src\Security\AzMan\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Security\AzMan\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.xml copy /V Src\Security\AzMan\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Security\AzMan\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.dll copy /V Src\Security\AzMan\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Security\AzMan\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.xml copy /V Src\Security\AzMan\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Security\Cache\CachingStore\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.dll copy /V Src\Security\Cache\CachingStore\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Security\Cache\CachingStore\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.xml copy /V Src\Security\Cache\CachingStore\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Security\Cache\CachingStore\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Design.dll copy /V Src\Security\Cache\CachingStore\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Security\Cache\CachingStore\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Design.xml copy /V Src\Security\Cache\CachingStore\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Security\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.dll copy /V Src\Security\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Security\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.xml copy /V Src\Security\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Security\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.dll copy /V Src\Security\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Security\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.xml copy /V Src\Security\Cryptography\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.xml %binDir%\.
@if errorlevel 1 goto :error

if Exist Src\Security\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.dll copy /V Src\Security\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.dll %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Security\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.xml copy /V Src\Security\Cryptography\Configuration\Design\bin\%buildType%\Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Design.xml %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO -----------------------------------------------
@ECHO Copy Configuration Tool files with verification
@ECHO -----------------------------------------------
@ECHO.

if Exist Src\Configuration\Console\bin\%buildType%\EntLibConfig.exe copy /V Src\Configuration\Console\bin\%buildType%\EntLibConfig.exe %binDir%\.
@if errorlevel 1 goto :error
if Exist Src\Configuration\Console\bin\%buildType%\EntLibConfig.exe.config copy /V Src\Configuration\Console\bin\%buildType%\EntLibConfig.exe.config %binDir%\.
@if errorlevel 1 goto :error

@ECHO.
@ECHO ----------------------------------------
@ECHO CopyAssemblies.bat Completed
@ECHO ----------------------------------------
@ECHO.

@REM  ----------------------------------------
@REM  Restore the command prompt and exit
@REM  ----------------------------------------
@goto :exit

@REM  -------------------------------------------
@REM  Handle errors
@REM
@REM  Use the following after any call to exit
@REM  and return an error code when errors occur
@REM
@REM  if errorlevel 1 goto :error	
@REM  -------------------------------------------
:error
  @ECHO An error occured in CopyAssemblies.bat - %errorLevel%

if %pause%==true PAUSE
@exit errorLevel

:HELP
echo Usage: CopyAssemblies [/q] [build output folder] [destination dir]
echo.
echo CopyAssemblies is to be executed in the directory where EnterpriseLibrary.sln resides
echo The default build output folder is Debug
echo The default destintation directory is bin
echo.
echo Examples:
echo.
echo    "CopyAssemblies" - copies Debug build assemblies to bin      
echo    "CopyAssemblies Release" - copies Release build assemblies to bin
echo    "CopyAssemblies Release C:\temp" - copies Release build assemblies to C:\temp
echo.

@REM  ----------------------------------------
@REM  The exit label
@REM  ----------------------------------------
:exit
popd
set pause=
set solutionDir=
set buildType=
set prompt=%savedPrompt%
set savedPrompt=
echo on