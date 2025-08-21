@echo off
echo Building DiffApp Windows Installer...
echo.

REM Build the C# application first
echo Building C# application...
dotnet publish DiffApp.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
if %ERRORLEVEL% neq 0 (
    echo Failed to build C# application
    exit /b %ERRORLEVEL%
)

echo.
echo C# application built successfully.
echo.

echo Output files:
echo - Application: .\bin\Release\net8.0\win-x64\publish\DiffApp.exe
echo - Registry files: .\install-context-menu.reg and .\uninstall-context-menu.reg
echo - Installation scripts: .\install.bat and .\uninstall.bat
echo.

echo Manual Installation Instructions (without MSI):
echo ==============================================
echo 1. Copy DiffApp.exe to C:\Program Files\DiffApp\
echo 2. Run install-context-menu.reg as administrator to add context menu items
echo 3. Or use install.bat for automated installation
echo.
echo The application will be ready to use from the context menu!
echo.

REM Optional: Try to build MSI if WiX is available
where candle >nul 2>nul
if %ERRORLEVEL% equ 0 (
    echo WiX Toolset found. Building MSI installer...
    cd installer
    wix build DiffApp.wxs -arch x64 -o DiffAppInstaller.msi 2>nul
    if %ERRORLEVEL% equ 0 (
        echo MSI Installer created: .\installer\DiffAppInstaller.msi
        cd ..
    ) else (
        echo MSI creation failed, but manual installation files are available.
        cd ..
    )
) else (
    echo MSI creation skipped (WiX not available).
    echo Use manual installation method above.
)

echo.
echo Build completed successfully!
pause