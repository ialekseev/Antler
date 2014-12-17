@echo off
set path=%path%;C:/Windows/Microsoft.NET/Framework/v4.0.30319;
set version=3.1.0-alpha
set skipBuild=false
set skipTests=false
set skipCopyingBuiltAssemblies=false
set skipDependentPackagesVersionsUpdate=false
set skipCreatingNuGetPackages=false
set skipPublishing=true

call publish_common.cmd

pause