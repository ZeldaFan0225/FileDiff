@echo off
echo DiffApp - File Comparison Tool - Manual Installation
echo ===================================================
echo.

echo This will install DiffApp to C:\Program Files\DiffApp and add context menu items.
echo You must run this script as Administrator.
echo.
pause

REM Create installation directory
echo Creating installation directory...
if not exist "C:\Program Files\DiffApp" (
    mkdir "C:\Program Files\DiffApp"
    if %ERRORLEVEL% neq 0 (
        echo Failed to create installation directory. Please run as Administrator.
        pause
        exit /b 1
    )
)

REM Copy application files
echo Copying application files...
copy /Y "DiffApp.exe" "C:\Program Files\DiffApp\DiffApp.exe"
if %ERRORLEVEL% neq 0 (
    echo Failed to copy application files.
    pause
    exit /b 1
)

REM Install context menu entries
echo Installing context menu entries...
regedit /s install-context-menu.reg
if %ERRORLEVEL% neq 0 (
    echo Failed to install registry entries.
    pause
    exit /b 1
)

echo.
echo Installation completed successfully!
echo.
echo Context menu items have been added:
echo - Set as Diff Left
echo - Compare with Left  
echo - Generate Patch with Left
echo.
echo Right-click any file to use these features.
echo.
pause