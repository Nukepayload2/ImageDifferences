call "D:\Program Files (x86)\Microsoft Visual Studio\2017\Professional\VC\Auxiliary\Build\vcvars64.bat"
cd /d %~dp0
set IlcPath=D:\Projects\corert\bin\Windows_NT.x64.Debug
dotnet build ImgDiff.NetCore2_aot.vbproj /t:LinkNative -c release -r win10-x64-aot -o bin/linked-win10-x64-aot
pause