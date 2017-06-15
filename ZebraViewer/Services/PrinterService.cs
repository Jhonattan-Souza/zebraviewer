using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Management.Automation;
using ZebraViewer.Helpers;
using ZebraViewer.Models;

namespace ZebraViewer.Services
{
    public class PrinterService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static bool IsAppDataDoorEnabled()
        {
            var appDataDoorPath = GetAppDataDoorPath();

            return SerialPort.GetPortNames().Any(port => port == appDataDoorPath);
        }

        private static void CreateAppDataPrinterPort()
        {
            using (var powerShellInstance = PowerShell.Create())
            {
                powerShellInstance.AddScript($"Add-PrinterPort -Name {GetAppDataDoorPath()}");
                powerShellInstance.Invoke();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static string GetAppDataDoorPath()
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
            var searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Printer");

            foreach (var o in searcher.Get())
            {
                var printer = (ManagementObject) o;
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
        public static void SetPrinterPort(Printer printerToChange, bool setToOlderName)
        {
            if (!IsAppDataDoorEnabled() && HasPowerShellPrinterCommand()) CreateAppDataPrinterPort();

            var searcher =
                new ManagementObjectSearcher($"SELECT * FROM Win32_Printer where Name = '{printerToChange.Name}'");

            var localPortName = Path.Combine(
                   Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "label.txt"
             );

            File.Create(localPortName).Close();

            printerToChange.PortName = setToOlderName ? printerToChange.OldPortName : localPortName;

            foreach (var o in searcher.Get())
            {
                var printer = (ManagementObject) o;
                printer.SetPropertyValue("PortName", printerToChange.PortName);

                printer.Put();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private static bool HasPowerShellPrinterCommand() => !SystemUtilities.GetOperationalSystemName().Contains("Windows 7");

        public static string GetPrinterFileCode()
        {
            return File.ReadAllText(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "label.txt"));
        } 
    }
}
