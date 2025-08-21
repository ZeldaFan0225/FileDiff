@echo off
echo DiffApp - File Comparison Tool - Uninstaller
echo =============================================
echo.

echo This will remove DiffApp from your system and remove context menu items.
echo You must run this script as Administrator.
echo.
pause

REM Remove context menu entries
echo Removing context menu entries...
regedit /s uninstall-context-menu.reg
if %ERRORLEVEL% neq 0 (
    echo Warning: Failed to remove some registry entries.
)

REM Remove application files
echo Removing application files...
if exist "C:\Program Files\DiffApp\DiffApp.exe" (
    del /Q "C:\Program Files\DiffApp\DiffApp.exe"
)

REM Remove installation directory if empty
if exist "C:\Program Files\DiffApp" (
    rmdir "C:\Program Files\DiffApp" 2>nul
    if exist "C:\Program Files\DiffApp" (
        echo Note: Installation directory not removed (may contain user files)
    )
)

echo.
echo Uninstallation completed!
echo.
echo Context menu items have been removed.
echo Please log out and back in for changes to take full effect.
echo.
pause