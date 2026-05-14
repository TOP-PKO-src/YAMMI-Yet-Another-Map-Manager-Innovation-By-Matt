using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using YetAnotherMapManager.Core;

#nullable disable
namespace YetAnotherMapManager.MapData.PkoNative;

internal class BinFileConvertor
{
  private static string textureFilePath = "\\scripts\\table\\TerrainInfo.bin";
  private static string areaFilePath = "\\scripts\\table\\AreaSet.bin";
  private static int CHUNK_SIZE_TEXTURE = 120;
  private static int CHUNK_SIZE_AREA = 140;
  private static byte[] cypherKey = new byte[18]
  {
    (byte) 152,
    (byte) 157,
    (byte) 159,
    (byte) 104,
    (byte) 224 /*0xE0*/,
    (byte) 102,
    (byte) 171,
    (byte) 112 /*0x70*/,
    (byte) 233,
    (byte) 209,
    (byte) 224 /*0xE0*/,
    (byte) 224 /*0xE0*/,
    (byte) 203,
    (byte) 221,
    (byte) 209,
    (byte) 203,
    (byte) 213,
    (byte) 207
  };

  public static List<TextureDef> LoadTextures(string clientPath, string cachePath)
  {
    FileStream input = File.OpenRead(clientPath + BinFileConvertor.textureFilePath);
    BinaryReader binaryReader = new BinaryReader((Stream) input);
    int num1 = binaryReader.ReadInt32();
    if (BinFileConvertor.CHUNK_SIZE_TEXTURE != num1)
    {
      int num2 = (int) MessageBox.Show($"Wrong file format for [{clientPath}{BinFileConvertor.textureFilePath}]");
      return (List<TextureDef>) null;
    }
    long num3 = input.Length / (long) num1;
    List<TextureDef> textureDefList = new List<TextureDef>();
    for (int index = 0; (long) index < num3; ++index)
    {
      binaryReader.ReadInt32();
      int id = binaryReader.ReadInt32();
      char[] chArray = binaryReader.ReadChars(76);
      int length = 0;
      while (length < chArray.Length && chArray[length] != char.MinValue)
        ++length;
      string filename = new string(chArray, 0, length);
      binaryReader.ReadInt32();
      binaryReader.ReadInt32();
      binaryReader.ReadInt32();
      binaryReader.ReadInt32();
      binaryReader.ReadInt32();
      binaryReader.ReadInt32();
      int flag1 = binaryReader.ReadInt32();
      binaryReader.ReadInt32();
      int flag2 = binaryReader.ReadInt32();
      textureDefList.Add(new TextureDef(id, filename, flag1, flag2, Color.Transparent));
    }
    binaryReader.Close();
    input.Close();
    List<Color> colorList = new List<Color>();
    System.Type type = Color.White.GetType();
    if ((System.Type) null != type)
    {
      PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
      int length = properties.Length;
      for (int index = 0; index < length; ++index)
      {
        Color color = (Color) properties[index].GetValue((object) null, (object[]) null);
        if (color != Color.Transparent && color != Color.Red)
          colorList.Add(color);
      }
    }
    for (int index = 0; index < textureDefList.Count; ++index)
      textureDefList[index].RenderColor = colorList[index % colorList.Count];
    foreach (TextureDef textureDef in textureDefList)
    {
      textureDef.CachedBitmapPath = $"{cachePath}_{(object) textureDef.Id}.bmp";
      BinFileConvertor.extractTextureBitmap($"{clientPath}\\{textureDef.Filename}", textureDef.CachedBitmapPath);
    }
    return textureDefList;
  }

