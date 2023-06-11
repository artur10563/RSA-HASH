using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace RSALIB
{
    //Win32_BaseBoard - информация о материнской плате.
    //Win32_Processor - информация о процессоре.
    //Win32_PhysicalMemory - информация о физической памяти.
    //Win32_DiskDrive - информация о жестком диске.
    //Win32_NetworkAdapter - информация о сетевом адаптере.

    public static class WMI
    {
        //Властивості, які змінюються з часом , не включаються в інформацію процесора
        private static List<string> excludedProcessorProperties = new List<string>()
            {
                "Availability","CpuStatus","CurrentClockSpeed", "CurrentVoltage", "ErrorCleared", "ErrorDescription","LastErrorCode","LoadPercentage"
            };

        private static Dictionary<string, object> GetInfoHelper(string path)
        {
            ManagementClass management = new ManagementClass(path);
            ManagementObjectCollection managementCollections = management.GetInstances();
            PropertyDataCollection propertyDatas = management.Properties;

            Dictionary<string, object> information = new Dictionary<string, object>();

            foreach (ManagementObject item in managementCollections)
            {
                foreach (var property in propertyDatas)
                {
                    information.Add(property.Name, item.Properties[property.Name].Value);
                }
            }

            return information;
        }


        public static Dictionary<string, object> GetBaseBoardInfo()
        {
            return GetInfoHelper("Win32_BaseBoard");
        }
        public static Dictionary<string, object> GetProcessorInfo()
        {
            Dictionary<string, object> info = GetInfoHelper("Win32_Processor");

            Dictionary<string, object> filtered = info
                .Where(item => !excludedProcessorProperties.Contains(item.Key))
                .ToDictionary(item => item.Key, item => item.Value);
            filtered
            return filtered;

        }

    }

}
