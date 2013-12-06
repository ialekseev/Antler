@cd /d "%~dp0"

copy Antler.Core.dll.nuspec output\Antler.Core.dll.nuspec
..\src\.nuget\Nuget.exe pack output\Antler.Core.dll.nuspec

move *.nupkg output
