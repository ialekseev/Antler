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

@powershell ./run_tests.ps1
if %ERRORLEVEL% neq 0 Exit %ERRORLEVEL%
:skipTests

::++++++++++++++++++++++ Copying built assemblies+++++++++++++++++++++++++++++++++++++++
if %skipCopyingBuiltAssemblies%==true goto skipCopyingBuiltAssemblies

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
:skipCopyingBuiltAssemblies
::++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++