using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace File_Monitoring_Windows_Service
{
    [RunInstaller(true)]
    public partial class InstallerProject : Installer
    {
        private ServiceProcessInstaller serviceProcessInstaller;
        private ServiceInstaller serviceInstaller;
        public InstallerProject()
        {
            InitializeComponent();
            // Configure the ServiceInstaller and ServiceProcessInstaller
            serviceProcessInstaller = new ServiceProcessInstaller
            {
                Account = System.ServiceProcess.ServiceAccount.LocalSystem
            };
            serviceInstaller = new ServiceInstaller
            {
                ServiceName = "FileMonitoringService",
                DisplayName = "File Monitoring Windows Service",
                Description = "A Windows Service that monitors file changes in a specified directory.",
                StartType = System.ServiceProcess.ServiceStartMode.Automatic,
                //config system watcher dependency
                ServicesDependedOn = new string[] { "EventLog","RpcSs", "LanmanWorkstation"}

            };

            // Add installers to the Installers collection
            Installers.Add(serviceProcessInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
