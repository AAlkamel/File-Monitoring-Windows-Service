# File Monitoring Windows Service

## Overview

This is a Windows Service written in C# that monitors a specified directory for file system changes (file creation, deletion, modification, and renaming) and logs these events to a configurable log file. The service can run as a background Windows Service or in console mode for testing and debugging purposes.

The service uses the `FileSystemWatcher` class to monitor file system events and provides configurable options through an App.config file.

## Key Features

- **File System Monitoring**: Monitors specified directories for file changes
- **Configurable Notifications**: Enable/disable notifications for different types of events (create, delete, change, rename)
- **Directory Filtering**: Monitor specific file types using filter patterns
- **Subdirectory Support**: Option to include or exclude subdirectories in monitoring
- **Comprehensive Logging**: Logs all service events and file changes with timestamps
- **Console Mode**: Can run interactively for testing without installing as a service
- **Auto-Directory Creation**: Automatically creates monitor and destination directories if they don't exist

## Project Components

### Core Files

- **`Program.cs`**: Main entry point that determines whether to run as a service or in console mode
- **`FileMonitoringWindowsService.cs`**: Main service class that handles file monitoring logic
- **`InstallerProject.cs`**: Installer class for registering the service with Windows
- **`App.config`**: Configuration file containing service settings

### Configuration Settings (App.config)

The service behavior is controlled through the following configuration settings:

- **`log_dir`**: Directory where log files are stored (default: `C:\FileMonitoringServiceLogs`)
- **`log_file`**: Name of the log file (default: `FileMonitoringService.log`)
- **`monitor_dir`**: Directory to monitor for changes (default: `C:\MonitorDirectory`)
- **`destination_dir`**: Destination directory for processed files (default: `C:\DestinationDirectory`)
- **`filter`**: File filter pattern (default: `*.*` for all files)
- **`include_sub_dirs`**: Whether to monitor subdirectories (default: `false`)
- **`notify_renames`**: Log file rename events (default: `true`)
- **`notify_changes`**: Log file modification events (default: `true`)
- **`notify_creations`**: Log file creation events (default: `true`)
- **`notify_deletions`**: Log file deletion events (default: `true`)

## Prerequisites

- **Windows Operating System** (Windows 7 SP1 or later)
- **.NET Framework 4.7.2** or later
- **Administrative privileges** for installing/uninstalling the service

## Building the Project

### Using Visual Studio

1. Open `File Monitoring Windows Service.sln` in Visual Studio
2. Select the desired configuration (Debug/Release)
3. Build the solution: `Build > Build Solution`

### Using Command Line (MSBuild)

```bash
msbuild "File Monitoring Windows Service.csproj" /p:Configuration=Release
```

### Using dotnet CLI

```bash
dotnet build --configuration Release
```

The compiled executable will be located in `bin\Release\` or `bin\Debug\` depending on your build configuration.

## Installation and Running

### Method 1: Install as Windows Service (Recommended for Production)

1. **Build the project** as described above
2. **Open Command Prompt as Administrator**
3. **Navigate to the output directory** (e.g., `bin\Release\`)
4. **Install the service**:
   ```
   installutil.exe "File Monitoring Windows Service.exe"
   ```
5. **Start the service**:
   ```
   sc start "File Monitoring Windows Service"
   ```

### Method 2: Run in Console Mode (For Testing/Debugging)

1. **Build the project** as described above
2. **Navigate to the output directory** (e.g., `bin\Release\`)
3. **Run the executable directly**:
   ```
   "File Monitoring Windows Service.exe"
   ```
4. The service will start monitoring and display logs in the console
5. Press any key to stop the service

## Service Management

### Windows Services Control Panel

- Open **Services** (search for "services.msc")
- Find "File Monitoring Windows Service"
- Right-click to Start, Stop, Pause, or Resume

### Command Line Management

```bash
# Check service status
sc query "File Monitoring Windows Service"

# Start service
sc start "File Monitoring Windows Service"

# Stop service
sc stop "File Monitoring Windows Service"

# Delete/Uninstall service
sc delete "File Monitoring Windows Service"
```

## Monitoring and Logs

- **Log Location**: Configured in `log_dir` setting (default: `C:\FileMonitoringServiceLogs\FileMonitoringService.log`)
- **Log Format**: `YYYY-MM-DD HH:MM:SS - Message`
- **Log Events**:
  - Service start/stop/pause/continue/shutdown events
  - File creation, deletion, modification, and rename events
  - Directory creation events (when auto-creating directories)

## Customization

### Modifying Configuration

1. Open `App.config` in a text editor
2. Modify the `<appSettings>` values as needed
3. Rebuild and reinstall the service for changes to take effect

### Example Configurations

**Monitor only .txt files in a specific directory:**
```xml
<add key="monitor_dir" value="C:\Documents" />
<add key="filter" value="*.txt" />
<add key="include_sub_dirs" value="true" />
```

**Log only file creations and deletions:**
```xml
<add key="notify_changes" value="false" />
<add key="notify_renames" value="false" />
<add key="notify_creations" value="true" />
<add key="notify_deletions" value="true" />
```

## Troubleshooting

### Common Issues

1. **Service fails to start**
   - Ensure the monitor directory exists and is accessible
   - Check log files for error messages
   - Verify .NET Framework is installed

2. **No events are logged**
   - Confirm the service is running
   - Check configuration settings
   - Ensure the monitor directory path is correct

3. **Permission denied errors**
   - Run installation commands as Administrator
   - Ensure the service account has permissions to monitor directories

### Log Analysis

Check the log file for detailed error messages and service events. Common log entries include:
- `Service Started`
- `File created: [path]`
- `File changed: [path]`
- `Service Stopped`

## Development Notes

- **Framework**: .NET Framework 4.7.2
- **Service Base Class**: Inherits from `System.ServiceProcess.ServiceBase`
- **File Monitoring**: Uses `System.IO.FileSystemWatcher`
- **Configuration**: Uses `System.Configuration.ConfigurationManager`

## License

This project is provided as-is for educational and development purposes.
