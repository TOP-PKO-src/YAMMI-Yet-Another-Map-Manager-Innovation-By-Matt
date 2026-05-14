using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

#nullable disable
namespace YetAnotherMapManager.Core.Security;

internal class AesSerializer(Type type) : Serializer(type)
{
  private static int KEY_SIZE = 192 /*0xC0*/;

  public byte[] GenerateAesKey()
  {
    Aes aes = (Aes) new AesCryptoServiceProvider();
    aes.GenerateKey();
    return aes.Key;
  }

  public byte[] GenerateAesIV()
  {
    Aes aes = (Aes) new AesCryptoServiceProvider();
    aes.GenerateIV();
    return aes.IV;
  }

  public string AesSerializeObject(byte[] key, byte[] iv, byte[] hash, object obj)
  {
    string data = this.SerializeObject(obj);
    return AesSerializer.Encrypt(key, iv, hash, data);
  }

  public object AesDeserializeObject(byte[] key, byte[] iv, byte[] hash, string pXmlizedString)
  {
    return this.DeserializeObject(AesSerializer.Decrypt(key, iv, hash, pXmlizedString));
  }

  private static string Encrypt(byte[] key, byte[] iv, byte[] hash, string data)
  {
    if (string.IsNullOrEmpty(data))
      return "";
    byte[] bytes1 = new PasswordDeriveBytes(key, hash).GetBytes(AesSerializer.KEY_SIZE / 8);
    RijndaelManaged rijndaelManaged = new RijndaelManaged();
    rijndaelManaged.Mode = CipherMode.CBC;
    byte[] inArray = (byte[]) null;
    using (ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(bytes1, iv))
    {
      using (MemoryStream memoryStream = new MemoryStream())
      {
        using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, encryptor, CryptoStreamMode.Write))
        {
          byte[] bytes2 = Encoding.UTF8.GetBytes(data);
          cryptoStream.Write(bytes2, 0, bytes2.Length);
          cryptoStream.FlushFinalBlock();
          inArray = memoryStream.ToArray();
          memoryStream.Close();
          cryptoStream.Close();
        }
      }
    }
    rijndaelManaged.Clear();
    return Convert.ToBase64String(inArray);
  }

  private static string Decrypt(byte[] key, byte[] iv, byte[] hash, string data)
  {
    if (string.IsNullOrEmpty(data))
      return "";
    byte[] bytes = new PasswordDeriveBytes(key, hash).GetBytes(AesSerializer.KEY_SIZE / 8);
    RijndaelManaged rijndaelManaged = new RijndaelManaged();
    rijndaelManaged.Mode = CipherMode.CBC;
    byte[] buffer = Convert.FromBase64String(data);
    byte[] numArray = new byte[buffer.Length];
    int count = 0;
    using (ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes, iv))
    {
      using (MemoryStream memoryStream = new MemoryStream(buffer))
      {
        using (CryptoStream cryptoStream = new CryptoStream((Stream) memoryStream, decryptor, CryptoStreamMode.Read))
        {
          count = cryptoStream.Read(numArray, 0, numArray.Length);
          memoryStream.Close();
          cryptoStream.Close();
        }
      }
    }
    rijndaelManaged.Clear();
    return Encoding.UTF8.GetString(numArray, 0, count);
  }
}
