::++++++++++++++++++++++ Building +++++++++++++++++++++++++++++++++++++++++++++++++++++
if %skipBuild%==true goto skipBuild
echo Building project...

msbuild.exe ..\src\Antler.sln /nologo /v:q /p:Configuration=Release /t:Clean
msbuild.exe ..\src\Antler.sln /nologo /v:q /p:Configuration=Release /clp:ErrorsOnly /p:TargetFrameworkVersion=%targetFrameworkVersion%
:skipBuild
::++++++++++++++++++++++ Running tests++++++++++++++++++++++++++++++++++++++++++++++++++

if %skipTests%==true goto skipTests
echo Running tests...
if exist output rmdir /s /q output
mkdir output

..\tools\nunit.runner\nunit-console.exe /work:output ..\src\specs\Antler-Domain-Specs\bin\Release\Antler.Domain.Specs.dll ..\src\specs\Antler-EntityFramework-SqlCe-Specs\bin\Release\Antler.EntityFramework.SqlCe.Specs.dll ..\src\specs\Antler-EntityFramework-SqlServer-Specs\bin\Release\Antler.EntityFramework.SqlServer.Specs.dll ..\src\specs\Antler-NHibernate-Sqlite-Specs\bin\Release\Antler.NHibernate.Sqlite.Specs.dll ..\src\specs\Antler-NHibernate-SqlServer-Specs\bin\Release\Antler.NHibernate.SqlServer.Specs.dll ..\src\specs\Antler-Storages-Specs\bin\Release\Antler.Storages.Specs.dll ..\src\specs\Antler-Windsor-Specs\bin\Release\Antler.Windsor.Specs.dll ..\src\specs\Antler-StructureMap-Specs\bin\Release\Antler.StructureMap.Specs.dll
if %ERRORLEVEL% neq 0 goto end
:skipTests

::++++++++++++++++++++++ Copying built assemblies+++++++++++++++++++++++++++++++++++++++
echo Copying assemblies...

mkdir core\output\lib\%targetNuGetFolder%
mkdir nh\output\lib\%targetNuGetFolder%
mkdir ef\output\lib\%targetNuGetFolder%
mkdir linq2db\output\lib\%targetNuGetFolder%
mkdir ef-sqlce\output\lib\%targetNuGetFolder%
mkdir windsor\output\lib\%targetNuGetFolder%
mkdir structuremap\output\lib\%targetNuGetFolder%

::Core
copy ..\src\main\Antler-Core\bin\Release\Antler.Core.* core\output\lib\%targetNuGetFolder%

::NHibernate adapter
copy ..\src\main\Antler-NHibernate\bin\Release\Antler.NHibernate.* nh\output\lib\%targetNuGetFolder%

::EntityFramework adapter
copy ..\src\main\Antler-EntityFramework\bin\Release\Antler.EntityFramework.* ef\output\lib\%targetNuGetFolder%

::Linq2Db adapter
copy ..\src\main\Antler-Linq2Db\bin\Release\Antler.Linq2Db.* linq2db\output\lib\%targetNuGetFolder%

::EntityFramework + SqlCe adapter
copy ..\src\main\Antler-EntityFramework\bin\Release\Antler.EntityFramework.* ef-sqlce\output\lib\%targetNuGetFolder%
copy ..\src\main\Antler-EntityFramework-SqlCe\bin\Release\Antler.EntityFramework.SqlCe.* ef-sqlce\output\lib\%targetNuGetFolder%

::Windsor adapter
copy ..\src\main\Antler-Windsor\bin\Release\Antler.Windsor.* windsor\output\lib\%targetNuGetFolder%

::StructureMap adapter
copy ..\src\main\Antler-StructureMap\bin\Release\Antler.StructureMap.* structuremap\output\lib\%targetNuGetFolder%
::++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++