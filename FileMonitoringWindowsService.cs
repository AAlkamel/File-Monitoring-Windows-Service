using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace File_Monitoring_Windows_Service
{


    public partial class FileMonitoringWindowsService : ServiceBase
    {
        string logFileName, logPath, logDir,
            monitorDir, destinationDir, filter;
        bool includeSubDirs, notifyRenames, notifyChanges,
            notifyCreations, notifyDeletions;
        FileSystemWatcher watcher;

        public FileMonitoringWindowsService()
        {
            InitializeComponent();
            CanPauseAndContinue = true;
            CanShutdown = true;
            //config variables
            // log
            logFileName = ConfigurationManager.AppSettings["log_file"] ?? "FileMonitoringService.log";
            logDir = ConfigurationManager.AppSettings["log_dir"] ?? "C:\\FileMonitoringServiceLogs";
            logPath = Path.Combine(logDir, logFileName);
            // monitor
            monitorDir = ConfigurationManager.AppSettings["monitor_dir"] ?? "C:\\MonitorDirectory";
            destinationDir = ConfigurationManager.AppSettings["destination_dir"] ?? "C:\\DestinationDirectory";
            filter = ConfigurationManager.AppSettings["filter"] ?? "*.*";
            includeSubDirs = bool.Parse(ConfigurationManager.AppSettings["include_sub_dirs"] ?? "false");
            notifyRenames = bool.Parse(ConfigurationManager.AppSettings["notify_renames"] ?? "true");
            notifyChanges = bool.Parse(ConfigurationManager.AppSettings["notify_changes"] ?? "true");
            notifyCreations = bool.Parse(ConfigurationManager.AppSettings["notify_creations"] ?? "true");
            notifyDeletions = bool.Parse(ConfigurationManager.AppSettings["notify_deletions"] ?? "true");

            //ensure monitor directory exists
            if (!Directory.Exists(monitorDir))
            {
                Directory.CreateDirectory(monitorDir);
            }
            //ensure destination directory exists
            if (!Directory.Exists(destinationDir))
            {
                Directory.CreateDirectory(destinationDir);
            }

        }

        protected override void OnStart(string[] args)
        {
            LogToFile("Service Started");
            LogToFile("Monitoring directory: " + monitorDir);
            LogToFile("Destination directory: " + destinationDir);
            LogToFile("Filter: " + filter);
            LogToFile("Include subdirectories: " + includeSubDirs);
            LogToFile("Notify renames: " + notifyRenames);
            LogToFile("Notify changes: " + notifyChanges);
            LogToFile("Notify creations: " + notifyCreations);
            LogToFile("Notify deletions: " + notifyDeletions);
            //using FileSystemWatcher
            watcher = new FileSystemWatcher
            {
                Path = monitorDir,
                Filter = filter,
                IncludeSubdirectories = includeSubDirs,
                EnableRaisingEvents = true
            };
            watcher.Renamed += (s, e) => {if (notifyRenames)    LogToFile($"File renamed: {e.OldFullPath} to {e.FullPath}");};
            watcher.Changed += (s, e) => {if (notifyChanges)    LogToFile($"File changed: {e.FullPath}");};
            watcher.Created += (s, e) => {if (notifyCreations)   LogToFile($"File created: {e.FullPath}");};
            watcher.Deleted += (s, e) => {if (notifyDeletions)    LogToFile($"File deleted: {e.FullPath}");};
        }

        protected override void OnContinue()
        {
            LogToFile("Service Continue");
        }
        protected override void OnPause()
        {
            LogToFile("Service Pause");
        }
        protected override void OnShutdown()
        {
            LogToFile("Service Shutdown");
        }

        protected override void OnStop()
        {
            LogToFile("Service Stopped");
            //clean watcher resources 
            watcher.EnableRaisingEvents = false;
            watcher.Dispose();

        }

        // log to file method
        private void LogToFile(string message)
        {
            if (!Directory.Exists(logDir))
            {
                Directory.CreateDirectory(logDir);
            }
            using (StreamWriter writer = new StreamWriter(logPath, true))
            {
                writer.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + message);
            }
            if (Environment.UserInteractive)
            {
                Console.WriteLine(message);
            }
        }

        public void StartInConsole()
        {

            OnStart(null);
            Console.WriteLine("press any key to stop");
            Console.ReadLine();
            OnStop();
            Console.ReadKey();
        }
    }
}
