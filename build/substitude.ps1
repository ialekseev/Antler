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

#nh-sqlite
substituteVersions "\..\src\main\Antler-NHibernate-Sqlite\packages.config" "\nh-sqlite\Antler.NHibernate.Sqlite.dll.nuspec" "NHibernate"
substituteVersions "\..\src\main\Antler-NHibernate-Sqlite\packages.config" "\nh-sqlite\Antler.NHibernate.Sqlite.dll.nuspec" "Iesi.Collections"
substituteVersions "\..\src\main\Antler-NHibernate-Sqlite\packages.config" "\nh-sqlite\Antler.NHibernate.Sqlite.dll.nuspec" "FluentNHibernate"

#nh-sqlserver
substituteVersions "\..\src\main\Antler-NHibernate-SqlServer\packages.config" "\nh-sqlserver\Antler.NHibernate.SqlServer.dll.nuspec" "NHibernate"
substituteVersions "\..\src\main\Antler-NHibernate-SqlServer\packages.config" "\nh-sqlserver\Antler.NHibernate.SqlServer.dll.nuspec" "Iesi.Collections"
substituteVersions "\..\src\main\Antler-NHibernate-SqlServer\packages.config" "\nh-sqlserver\Antler.NHibernate.SqlServer.dll.nuspec" "FluentNHibernate"

#ef-sqlserver
substituteVersions "\..\src\main\Antler-EntityFramework-SqlServer\packages.config" "\ef-sqlserver\Antler.EntityFramework.SqlServer.dll.nuspec" "EntityFramework"

#ef-sqlce
substituteVersions "\..\src\main\Antler-EntityFramework-SqlCe\packages.config" "\ef-sqlce\Antler.EntityFramework.SqlCe.dll.nuspec" "EntityFramework"
substituteVersions "\..\src\main\Antler-EntityFramework-SqlCe\packages.config" "\ef-sqlce\Antler.EntityFramework.SqlCe.dll.nuspec" "EntityFramework.SqlServerCompact"
substituteVersions "\..\src\main\Antler-EntityFramework-SqlCe\packages.config" "\ef-sqlce\Antler.EntityFramework.SqlCe.dll.nuspec" "Microsoft.SqlServer.Compact"

#windsor
substituteVersions "\..\src\main\Antler-Windsor\packages.config" "\windsor\Antler.Windsor.dll.nuspec" "Castle.Windsor"