using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;
using ZebraViewer.Models;

namespace ZebraViewer.Services
{
    public class PrinterService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool IsAppDataDoorEnabled()
        {
            string appDataDoorPath = GetAppDataDoorPath();

            foreach (var port in SerialPort.GetPortNames())
            {
                if (port == appDataDoorPath) return true;
            }

            return false;
        }

        private void CreateAppDataPrinterPort()
        {
            using (PowerShell PowerShellInstance = PowerShell.Create())
            {
                PowerShellInstance.AddScript($"Add-PrinterPort -Name {GetAppDataDoorPath()}");
                PowerShellInstance.Invoke();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private string GetAppDataDoorPath()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
                "label.txt");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<Printer> GetPrinters()
        {
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            foreach (ManagementObject printer in searcher.Get())
            {
                yield return new Printer
                {
                    Name = printer.GetPropertyValue("Name").ToString(),
                    PortName = printer.GetPropertyValue("PortName").ToString(),
                    OldPortName = printer.GetPropertyValue("PortName").ToString()
                };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="printerToChange"></param>
        /// <param name="setToOlderName"></param>
        public void SetPrinterPort(Printer printerToChange, bool setToOlderName)
        {
            if (!IsAppDataDoorEnabled()) CreateAppDataPrinterPort();

            ManagementObjectSearcher searcher =
                new ManagementObjectSearcher($"SELECT * FROM Win32_Printer where Name = '{printerToChange.Name}'");

            string localPortName = Path.Combine(
                   Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "label.txt"
             );

            File.Create(localPortName);

            printerToChange.PortName = setToOlderName ? printerToChange.OldPortName : localPortName;

            foreach (ManagementObject printer in searcher.Get())
            {
                printer.SetPropertyValue("PortName", printerToChange.PortName);

                printer.Put();
            }
        }
    }
}
