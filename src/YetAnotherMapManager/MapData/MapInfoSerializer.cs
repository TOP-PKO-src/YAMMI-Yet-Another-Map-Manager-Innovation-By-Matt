using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

#nullable disable
namespace YetAnotherMapManager.MapData;

internal class MapInfoSerializer
{
  public static void Serialize(MapInfo mapInfo, string filename)
  {
    Stream serializationStream = (Stream) null;
    try
    {
      IFormatter formatter = (IFormatter) new BinaryFormatter();
      serializationStream = (Stream) new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
      formatter.Serialize(serializationStream, (object) MapInfo.DATA_FORMAT_VERSION);
      formatter.Serialize(serializationStream, (object) mapInfo);
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

  public static MapInfo Unserialize(string filename)
  {
    Stream serializationStream = (Stream) null;
    MapInfo mapInfo = (MapInfo) null;
    try
    {
      IFormatter formatter = (IFormatter) new BinaryFormatter();
      serializationStream = (Stream) new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.None);
      int num = (int) formatter.Deserialize(serializationStream);
      mapInfo = (MapInfo) formatter.Deserialize(serializationStream);
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show($"Unserialization failed !\n{ex.Message}\n{ex.StackTrace}");
    }
    finally
    {
      serializationStream?.Close();
    }
    return mapInfo;
  }
}
