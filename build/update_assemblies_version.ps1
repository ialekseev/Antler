param(
[string]$version
)

$version = $version.split("-")[0]
$targetVersionBlock = 'AssemblyVersion("' + $version + '")'
$assemblyInfos = Get-ChildItem -Path ..\src\main\ -Filter AssemblyInfo.cs -recurse | Resolve-Path -Relative

$assemblyVersionPattern = 'AssemblyVersion\("([0-9]+(\.([0-9]+|\*)){1,3})"\)'  
foreach ($assemblyInfo in $assemblyInfos) {
   (Get-Content $assemblyInfo) | ForEach-Object {  
                % {$_ -replace $assemblyVersionPattern, $targetVersionBlock} |                  
            } | Set-Content $assemblyInfo         
 }
