using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

#nullable disable
namespace YetAnotherMapManager.Core;

internal class SettingsSerializer
{
  private static string CACHE_FOLDER = "Cache";
  private static string TEXTURE_CACHE_FOLDER = SettingsSerializer.CACHE_FOLDER + "/Textures";

  public static void Serialize(Settings settings, string filename)
  {
    Stream serializationStream = (Stream) null;
    try
    {
      IFormatter formatter = (IFormatter) new BinaryFormatter();
      serializationStream = (Stream) new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
      formatter.Serialize(serializationStream, (object) Settings.DATA_FORMAT_VERSION);
      formatter.Serialize(serializationStream, (object) settings);
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show($"Serialization failed !\n{ex.Message}\n{ex.StackTrace}");
    }
    finally
    {
      serializationStream?.Close();
    }
  }

  public static Settings Unserialize(string filename)
  {
    Stream serializationStream = (Stream) null;
    Settings settings = (Settings) null;
    try
    {
      IFormatter formatter = (IFormatter) new BinaryFormatter();
      serializationStream = (Stream) new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None);
      int num = (int) formatter.Deserialize(serializationStream);
      settings = (Settings) formatter.Deserialize(serializationStream);
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show($"Unserialization failed !\n{ex.Message}\n{ex.StackTrace}");
    }
    finally
    {
      serializationStream?.Close();
    }
    return settings;
  }

  private string UTF8ByteArrayToString(byte[] characters)
  {
    return new UTF8Encoding().GetString(characters);
  }

  private byte[] StringToUTF8ByteArray(string pXmlString)
  {
    return new UTF8Encoding().GetBytes(pXmlString);
  }

  public static void SaveAsXML(Settings settings, string filename)
  {
    FileStream fileStream = (FileStream) null;
    try
    {
      fileStream = new FileStream(filename, FileMode.Create);
      new XmlSerializer(typeof (Settings)).Serialize((Stream) fileStream, (object) settings);
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show("Failed to serialize settings: \n" + ex.Message);
    }
    finally
    {
      fileStream?.Close();
    }
  }

  public static Settings LoadFromXML(string filename)
  {
    Settings settings = (Settings) null;
    FileStream fileStream = (FileStream) null;
    try
    {
      fileStream = new FileStream(filename, FileMode.Open);
      settings = (Settings) new XmlSerializer(typeof (Settings)).Deserialize((Stream) fileStream);
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show("Failed to load settings: \n" + ex.Message);
    }
    finally
    {
      fileStream?.Close();
    }
    return settings;
  }
}
