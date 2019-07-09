@echo off
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

