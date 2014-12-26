
if exist core\output rmdir /s /q core\output
if exist nh\output rmdir /s /q nh\output
if exist ef\output rmdir /s /q ef\output
if exist linq2db\output rmdir /s /q linq2db\output
if exist ef-sqlce\output rmdir /s /q ef-sqlce\output
if exist windsor\output rmdir /s /q windsor\output
if exist structuremap\output rmdir /s /q structuremap\output

@powershell ./update_assemblies_version.ps1 %version%

set targetFrameworkVersion=v4.0
set targetNuGetFolder=net40
call build.cmd

set targetFrameworkVersion=v4.5
set targetNuGetFolder=net45
call build.cmd

::+++++++++++++++++++++ Updating Nuget Spec files+++++++++++++++++++++++++++++++++++++++++++
if %skipDependentPackagesVersionsUpdate%==true goto skipVersionsUpdate
echo Updating Nuget Spec files(Dependent packages versions update)...
@powershell ./substitute.ps1 %version%
:skipVersionsUpdate

::++++++++++++++++++++ Creating Nuget packages+++++++++++++++++++++++++++++++++++++++++++++
if %skipCreatingNuGetPackages%==true goto skipCreatingNuGetPackages
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
:skipCreatingNuGetPackages

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
echo Done.