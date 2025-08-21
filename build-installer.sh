#!/bin/bash

echo "Building DiffApp Windows Installer on Linux/macOS..."
echo

# Build the C# application first
echo "Building C# application..."
dotnet publish DiffApp.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true

if [ $? -ne 0 ]; then
    echo "Failed to build C# application"
    exit 1
fi

echo
echo "C# application built successfully."
echo

# Check if WiX is available (only works on Windows, so this is for cross-platform awareness)
echo "Note: MSI creation requires WiX Toolset which runs on Windows."
echo "Built application location: ./bin/Release/net8.0/win-x64/publish/DiffApp.exe"
echo
echo "Manual Installation Instructions for Windows:"
echo "============================================="
echo "1. Copy DiffApp.exe to C:\\Program Files\\DiffApp\\"
echo "2. Run install-context-menu.reg as administrator"
echo "3. The application will be ready to use from the context menu"
echo
echo "Registry files created:"
echo "- install-context-menu.reg (adds context menu items)"
echo "- uninstall-context-menu.reg (removes context menu items)"
echo