  public static void extractTextureBitmap(string sourceFilename, string targetCacheFile)
  {
    if (!File.Exists(sourceFilename))
      return;
    FileStream fileStream1 = File.OpenRead(sourceFilename);
    FileStream fileStream2 = File.OpenWrite(targetCacheFile);
    byte[] buffer1 = new byte[4];
    fileStream1.Seek(-4L, SeekOrigin.End);
    fileStream1.Read(buffer1, 0, 4);
    if (buffer1[0] == (byte) 109 && buffer1[1] == (byte) 112 /*0x70*/ && buffer1[2] == (byte) 46 && buffer1[3] == (byte) 120)
    {
      byte[] buffer2 = new byte[44];
      byte[] buffer3 = new byte[44];
      fileStream1.Seek(-48L, SeekOrigin.End);
      fileStream1.Read(buffer2, 0, 44);
      fileStream1.Seek(0L, SeekOrigin.Begin);
      fileStream1.Read(buffer3, 0, 44);
      fileStream2.Write(buffer2, 0, 44);
      int count = (int) (fileStream1.Length - 4L - 44L - 44L);
      byte[] buffer4 = new byte[count];
      fileStream1.Read(buffer4, 0, count);
      fileStream2.Write(buffer4, 0, count);
      fileStream2.Write(buffer3, 0, 44);
    }
    else
    {
      fileStream1.Seek(0L, SeekOrigin.Begin);
      byte[] buffer5 = new byte[1024 /*0x0400*/];
      int count;
      while ((count = fileStream1.Read(buffer5, 0, buffer5.Length)) > 0)
        fileStream2.Write(buffer5, 0, count);
    }
    fileStream2.Close();
    fileStream1.Close();
  }

  public static List<AreaDef> LoadAreas(string clientPath)
  {
    FileStream input = File.OpenRead(clientPath + BinFileConvertor.areaFilePath);
    BinaryReader binaryReader = new BinaryReader((Stream) input);
    int num1 = binaryReader.ReadInt32();
    if (BinFileConvertor.CHUNK_SIZE_AREA != num1)
    {
      int num2 = (int) MessageBox.Show($"Wrong file format for [{clientPath}{BinFileConvertor.areaFilePath}]");
      return (List<AreaDef>) null;
    }
    long num3 = input.Length / (long) num1;
    List<AreaDef> areaDefList = new List<AreaDef>();
    for (int index1 = 0; (long) index1 < num3; ++index1)
    {
      int id;
      string name;
      if (binaryReader.ReadInt32() == 1)
      {
        id = binaryReader.ReadInt32();
        char[] chArray = binaryReader.ReadChars(76);
        int length = 0;
        while (length < chArray.Length && chArray[length] != char.MinValue)
          ++length;
        name = new string(chArray, 0, length);
      }
      else
      {
        byte[] numArray1 = binaryReader.ReadBytes(4);
        id = (int) numArray1[0] - (int) BinFileConvertor.cypherKey[4] & (int) byte.MaxValue | ((int) numArray1[1] - (int) BinFileConvertor.cypherKey[5] & (int) byte.MaxValue) << 8 | ((int) numArray1[2] - (int) BinFileConvertor.cypherKey[6] & (int) byte.MaxValue) << 16 /*0x10*/ | ((int) numArray1[3] - (int) BinFileConvertor.cypherKey[7] & (int) byte.MaxValue) << 24;
        byte[] numArray2 = binaryReader.ReadBytes(76);
        int length;
        for (length = 0; length < numArray2.Length; ++length)
        {
          numArray2[length] = (byte) ((int) numArray2[length] - (int) BinFileConvertor.cypherKey[(8 + length) % BinFileConvertor.cypherKey.Length] & (int) byte.MaxValue);
          if (numArray2[length] == (byte) 0)
            break;
        }
        byte[] bytes = new byte[length];
        for (int index2 = 0; index2 < length; ++index2)
          bytes[index2] = numArray2[index2];
        name = Encoding.ASCII.GetString(bytes);
      }
      binaryReader.ReadBytes(num1 - 84);
      areaDefList.Add(new AreaDef(id, name, Color.Transparent));
    }
    binaryReader.Close();
    input.Close();
    List<Color> colorList = new List<Color>();
    System.Type type = Color.White.GetType();
    if ((System.Type) null != type)
    {
      PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
      int length = properties.Length;
      for (int index = 0; index < length; ++index)
      {
        Color color = (Color) properties[index].GetValue((object) null, (object[]) null);
        if (color != Color.Transparent && color != Color.Red)
          colorList.Add(color);
      }
    }
    for (int index = 0; index < areaDefList.Count; ++index)
      areaDefList[index].RenderColor = colorList[index % colorList.Count];
    return areaDefList;
  }
}
