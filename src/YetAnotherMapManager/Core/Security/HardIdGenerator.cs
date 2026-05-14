using System;
using System.IO;
using System.Management;

#nullable disable
namespace YetAnotherMapManager.Core.Security;

internal class HardIdGenerator
{
  public static string getUniqueID() => HardIdGenerator.getUniqueID("");

  private static string getUniqueID(string drive)
  {
    if (drive == string.Empty)
    {
      drive = "aASDH";
      foreach (DriveInfo drive1 in DriveInfo.GetDrives())
      {
        try
        {
          if (drive1.IsReady)
          {
            try
            {
              drive = drive1.RootDirectory.ToString();
              break;
            }
            catch (Exception)
            {
            }
          }
        }
        catch (Exception)
        {
        }
      }
    }
    if (drive.EndsWith(":\\"))
      drive = drive.Substring(0, drive.Length - 2);
    string str1 = "aA1mL^H";
    try
    {
      str1 = HardIdGenerator.getVolumeSerial(drive);
    }
    catch (Exception)
    {
    }
    string str2 = "xp5fDFK345C^d45FF3{~FSffffs144";
    try
    {
      str2 = HardIdGenerator.getCPUID();
    }
    catch (Exception)
    {
    }
    return str2.Substring(13) + str2.Substring(1, 4) + str1 + str2.Substring(4, 4);
  }

  private static string getVolumeSerial(string drive)
  {
    ManagementObject managementObject = new ManagementObject($"win32_logicaldisk.deviceid=\"{drive}:\"");
    managementObject.Get();
    string volumeSerial = managementObject["VolumeSerialNumber"].ToString();
    managementObject.Dispose();
    return volumeSerial;
  }

  private static string getCPUID()
  {
    string cpuid = "";
    foreach (ManagementObject instance in new ManagementClass("win32_processor").GetInstances())
    {
      if (cpuid == "")
      {
        cpuid = instance.Properties["processorID"].Value.ToString();
        break;
      }
    }
    return cpuid;
  }
}
