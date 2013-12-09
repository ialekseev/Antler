@echo off
set path=%path%;C:/Windows/Microsoft.NET/Framework/v4.0.30319;

::echo Under construction
::goto end

echo Building project...
msbuild ../src/Antler.sln /nologo /v:q /p:Configuration=Release /t:Clean
msbuild ../src/Antler.sln /nologo /v:q /p:Configuration=Release /clp:ErrorsOnly

echo Copying assemblies...
if exist core\output rmdir /s /q output
if exist nh-sqlite\output rmdir /s /q output
if exist ef-sqlce\output rmdir /s /q output
if exist windsor\output rmdir /s /q output

mkdir core\output\lib\net40
mkdir nh-sqlite\output\lib\net40
mkdir ef-sqlce\output\lib\net40
mkdir windsor\output\lib\net40

::Core
copy ..\src\main\Antler-Core\bin\Release\Antler.Core.* core\output\lib\net40

::NHibernate + Sqlite
copy ..\src\main\Antler-NHibernate\bin\Release\Antler.NHibernate.* nh-sqlite\output\lib\net40
copy ..\src\main\Antler-NHibernate-Sqlite\bin\Release\Antler.NHibernate.Sqlite.* nh-sqlite\output\lib\net40

::EntityFramework + SqlCe
copy ..\src\main\Antler-EntityFramework\bin\Release\Antler.EntityFramework.* ef-sqlce\output\lib\net40
copy ..\src\main\Antler-EntityFramework-SqlCe\bin\Release\Antler.EntityFramework.SqlCe.* ef-sqlce\output\lib\net40

::Windsor
copy ..\src\main\Antler-Windsor\bin\Release\Antler.Windsor.* windsor\output\lib\net40

echo Done.

:end

pause