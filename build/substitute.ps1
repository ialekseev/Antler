$publishVersion=$args[0]
$configDirectory = Split-Path $script:MyInvocation.MyCommand.Path

function substituteVersions($fromFilePath, $toFilePath, $packageId){
$fromFilePathFull = ($configDirectory + $fromFilePath)
[xml]$fromFile = Get-Content $fromFilePathFull
$version = $fromFile.SelectSingleNode("//packages/package[@id='" + $packageId+ "']").version

$toFilePathFull = ($configDirectory + $toFilePath)
[xml]$toFile = Get-Content $toFilePathFull
$toFile.SelectSingleNode("//package/metadata/dependencies/dependency[@id='" + $packageId + "']").version = $version
$toFile.Save($toFilePathFull)
}

function setCorePackageVersion($toFilePath){
  $toFilePathFull = ($configDirectory + $toFilePath)
  [xml]$toFile = Get-Content $toFilePathFull
  $toFile.SelectSingleNode("//package/metadata/dependencies/dependency[@id='Antler.Core']").version = "[" + $publishVersion +"]"
  $toFile.Save($toFilePathFull)
}

#nh
setCorePackageVersion "\nh\Antler.NHibernate.dll.nuspec"
substituteVersions "\..\src\main\Antler-NHibernate\packages.config" "\nh\Antler.NHibernate.dll.nuspec" "NHibernate"
substituteVersions "\..\src\main\Antler-NHibernate\packages.config" "\nh\Antler.NHibernate.dll.nuspec" "Iesi.Collections"
substituteVersions "\..\src\main\Antler-NHibernate\packages.config" "\nh\Antler.NHibernate.dll.nuspec" "FluentNHibernate"

#ef
setCorePackageVersion "\ef\Antler.EntityFramework.dll.nuspec"
substituteVersions "\..\src\main\Antler-EntityFramework\packages.config" "\ef\Antler.EntityFramework.dll.nuspec" "EntityFramework"

#linq2db
setCorePackageVersion "\linq2db\Antler.Linq2Db.dll.nuspec"
substituteVersions "\..\src\main\Antler-Linq2Db\packages.config" "\linq2db\Antler.Linq2Db.dll.nuspec" "linq2db"

#ef-sqlce
setCorePackageVersion "\ef-sqlce\Antler.EntityFramework.SqlCe.dll.nuspec"
substituteVersions "\..\src\main\Antler-EntityFramework-SqlCe\packages.config" "\ef-sqlce\Antler.EntityFramework.SqlCe.dll.nuspec" "EntityFramework"
substituteVersions "\..\src\main\Antler-EntityFramework-SqlCe\packages.config" "\ef-sqlce\Antler.EntityFramework.SqlCe.dll.nuspec" "EntityFramework.SqlServerCompact"
substituteVersions "\..\src\main\Antler-EntityFramework-SqlCe\packages.config" "\ef-sqlce\Antler.EntityFramework.SqlCe.dll.nuspec" "Microsoft.SqlServer.Compact"

#mongodb
setCorePackageVersion "\mongodb\Antler.MongoDb.dll.nuspec"
substituteVersions "\..\src\main\Antler-MongoDb\packages.config" "\mongodb\Antler.MongoDb.dll.nuspec" "mongocsharpdriver"

#windsor
setCorePackageVersion "\windsor\Antler.Windsor.dll.nuspec"
substituteVersions "\..\src\main\Antler-Windsor\packages.config" "\windsor\Antler.Windsor.dll.nuspec" "Castle.Windsor"

#structuremap
setCorePackageVersion "\structuremap\Antler.StructureMap.dll.nuspec"
substituteVersions "\..\src\main\Antler-StructureMap\packages.config" "\structuremap\Antler.StructureMap.dll.nuspec" "structuremap"