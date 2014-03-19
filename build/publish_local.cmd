@echo off
set path=%path%;C:/Windows/Microsoft.NET/Framework/v4.0.30319;
set version=2.7
set skipBuild=false
set skipTests=false
set skipDependentPackagesVersionsUpdate=false
set skipPublishing=false

call publish_common.cmd

pause