$dir = Split-Path $script:MyInvocation.MyCommand.Path
$nunit = "..\tools\nunit.runner\nunit-console.exe" 

$assemblies = Get-ChildItem ..\src\specs\Antler-*-Specs\bin\Release\Antler.*.Specs.dll -recurse | Resolve-Path -Relative | select $_.FullName
foreach ($assembly in $assemblies) {
   Write-Host "Running tests from assembly: " $assembly
   & $nunit /work:output $assembly
   If($LASTEXITCODE -ne 0)
   {
     Write-Host "Tests failed with error code: " $LASTEXITCODE
     exit $LASTEXITCODE
   }   
 }