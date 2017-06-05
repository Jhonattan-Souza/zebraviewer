using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace ZebraViewer.Services
{
    public sealed class SystemUtilities
    {
        public static string GetOperationalSystemName()
        {
            return new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem")
                .Get()
                .Cast<ManagementObject>()
                .FirstOrDefault()?
                .GetPropertyValue("Caption")
                .ToString();             
        }
    }
}
