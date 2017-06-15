using System.Linq;
using System.Management;

namespace ZebraViewer.Helpers
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
