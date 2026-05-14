using System;
using System.IO;
using System.Windows.Forms;

#nullable disable
namespace YetAnotherMapManager.Core.Security;

internal class LicenseKeyValidator
{
  public static License GetLicense(string EncryptedLicense)
  {
    License license = (License) null;
    string privateKeyPath = "RsaPrivateKey.xml";
    try
    {
      string uniqueId = HardIdGenerator.getUniqueID();
      license = (License) new RsaSerializer(typeof (License)).RsaDeserializeObject(privateKeyPath, uniqueId, EncryptedLicense);
      if (!LicenseKeyValidator.IsLicenseValid(license))
        license = (License) null;
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show($"{ex.Message}\n{ex.StackTrace}");
      Console.WriteLine(ex.Message);
    }
    return license;
  }

  public static bool IsLicenseValid(License license)
  {
    string path = "settings.xml";
    DateTime creationTime = File.GetCreationTime(path);
    DateTime lastAccessTime = File.GetLastAccessTime(path);
    DateTime lastWriteTime = File.GetLastWriteTime(path);
    return !((!(creationTime > lastAccessTime) ? (!(lastAccessTime > lastWriteTime) ? lastWriteTime : lastAccessTime) : (!(creationTime > lastWriteTime) ? lastWriteTime : creationTime)) > DateTime.Now) && !(license.endTime < DateTime.Now);
  }
}
