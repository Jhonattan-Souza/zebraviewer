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

        public static void SetPrinterPort(Printer printerToChange, bool setToOlderName)
        {
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
