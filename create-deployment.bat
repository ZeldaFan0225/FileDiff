@echo off
echo Creating DiffApp Deployment Package...
echo =====================================
echo.

REM Build the application
echo Building application...
call build-installer.bat
if %ERRORLEVEL% neq 0 (
    echo Build failed!
    pause
    exit /b %ERRORLEVEL%
)

REM Create deployment folder
echo Creating deployment package...
if exist "deployment" rmdir /s /q "deployment"
mkdir "deployment"

REM Copy built application
copy "bin\Release\net8.0\win-x64\publish\DiffApp.exe" "deployment\"
if %ERRORLEVEL% neq 0 (
    echo Failed to copy application executable!
    pause
    exit /b 1
)

REM Copy installation files
copy "install-context-menu.reg" "deployment\"
copy "uninstall-context-menu.reg" "deployment\"
copy "install.bat" "deployment\"
copy "uninstall.bat" "deployment\"
copy "README.md" "deployment\"

REM Create simple installation instructions
echo Creating installation instructions...
(
echo DiffApp - File Comparison Tool
echo ==============================
echo.
echo To install:
echo 1. Run install.bat as Administrator
echo 2. Right-click any file to see new context menu options
echo.
echo To uninstall:
echo 1. Run uninstall.bat as Administrator
echo.
echo For more information, see README.md
) > "deployment\INSTALL.txt"

echo.
echo Deployment package created in 'deployment' folder!
echo.
echo Contents:
dir "deployment" /b
echo.
echo To distribute: Zip the deployment folder and share it.
echo Users should extract and run install.bat as Administrator.
echo.
pause