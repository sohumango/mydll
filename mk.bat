@echo off

echo %path% | findstr 2017 >NUL && (
        echo find14
) || (
    echo notfind14
    call "C:\Program Files (x86)\Microsoft Visual Studio\2017\Community\VC\Auxiliary\Build\vcvars64.bat"
)

if "%1" == "" (
    echo]       -----make dll---------
    cl.exe /D_USRDLL /D_WINDLL mydll.cpp /link Ole32.lib /dll /out:mydll.dll
    echo]
    echo]
    echo]       -----make testdll.exe---------
    csc -langversion:latest testdll.cs /unsafe 
    if exist "testdll.exe" (
        echo]     -----run testdll.exe---------
        testdll.exe
    )
) else (
    rm mydll.exp
    rm mydll.dll
    rm mydll.lib
    rm mydll.obj
    rm testdll.exe
)

