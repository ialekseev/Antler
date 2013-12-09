@cd /d "%~dp0"

copy core\Antler.Core.dll.nuspec core\output
..\src\.nuget\Nuget.exe pack core\output\Antler.Core.dll.nuspec
move Antler.Core*.nupkg core\output

copy nh-sqlite\Antler.NHibernate.Sqlite.dll.nuspec nh-sqlite\output
..\src\.nuget\Nuget.exe pack nh-sqlite\output\Antler.NHibernate.Sqlite.dll.nuspec
move Antler.NHibernate.Sqlite*.nupkg nh-sqlite\output

copy ef-sqlce\Antler.EntityFramework.SqlCe.dll.nuspec ef-sqlce\output
..\src\.nuget\Nuget.exe pack ef-sqlce\output\Antler.EntityFramework.SqlCe.dll.nuspec
move Antler.EntityFramework.SqlCe*.nupkg ef-sqlce\output

copy windsor\Antler.Windsor.dll.nuspec windsor\output
..\src\.nuget\Nuget.exe pack windsor\output\Antler.Windsor.dll.nuspec
move Antler.Windsor*.nupkg windsor\output

pause