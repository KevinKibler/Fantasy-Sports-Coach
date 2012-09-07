Fantasy-Sports-Coach
===================
Provides various statistical analysis to assist in fantasy sports games. 

Only hockey is supported now. Core library is flexible enough to be used with any hockey league and supports custom league configurations.

Supported Platforms:
* Windows

Build Dependencies:
* Microsoft .NET 4.0 SDK
* Microsoft ASP.NET MVC3
* NUnit (optional)

Runtime Dependencies:
* Microsoft .NET 4.0
* Microsoft SQL Server 2008 Express (can be changed by editing website web.config)

Build Instructions:
Use MSBuild 4.0 to build the project:
* Build: at project root, run "msbuild.exe Build.proj"
* Test: at project root, run "msbuild.exe Build.proj /T:Test"
