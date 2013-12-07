@echo off
set path=%path%;C:/Windows/Microsoft.NET/Framework/v4.0.30319;

::echo Under construction
::goto end

echo Building project...
msbuild ../src/Antler.Core.sln /nologo /v:q /p:Configuration=Release /t:Clean
msbuild ../src/Antler.Core.sln /nologo /v:q /p:Configuration=Release /clp:ErrorsOnly

echo Merging assemblies...
if exist output rmdir /s /q output
mkdir output
mkdir output\lib
mkdir output\lib\net40


echo Done.

:end