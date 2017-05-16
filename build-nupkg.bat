@echo off

set nupkg_version=0.9.1

.nuget\nuget pack nclang.nuspec -Prop Version=%nupkg_version% -Prop Configuration=Release
