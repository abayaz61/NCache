@ECHO off
 @Set directory=%1
 
 @SET iuPathIS=%WINDIR%\Microsoft.NET\Framework\v4.0.30319\

 REG QUERY "HKLM\SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" /v InstallPath >nul 2>&1
 SET QUERY_STATUS=%ERRORLEVEL%
 IF %QUERY_STATUS%==0 ( 
	ECHO VERIFYING IF SERVICE ALREADY EXISTS
	ECHO =======================================
	SC QUERY NCACHESVC > NUL
	IF ERRORLEVEL 1060 ( 
		ECHO SERVICE DOES NOT EXIST
		ECHO ==========================
	)
	IF NOT ERRORLEVEL 1060 (
		ECHO SERVICE EXISTS, REMOVING PREVIOUS SERVICE
		ECHO =============================================
		NET STOP NCACHESVC
		SC DELETE NCACHESVC
		SC QUERY NCACHESVC > NUL
		IF NOT ERRORLEVEL 1060 (
		ECHO SERVICE REMOVED SUCCESSFULLY
		ECHO ================================= 
		)
	) 
 	SETLOCAL ENABLEEXTENSIONS 
	SETLOCAL enabledelayedexpansion
	
	ECHO INSTALLING SERVICE 4.0
	ECHO ========================
	%iuPathIS%INSTALLUTIL.EXE /i %1\bin\service\Alachisoft.NCache.Service.exe
	ECHO. 
 )
 
 ECHO STARTING SERVICE
 ECHO ====================
 NET START NCACHESVC
 SC QUERY NCACHESVC
 IF ERRORLEVEL 1060 (
 	ECHO SERVICE NOT STARTED
 )
 ECHO.