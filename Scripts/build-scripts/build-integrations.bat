@ECHO OFF

::_________________________________________::
::_____SETTING SOME VARIABLES FOR HELP_____::
::_________________________________________::

SET INTEGRATIONPARENTFOLDER=..\..\Integration
SET ARGS=/t:Rebuild /p:Configuration=Release /p:Platform="Any CPU"
SET MSBUILDEXE=%windir%\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe

::_________________________________________::
::________BUILDING ALACHISOFT.COMMON_______::
::_________________________________________::

ECHO BUILDING INTERGRATION ALACHISOFT.COMMON FOR
ECHO ================================================
	@"%MSBUILDEXE%" "%INTEGRATIONPARENTFOLDER%\Alachisoft.Common\Alachisoft.Common.sln" %ARGS%
	IF NOT %ERRORLEVEL%==0 GOTO :failAlachisoftDotCommon
	IF %ERRORLEVEL%==0 ECHO Alachisoft.Common build successful

::__________________________________________::
::_BUILDING ALACHISOFT.CONTENT OPTIMIZATION_::
::__________________________________________::

ECHO BUILDING INTERGRATION ALACHISOFT.CONTENT OPTIMIZATION FOR
ECHO ================================================
	@"%MSBUILDEXE%" "%INTEGRATIONPARENTFOLDER%\ViewStateCaching\Alachisoft.ContentOptimization\Alachisoft.ContentOptimization.sln" %ARGS%
	IF NOT %ERRORLEVEL%==0 GOTO :failAlachisoftDotContentOptimization
	IF %ERRORLEVEL%==0 ECHO Alachisoft.ContentOptimization build successful

::_________________________________________::
::_______BUILDING VIEW STATE CACHING_______::
::_________________________________________::

ECHO BUILDING INTERGRATION VIEW STATE CACHING FOR
ECHO ================================================
	@"%MSBUILDEXE%" "%INTEGRATIONPARENTFOLDER%\ViewStateCaching\ContentOptimization.sln" %ARGS%
	IF NOT %ERRORLEVEL%==0 GOTO :failViewStateCaching
	IF %ERRORLEVEL%==0 ECHO ViewStateCaching build successful
	
::___________________________________::
::______BUILDING LINQ TO NCACHE______::
::___________________________________::

ECHO BUILDING INTERGRATION LINQ TO NCACHE FOR 
ECHO ================================================
	@"%MSBUILDEXE%" "%INTEGRATIONPARENTFOLDER%\LinqToNCache\LinqToNCache.sln" %ARGS%
	IF NOT %ERRORLEVEL%==0 GOTO :failLinqToNCache
	IF %ERRORLEVEL%==0 ECHO LinqToNCache build successful

::_________________________________________::
::_____BUILDING NHIBERNATE OPEN SOURCE_____::
::_________________________________________::

ECHO BUILDING INTERGRATION NHIBERNATE OPEN SOURCE FOR 
ECHO ================================================
	@"%MSBUILDEXE%" "%INTEGRATIONPARENTFOLDER%\NHibernate\src\NHibernateNCache.OpenSource.sln" %ARGS%
	IF NOT %ERRORLEVEL%==0 GOTO :failNHibernate
	IF %ERRORLEVEL%==0 ECHO NHibernateNCache.OpenSource build successful

::_________________________________________::
::___________BUILDING OUTPUTCACHE__________::
::_________________________________________::

ECHO BUILDING INTERGRATION OUTPUT CACHE FOR 
ECHO ================================================
	@"%MSBUILDEXE%" "%INTEGRATIONPARENTFOLDER%\OutputCache\OutputCache.sln" %ARGS%
	IF NOT %ERRORLEVEL%==0 GOTO :failOutputCache
	IF %ERRORLEVEL%==0 ECHO OutputCache build successful

::_________________________________________::
::_________BUILDING SIGNALR.NCACHE_________::
::_________________________________________::

ECHO BUILDING INTERGRATION SIGNALR NCACHE FOR 
ECHO ================================================
	@"%MSBUILDEXE%" "%INTEGRATIONPARENTFOLDER%\SignalR.NCache\SIgnalR.NCache.sln" %ARGS%
	IF NOT %ERRORLEVEL%==0 GOTO :failSignalRNCache
	IF %ERRORLEVEL%==0 ECHO SignalR.NCache build successful

EXIT /b 0

:failAlachisoftDotCommon
ECHO FAILED TO BUILD ALACHISOFT.COMMON
ECHO =======================================
"%INTEGRATIONPARENTFOLDER%\Alachisoft.Common\Alachisoft.Common.sln"
PAUSE
EXIT /b 1

:failAlachisoftDotContentOptimization
ECHO FAILED TO BUILD ALACHISOFT.CONTENT OPTIMIZATION 
ECHO =======================================
"%INTEGRATIONPARENTFOLDER%\ViewStateCaching\Alachisoft.ContentOptimization\Alachisoft.ContentOptimization.sln"
PAUSE
EXIT /b 1

:failViewStateCaching
ECHO FAILED TO BUILD VIEW STATE CACHING 
ECHO =======================================
"%INTEGRATIONPARENTFOLDER%\ViewStateCaching\ContentOptimization.sln"
PAUSE
EXIT /b 1

:failLinqToNCache
ECHO FAILED TO BUILD LINQ TO NCACHE 
ECHO =======================================
"%INTEGRATIONPARENTFOLDER%\LinqToNCache\LinqToNCache.sln"
PAUSE
EXIT /b 1

:failNHibernate
ECHO FAILED TO BUILD NHIBERNATE OPEN SOURCE
ECHO =============================
"%INTEGRATIONPARENTFOLDER%\NHibernate\src\NHibernateNCache.OpenSource.sln"
PAUSE
EXIT /b 1

:failOutputCache
ECHO FAILED TO BUILD OUTPUTCACHE 
ECHO =============================
"%INTEGRATIONPARENTFOLDER%\OutputCache\OutputCache.sln"
PAUSE
EXIT /b 1

:failSignalRNCache
ECHO FAILED TO BUILD SIGNALR NCACHE 
ECHO =============================
"%INTEGRATIONPARENTFOLDER%\SignalR.NCache\SIgnalR.NCache.sln"
PAUSE
EXIT /b 1
