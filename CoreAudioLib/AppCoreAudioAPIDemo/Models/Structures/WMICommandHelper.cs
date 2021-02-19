using System;
using System.Management;

namespace AppCoreAudioAPIDemo.Models.Structures
{
    /// <summary>
    /// Ajax WMI command for all HP platform
    /// </summary>
    class WMICommandHelper
    {
        private static byte[] sign = { 83, 69, 67, 85 };
        public static int Execute(int command, int commandType, int inputDataSize, byte[] inputData, out byte[] returnData)
        {
            returnData = new byte[0];
            try
            {
                ManagementObject classInstance = new ManagementObject("root\\wmi", "hpqBIntM.InstanceName='ACPI\\PNP0C14\\0_0'", null);
                ManagementObject DataIn = new ManagementClass("root\\wmi:hpqBDataIn");
                ManagementBaseObject inParams = classInstance.GetMethodParameters("hpqBIOSInt128");
                ManagementBaseObject DataOut = new ManagementClass("root\\wmi:hpqBDataOut128");

                DataIn["Sign"] = sign;
                DataIn["Command"] = command;
                DataIn["CommandType"] = commandType;
                DataIn["Size"] = inputDataSize;
                DataIn["hpqBData"] = inputData;

                inParams["InData"] = DataIn as ManagementBaseObject;
                InvokeMethodOptions methodOptions = new InvokeMethodOptions { Timeout = System.TimeSpan.MaxValue, };

                ManagementBaseObject outParams = classInstance.InvokeMethod("hpqBIOSInt128", inParams, methodOptions);
                DataOut = outParams["OutData"] as ManagementBaseObject;

                returnData = (DataOut["Data"] as byte[]);

                return Convert.ToInt32(DataOut["rwReturnCode"]);
            }
            catch (Exception err)
            {
                Console.WriteLine("WmiCommand.Execute occurs exception: " + err);
                return -999;
            }
        }
    }
}
