@echo off
set path=%path%;C:/Windows/Microsoft.NET/Framework/v4.0.30319;

echo Under construction
goto end

echo Building project...
msbuild src/Antler.sln /nologo /v:q /p:Configuration=Release /t:Clean
msbuild src/Antler.sln /nologo /v:q /p:Configuration=Release /clp:ErrorsOnly

echo Merging assemblies...
if exist output rmdir /s /q output
mkdir output
mkdir output\bin

:tools\ilmerge\ILMerge.exe /keyfile:src\Antler.snk /wildcards /target:library
tools\ilmerge\ILMerge.exe /target:library ^
 /targetplatform:"v4,C:\WINDOWS\Microsoft.NET\Framework\v4.0.30319" ^
 /out:"output\bin\Antler.Core.dll" ^
 "src\main\Antler-Abstractions\bin\Release\SmartElk.Antler.Abstractions.dll" ^
 "src\main\Antler-Common\bin\Release\SmartElk.Antler.Common.dll" ^
 "src\main\Antler-Domain\bin\Release\SmartElk.Antler.Domain.dll" 

copy src\main\Antler-Windsor\bin\Release\SmartElk.Antler.Windsor.* output\bin\Antler.Windsor.dll

echo Done.

:end