using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace YetAnotherMapManager.Core.Security;

internal class RsaSerializer(Type type) : AesSerializer(type)
{
  private static RSACryptoServiceProvider GetCryptoService()
  {
    return new RSACryptoServiceProvider(new CspParameters(1)
    {
      KeyContainerName = "SpiderContainer",
      Flags = CspProviderFlags.UseMachineKeyStore,
      ProviderName = "Microsoft Strong Cryptographic Provider"
    });
  }

  public string RsaSerializeObject(string publicKeyPath, string Hash, object Obj)
  {
    byte[] aesKey = this.GenerateAesKey();
    byte[] aesIv = this.GenerateAesIV();
    byte[] bytes = Encoding.UTF8.GetBytes(Hash);
    byte[] buffer1 = Convert.FromBase64String(this.AesSerializeObject(aesKey, aesIv, bytes, Obj));
    RSACryptoServiceProvider cryptoService = RsaSerializer.GetCryptoService();
    StreamReader streamReader = new StreamReader(publicKeyPath);
    string end = streamReader.ReadToEnd();
    cryptoService.FromXmlString(end);
    streamReader.Close();
    byte[] buffer2 = cryptoService.Encrypt(aesKey, false);
    byte[] buffer3 = cryptoService.Encrypt(aesIv, false);
    using (MemoryStream memoryStream = new MemoryStream())
    {
      memoryStream.Write(BitConverter.GetBytes(buffer2.Length), 0, 4);
      memoryStream.Write(BitConverter.GetBytes(buffer3.Length), 0, 4);
      memoryStream.Write(buffer2, 0, buffer2.Length);
      memoryStream.Write(buffer3, 0, buffer3.Length);
      memoryStream.Write(buffer1, 0, buffer1.Length);
      return Convert.ToBase64String(memoryStream.ToArray());
    }
  }

  public object RsaDeserializeObject(string privateKeyPath, string Hash, string cypheredData)
  {
    try
    {
      byte[] numArray1;
      byte[] numArray2;
      byte[] numArray3;
      using (MemoryStream memoryStream = new MemoryStream(Convert.FromBase64String(cypheredData)))
      {
        byte[] buffer = new byte[4];
        memoryStream.Read(buffer, 0, 4);
        int int32_1 = BitConverter.ToInt32(buffer, 0);
        memoryStream.Read(buffer, 0, 4);
        int int32_2 = BitConverter.ToInt32(buffer, 0);
        numArray1 = new byte[int32_1];
        memoryStream.Read(numArray1, 0, numArray1.Length);
        numArray2 = new byte[int32_2];
        memoryStream.Read(numArray2, 0, numArray2.Length);
        numArray3 = new byte[memoryStream.Length - 8L - (long) int32_1 - (long) int32_2];
        memoryStream.Read(numArray3, 0, numArray3.Length);
      }
      RSACryptoServiceProvider cryptoService = RsaSerializer.GetCryptoService();
      StreamReader streamReader = new StreamReader(privateKeyPath);
      string end = streamReader.ReadToEnd();
      cryptoService.FromXmlString(end);
      streamReader.Close();
      return this.AesDeserializeObject(cryptoService.Decrypt(numArray1, false), cryptoService.Decrypt(numArray2, false), Encoding.UTF8.GetBytes(Hash), Convert.ToBase64String(numArray3));
    }
    catch (Exception)
    {
      return this.RsaDeserializeObject(privateKeyPath, "", cypheredData);
    }
  }

  public static void GenerateKeyPair(string publicKeyPath, string privateKeyPath)
  {
    RSACryptoServiceProvider cryptoService = RsaSerializer.GetCryptoService();
    if (File.Exists(privateKeyPath))
      File.Delete(privateKeyPath);
    if (File.Exists(publicKeyPath))
      File.Delete(publicKeyPath);
    FileStream fileStream1 = new FileStream(privateKeyPath, FileMode.CreateNew, FileAccess.ReadWrite);
    StreamWriter streamWriter1 = new StreamWriter((Stream) fileStream1);
    string xmlString1 = cryptoService.ToXmlString(true);
    streamWriter1.Write(xmlString1);
    streamWriter1.Close();
    fileStream1.Close();
    FileStream fileStream2 = new FileStream(publicKeyPath, FileMode.CreateNew, FileAccess.ReadWrite);
    StreamWriter streamWriter2 = new StreamWriter((Stream) fileStream2);
    string xmlString2 = cryptoService.ToXmlString(false);
    streamWriter2.Write(xmlString2);
    streamWriter2.Close();
    fileStream2.Close();
  }
}
