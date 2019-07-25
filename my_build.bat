@rem curl https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -o nuget.exe
nuget restore
"c:\Program Files (x86)\Microsoft Visual Studio\2017\Community\MSBuild\15.0\Bin\MSBuild" COE.Examples\COE.Examples.csproj
