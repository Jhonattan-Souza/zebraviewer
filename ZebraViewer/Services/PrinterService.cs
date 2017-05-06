using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using ZebraViewer.Models;

namespace ZebraViewer.Services
{
    public class PrinterService
    {
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

        public static void SetPrinterPort(Printer printerToChange, bool toOlderName)
        {
            ManagementObjectSearcher searcher = 
                new ManagementObjectSearcher($"SELECT * FROM Win32_Printer where Name = '{printerToChange.Name}'");

            File.Create(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "label.txt"));

            printerToChange.PortName = toOlderName ? printerToChange.OldPortName : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "label.txt");

            foreach (ManagementObject printer in searcher.Get())
            {
                printer.SetPropertyValue("PortName", toOlderName ? printerToChange.OldPortName : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "label.txt"));
            }
        }
    }
}
