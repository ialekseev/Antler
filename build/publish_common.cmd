::++++++++++++++++++++++ Building +++++++++++++++++++++++++++++++++++++++++++++++++++++
if %skipBuild%==true goto skipBuild
echo Building project...

msbuild.exe ..\src\Antler.sln /nologo /v:q /p:Configuration=Release /t:Clean
msbuild.exe ..\src\Antler.sln /nologo /v:q /p:Configuration=Release /clp:ErrorsOnly
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

if exist core\output rmdir /s /q core\output
if exist nh\output rmdir /s /q nh\output
if exist ef\output rmdir /s /q ef\output
if exist linq2db\output rmdir /s /q linq2db\output
if exist ef-sqlce\output rmdir /s /q ef-sqlce\output
if exist windsor\output rmdir /s /q windsor\output
if exist structuremap\output rmdir /s /q structuremap\output

mkdir core\output\lib\net40
mkdir nh\output\lib\net40
mkdir ef\output\lib\net40
mkdir linq2db\output\lib\net40
mkdir ef-sqlce\output\lib\net40
mkdir windsor\output\lib\net40
mkdir structuremap\output\lib\net40

::Core
copy ..\src\main\Antler-Core\bin\Release\Antler.Core.* core\output\lib\net40

::NHibernate adapter
copy ..\src\main\Antler-NHibernate\bin\Release\Antler.NHibernate.* nh\output\lib\net40

::EntityFramework adapter
copy ..\src\main\Antler-EntityFramework\bin\Release\Antler.EntityFramework.* ef\output\lib\net40

::Linq2Db adapter
copy ..\src\main\Antler-Linq2Db\bin\Release\Antler.Linq2Db.* linq2db\output\lib\net40

::EntityFramework + SqlCe adapter
copy ..\src\main\Antler-EntityFramework\bin\Release\Antler.EntityFramework.* ef-sqlce\output\lib\net40
copy ..\src\main\Antler-EntityFramework-SqlCe\bin\Release\Antler.EntityFramework.SqlCe.* ef-sqlce\output\lib\net40

::Windsor adapter
copy ..\src\main\Antler-Windsor\bin\Release\Antler.Windsor.* windsor\output\lib\net40

::StructureMap adapter
copy ..\src\main\Antler-StructureMap\bin\Release\Antler.StructureMap.* structuremap\output\lib\net40

::+++++++++++++++++++++ Updating Nuget Spec files+++++++++++++++++++++++++++++++++++++++++++
if %skipDependentPackagesVersionsUpdate%==true goto skipVersionsUpdate
echo Updating Nuget Spec files(Dependent packages versions update)...
@powershell ./substitude.ps1 %version%
:skipVersionsUpdate
::++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


::++++++++++++++++++++ Creating Nuget packages+++++++++++++++++++++++++++++++++++++++++++++
echo Creating NuGet packages...

copy core\Antler.Core.dll.nuspec core\output
..\src\.nuget\Nuget.exe pack core\output\Antler.Core.dll.nuspec -properties version=%version%
move Antler.Core*.nupkg core\output

copy nh\Antler.NHibernate.dll.nuspec nh\output
..\src\.nuget\Nuget.exe pack nh\output\Antler.NHibernate.dll.nuspec -properties version=%version%
move Antler.NHibernate*.nupkg nh\output

copy ef\Antler.EntityFramework.dll.nuspec ef\output
..\src\.nuget\Nuget.exe pack ef\output\Antler.EntityFramework.dll.nuspec -properties version=%version%
move Antler.EntityFramework*.nupkg ef\output

copy linq2db\Antler.Linq2Db.dll.nuspec linq2db\output
..\src\.nuget\Nuget.exe pack linq2db\output\Antler.Linq2Db.dll.nuspec -properties version=%version%
move Antler.Linq2Db*.nupkg linq2db\output

copy ef-sqlce\Antler.EntityFramework.SqlCe.dll.nuspec ef-sqlce\output
..\src\.nuget\Nuget.exe pack ef-sqlce\output\Antler.EntityFramework.SqlCe.dll.nuspec -properties version=%version%
move Antler.EntityFramework.SqlCe*.nupkg ef-sqlce\output

copy windsor\Antler.Windsor.dll.nuspec windsor\output
..\src\.nuget\Nuget.exe pack windsor\output\Antler.Windsor.dll.nuspec -properties version=%version%
move Antler.Windsor*.nupkg windsor\output

copy structuremap\Antler.StructureMap.dll.nuspec structuremap\output
..\src\.nuget\Nuget.exe pack structuremap\output\Antler.StructureMap.dll.nuspec -properties version=%version%
move Antler.StructureMap*.nupkg structuremap\output

::+++++++++++++++++++++ Publishing NuGet packages+++++++++++++++++++++++++++++++++++++++++++
if %skipPublishing%==true goto skipPublishing
echo Publishing NuGet packages...
..\src\.nuget\Nuget.exe push core\output\Antler.Core.%version%.nupkg
..\src\.nuget\Nuget.exe push nh\output\Antler.NHibernate.%version%.nupkg
..\src\.nuget\Nuget.exe push ef\output\Antler.EntityFramework.%version%.nupkg
..\src\.nuget\Nuget.exe push linq2db\output\Antler.Linq2Db.%version%.nupkg
..\src\.nuget\Nuget.exe push ef-sqlce\output\Antler.EntityFramework.SqlCe.%version%.nupkg
..\src\.nuget\Nuget.exe push windsor\output\Antler.Windsor.%version%.nupkg
..\src\.nuget\Nuget.exe push structuremap\output\Antler.StructureMap.%version%.nupkg
:skipPublishing
::++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
: end
echo Done.