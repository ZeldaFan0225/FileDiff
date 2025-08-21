# DiffApp - File Comparison Tool

A Windows context menu integration tool for file comparison and diff operations. Right-click any file to compare it with others or generate patches.

## Features

- **Set as Diff Left**: Mark a file as the left side for comparison
- **Compare with Left**: Compare any file with the previously selected left file
- **Generate Patch with Left**: Create a unified diff patch file
- **Direct File Comparison**: Compare two files directly via command line
- **Windows Context Menu Integration**: All operations available via right-click menu

## Installation

### Option 1: MSI Installer (Recommended)

1. Run `build-installer.bat` on Windows to create the MSI installer
2. Run `DiffAppInstaller.msi` as administrator
3. The application and context menu items will be installed automatically

### Option 2: Manual Installation

1. Build the application:
   ```bash
   dotnet publish DiffApp.csproj -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true
   ```

2. Copy `DiffApp.exe` from `bin\Release\net8.0\win-x64\publish\` to `C:\Program Files\DiffApp\`

3. Run `install-context-menu.reg` as administrator to add context menu items

### Option 3: Cross-Platform Build

On Linux/macOS, run `build-installer.sh` to build the Windows executable, then manually install on Windows.

## Usage

### Context Menu Operations

1. **Set as Diff Left**: Right-click any file and select "Set as Diff Left"
2. **Compare with Left**: Right-click another file and select "Compare with Left" 
3. **Generate Patch**: Right-click a file and select "Generate Patch with Left"

### Command Line Usage

```bash
DiffApp.exe --set-left <file>           # Set file as left side for comparison
DiffApp.exe --compare <file>            # Compare file with previously set left file
DiffApp.exe --patch <file>              # Generate patch file comparing with left file
DiffApp.exe --diff <file1> <file2>      # Compare two files directly
DiffApp.exe --create-patch <orig> <mod> [output.patch]  # Create patch file
```

### Examples

```bash
# Direct comparison
DiffApp.exe --diff old.txt new.txt

# Create patch file
DiffApp.exe --create-patch original.cs modified.cs changes.patch

# Context menu workflow
# 1. Right-click file1.txt → "Set as Diff Left"
# 2. Right-click file2.txt → "Compare with Left"
```

## Context Menu Items

The installer adds these context menu items to all files:

- **Set as Diff Left** - Mark file for comparison
- **Compare with Left** - Compare with previously marked file  
- **Generate Patch with Left** - Create patch file
- **Compare Files** - Direct comparison option

## Uninstallation

### MSI Installer
Use Windows "Add or Remove Programs" to uninstall.

### Manual Installation
1. Delete `C:\Program Files\DiffApp\` folder
2. Run `uninstall-context-menu.reg` as administrator

## Files Included

- `DiffApp.exe` - Main application executable
- `install-context-menu.reg` - Registry entries for context menu
- `uninstall-context-menu.reg` - Registry removal script
- `build-installer.bat` - Windows build script
- `build-installer.sh` - Cross-platform build script  
- `installer/DiffApp.wxs` - WiX installer source
- `installer/DiffAppInstaller.wixproj` - WiX project file

## Requirements

- Windows 10 or later
- .NET 8.0 runtime (included in self-contained build)
- Administrator privileges for installation

## Development

### Building from Source

```bash
# Restore dependencies
dotnet restore

# Build debug version
dotnet build

# Build release version  
dotnet publish -c Release -r win-x64 --self-contained true
```

### Creating MSI Installer

1. Install WiX Toolset 4.0+
2. Run `build-installer.bat`
3. MSI file will be created in `installer/` directory

## Troubleshooting

### Context Menu Items Not Appearing
- Ensure you ran the installer or registry file as administrator
- Try logging out and back in to refresh the shell
- Check that `DiffApp.exe` exists in `C:\Program Files\DiffApp\`

### "Left file not set" Error
- First right-click a file and select "Set as Diff Left"
- Then right-click another file and select "Compare with Left"

### Application Won't Start
- Check that .NET 8.0 is installed (self-contained build includes runtime)
- Verify file permissions in installation directory
- Run from command prompt to see error messages

## License

This project is open source. See the repository for license details.