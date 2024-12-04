@echo off
set PKG=Unicorn.Reporting.ReportPortal
set VERSION=%1
set OUT_DIR=%2

dotnet pack src\%PKG%.csproj -o %OUT_DIR% -c Release -p:NuspecFile=%PKG%.nuspec -p:Version=%VERSION%