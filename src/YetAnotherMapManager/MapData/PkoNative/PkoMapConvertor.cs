using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using YetAnotherMapManager.Core.Security;
using YetAnotherMapManager.Debug;
using YetAnotherMapManager.Plugins;
using YetAnotherMapManager.Plugins.Native.Data;

#nullable disable
namespace YetAnotherMapManager.MapData.PkoNative;

internal class PkoMapConvertor
{
  private static int MAGIC = 780627;
  private static int H_SPLIT_SIZE = 8;
  private static int V_SPLIT_SIZE = 8;
  private static int NATIVE_TILE_SIZE = 15;

  public static MapInfo LoadMap(string filename)
  {
    PerfTimer.Instance.Start($"Load Map [{filename}]");
    FileStream input = File.OpenRead(filename);
    BinaryReader reader = new BinaryReader((Stream) input);
    _mapProperties header = PkoMapConvertor.readMapHeader(reader);
    MapInfo map = new MapInfo(header.mapWidth, header.mapHeight);
    PkoMapConvertor.storeNativeSectionLayout(header, map, input.Length);
    SeaLandMapData data1 = new SeaLandMapData();
    map.AddCustomData(SeaLandMapData.KEY, (IMapData) data1);
    data1.InitializeData(map);
    SolidMapData data2 = new SolidMapData();
    map.AddCustomData(SolidMapData.KEY, (IMapData) data2);
    data2.InitializeData(map);
    HeightMapData data3 = new HeightMapData();
    map.AddCustomData(HeightMapData.KEY, (IMapData) data3);
    data3.InitializeData(map);
    TextureMapData data4 = new TextureMapData();
    map.AddCustomData(TextureMapData.KEY, (IMapData) data4);
    data4.InitializeData(map);
    AreaMapData data5 = new AreaMapData();
    map.AddCustomData(AreaMapData.KEY, (IMapData) data5);
    data5.InitializeData(map);
    ColorMapData data6 = new ColorMapData();
    map.AddCustomData(ColorMapData.KEY, (IMapData) data6);
    data6.InitializeData(map);
    PvpMapData data7 = new PvpMapData();
    map.AddCustomData(PvpMapData.KEY, (IMapData) data7);
    data7.InitializeData(map);
    SafeZoneMapData data8 = new SafeZoneMapData();
    map.AddCustomData(SafeZoneMapData.KEY, (IMapData) data8);
    data8.InitializeData(map);
    SolidHeightMapData data9 = new SolidHeightMapData();
    map.AddCustomData(SolidHeightMapData.KEY, (IMapData) data9);
    data9.InitializeData(map);
    TextureBlendingMapData data10 = new TextureBlendingMapData();
    map.AddCustomData(TextureBlendingMapData.KEY, (IMapData) data10);
    data10.InitializeData(map);
    PkoMapConvertor.readMapData(header, map, reader);
    reader.Close();
    input.Close();
    GC.Collect();
    PerfTimer.Instance.Stop($"Load Map [{filename}]");
    PerfTimer.Instance.PrintStats();
    return map;
  }

  private static _mapProperties readMapHeader(BinaryReader reader)
  {
    PerfTimer.Instance.Start("Read Map Header");
    _mapProperties mapProperties = new _mapProperties();
    reader.BaseStream.Seek(0L, SeekOrigin.Begin);
    mapProperties.magic = reader.ReadInt32();
    mapProperties.mapWidth = reader.ReadInt32();
    mapProperties.mapHeight = reader.ReadInt32();
    mapProperties.widthBreakdownSize = reader.ReadInt32();
    mapProperties.heightBreakdownSize = reader.ReadInt32();
    mapProperties.xDivisionCount = mapProperties.mapWidth / mapProperties.widthBreakdownSize;
    mapProperties.yDivisionCount = mapProperties.mapHeight / mapProperties.heightBreakdownSize;
    int capacity = mapProperties.xDivisionCount * mapProperties.yDivisionCount;
    mapProperties.offsetList = new List<int>(capacity);
    for (int index = 0; index < capacity; ++index)
    {
      int num = reader.ReadInt32();
      mapProperties.offsetList.Add(num);
    }
    PerfTimer.Instance.Stop("Read Map Header");
    return mapProperties;
  }

  private static void readMapData(_mapProperties header, MapInfo map, BinaryReader reader)
  {
    PerfTimer.Instance.Start("Read Map Data");
    int sectionIndex = 0;
    foreach (int offset in header.offsetList)
    {
      if (offset != 0)
      {
        reader.BaseStream.Seek((long) offset, SeekOrigin.Begin);
        PkoMapConvertor.readMapSection(sectionIndex, header, map, reader);
      }
      ++sectionIndex;
    }
    PerfTimer.Instance.Stop("Read Map Data");
  }

  private static void storeNativeSectionLayout(_mapProperties header, MapInfo map, long fileSize)
  {
    int[] sectionOffsets = new int[header.offsetList.Count];
    for (int index = 0; index < header.offsetList.Count; ++index)
      sectionOffsets[index] = header.offsetList[index];
    map.SetNativeSectionLayout(header.widthBreakdownSize, header.heightBreakdownSize, sectionOffsets, fileSize);
  }

  private static void readMapSection(
    int sectionIndex,
    _mapProperties header,
    MapInfo map,
    BinaryReader reader)
  {
    PerfTimer.Instance.Start("Read Map Section");
    SeaLandMapData customData1 = (SeaLandMapData) map.GetCustomData(SeaLandMapData.KEY);
    SolidMapData customData2 = (SolidMapData) map.GetCustomData(SolidMapData.KEY);
    HeightMapData customData3 = (HeightMapData) map.GetCustomData(HeightMapData.KEY);
    TextureMapData customData4 = (TextureMapData) map.GetCustomData(TextureMapData.KEY);
    AreaMapData customData5 = (AreaMapData) map.GetCustomData(AreaMapData.KEY);
    PvpMapData customData6 = (PvpMapData) map.GetCustomData(PvpMapData.KEY);
    SafeZoneMapData customData7 = (SafeZoneMapData) map.GetCustomData(SafeZoneMapData.KEY);
    SolidHeightMapData customData8 = (SolidHeightMapData) map.GetCustomData(SolidHeightMapData.KEY);
    ColorMapData customData9 = (ColorMapData) map.GetCustomData(ColorMapData.KEY);
    TextureBlendingMapData customData10 = (TextureBlendingMapData) map.GetCustomData(TextureBlendingMapData.KEY);
    int num1 = sectionIndex % header.xDivisionCount;
    int num2 = sectionIndex / header.xDivisionCount;
    for (int index1 = 0; index1 < header.heightBreakdownSize; ++index1)
    {
      for (int index2 = 0; index2 < header.widthBreakdownSize; ++index2)
      {
        Tile tile = PkoMapConvertor.readMapTile(reader);
        int x = num1 * header.widthBreakdownSize + index2;
        int y = num2 * header.heightBreakdownSize + index1;
        if (tile.isBridge())
        {
          PerfTimer.Instance.Start("Set Tile Type");
          customData1.SetTileType(x, y, SeaLandMapData.TYPE_BRIDGE);
          PerfTimer.Instance.Stop("Set Tile Type");
        }
        else if (tile.isLand())
        {
          PerfTimer.Instance.Start("Set Tile Type");
          customData1.SetTileType(x, y, SeaLandMapData.TYPE_LAND);
          PerfTimer.Instance.Stop("Set Tile Type");
        }
        else
        {
          PerfTimer.Instance.Start("Set Tile Type");
          customData1.SetTileType(x, y, SeaLandMapData.TYPE_SEA);
          PerfTimer.Instance.Stop("Set Tile Type");
        }
        PerfTimer.Instance.Start("Set Solid blocks");
        customData2[x, y, 0, 0] = (128 /*0x80*/ & (int) tile.squares[0, 0]) == 128 /*0x80*/;
        customData2[x, y, 1, 0] = (128 /*0x80*/ & (int) tile.squares[1, 0]) == 128 /*0x80*/;
        customData2[x, y, 0, 1] = (128 /*0x80*/ & (int) tile.squares[0, 1]) == 128 /*0x80*/;
        customData2[x, y, 1, 1] = (128 /*0x80*/ & (int) tile.squares[1, 1]) == 128 /*0x80*/;
        PerfTimer.Instance.Stop("Set Solid blocks");
        PerfTimer.Instance.Start("Set Solid blocks height");
        customData8[x, y, 0, 0] = PkoMapConvertor.GetSolidHeight(tile.squares[0, 0]);
        customData8[x, y, 1, 0] = PkoMapConvertor.GetSolidHeight(tile.squares[1, 0]);
        customData8[x, y, 0, 1] = PkoMapConvertor.GetSolidHeight(tile.squares[0, 1]);
        customData8[x, y, 1, 1] = PkoMapConvertor.GetSolidHeight(tile.squares[1, 1]);
        PerfTimer.Instance.Stop("Set Solid blocks height");
        PerfTimer.Instance.Start("Set Height Map");
        customData3[x, y] = tile.cHeight;
        PerfTimer.Instance.Stop("Set Height Map");
        PerfTimer.Instance.Start("Set Texture Map");
        customData4[x, y] = tile.btTileInfo;
        PerfTimer.Instance.Stop("Set Height Map");
        PerfTimer.Instance.Start("Set Area Map");
        customData5[x, y] = tile.btIsland;
        PerfTimer.Instance.Stop("Set Area Map");
        PerfTimer.Instance.Start("Set Pvp Map");
        customData6[x, y] = ((int) tile.sRegion & 64 /*0x40*/) == 64 /*0x40*/;
        PerfTimer.Instance.Stop("Set Pvp Map");
        PerfTimer.Instance.Start("Safe Zone Map");
        customData7[x, y] = ((int) tile.sRegion & 2) == 2;
        PerfTimer.Instance.Stop("Safe Zone Map");
        PerfTimer.Instance.Start("Color Map");
        customData9[x, y] = tile.sColor;
        PerfTimer.Instance.Stop("Color Map");
        PerfTimer.Instance.Start("Test Map");
        customData10[x, y] = tile.dwTileInfo;
        PerfTimer.Instance.Stop("Test Map");
      }
    }
    PerfTimer.Instance.Stop("Read Map Section");
  }

  private static sbyte GetSolidHeight(sbyte value)
  {
    bool flag = ((int) value & 64 /*0x40*/) == 64 /*0x40*/;
    value &= (sbyte) 63 /*0x3F*/;
    value >>= 1;
    if (flag)
      value = (sbyte) ((int) ~value + 1);
    return value;
  }

  private static byte GetSolidHeightForPko(sbyte value, bool isBlocking)
  {
    byte num = (byte) ((uint) value & (uint) byte.MaxValue);
    bool flag = ((int) num & 128 /*0x80*/) == 128 /*0x80*/;
    if (flag)
      num = (byte) ((uint) ~num + 1U);
    byte solidHeightForPko = (byte) ((uint) num << 1);
    if (isBlocking)
      solidHeightForPko |= (byte) 128 /*0x80*/;
    if (flag)
      solidHeightForPko |= (byte) 64 /*0x40*/;
    return solidHeightForPko;
  }

  private static Tile readMapTile(BinaryReader reader)
  {
    PerfTimer.Instance.Start("Read Map Tile");
    Tile tile = new Tile()
    {
      dwTileInfo = reader.ReadUInt32(),
      btTileInfo = reader.ReadByte(),
      sColor = reader.ReadUInt16(),
      cHeight = reader.ReadSByte(),
      sRegion = reader.ReadInt16(),
      btIsland = reader.ReadByte(),
      squares = new sbyte[2, 2]
    };
    tile.squares[0, 0] = reader.ReadSByte();
    tile.squares[1, 0] = reader.ReadSByte();
    tile.squares[0, 1] = reader.ReadSByte();
    tile.squares[1, 1] = reader.ReadSByte();
    PerfTimer.Instance.Stop("Read Map Tile");
    return tile;
  }

  public static void WriteMap(License l, string filename, MapInfo map)
  {
    PerfTimer.Instance.Start($"Write Map [{filename}]");
    FileStream output = File.Create(filename);
    BinaryWriter writer = new BinaryWriter((Stream) output);
    bool[] sectionWriteMask = PkoMapConvertor.getSectionWriteMask(map);
    int sectionWidth = PkoMapConvertor.getSectionWidth(map);
    int sectionHeight = PkoMapConvertor.getSectionHeight(map);
    int[] sectionOffsets = PkoMapConvertor.getSectionOffsets(map, sectionWriteMask, sectionWidth, sectionHeight);
    PkoMapConvertor.writeMapHeader(l, map, writer, sectionOffsets, sectionWidth, sectionHeight);
    PkoMapConvertor.writeMapData(map, writer, sectionOffsets, sectionWidth, sectionHeight);
    PkoMapConvertor.applyNativeFileLengthHint(map, writer, sectionOffsets, sectionWidth, sectionHeight);
    writer.Close();
    output.Close();
    PerfTimer.Instance.Stop($"Write Map [{filename}]");
    PerfTimer.Instance.PrintStats();
  }

  private static int getSectionWidth(MapInfo map)
  {
    return map.NativeSectionWidth > 0 ? map.NativeSectionWidth : PkoMapConvertor.H_SPLIT_SIZE;
  }

  private static int getSectionHeight(MapInfo map)
  {
    return map.NativeSectionHeight > 0 ? map.NativeSectionHeight : PkoMapConvertor.V_SPLIT_SIZE;
  }

  private static int getSectionByteSize(int sectionWidth, int sectionHeight)
  {
    return sectionWidth * sectionHeight * PkoMapConvertor.NATIVE_TILE_SIZE;
  }

  private static void writeMapHeader(
    License l,
    MapInfo map,
    BinaryWriter writer,
    int[] sectionOffsets,
    int sectionWidth,
    int sectionHeight)
  {
    writer.Write(PkoMapConvertor.MAGIC);
    writer.Write(map.Width);
    writer.Write(map.Height);
    writer.Write(sectionWidth);
    writer.Write(sectionHeight);
    int num1 = map.Width / sectionWidth * (map.Height / sectionHeight);
    for (int index = 0; index < num1; ++index)
      writer.Write(sectionOffsets[index]);
  }

  private static bool[] getSectionWriteMask(MapInfo map)
  {
    int sectionWidth = PkoMapConvertor.getSectionWidth(map);
    int sectionHeight = PkoMapConvertor.getSectionHeight(map);
    int num1 = map.Width / sectionWidth;
    int num2 = map.Height / sectionHeight;
    int num3 = num1 * num2;
    bool[] sectionPresence;
    bool flag = map.TryGetNativeSectionPresence(sectionWidth, sectionHeight, num3, out sectionPresence);
    bool[] sectionWriteMask = new bool[num3];
    int sectionIndex = 0;
    for (int index1 = 0; index1 < num2; ++index1)
    {
      for (int index2 = 0; index2 < num1; ++index2)
      {
        bool isPresentInSource = flag && sectionPresence[sectionIndex];
        sectionWriteMask[sectionIndex] = isPresentInSource || !PkoMapConvertor.isDefaultSection(map, index2, index1, sectionWidth, sectionHeight);
        ++sectionIndex;
      }
    }
    return sectionWriteMask;
  }

  private static int[] getSectionOffsets(
    MapInfo map,
    bool[] sectionWriteMask,
    int sectionWidth,
    int sectionHeight)
  {
    int sectionCount = sectionWriteMask.Length;
    int headerSize = 20 + sectionCount * 4;
    int sectionByteSize = PkoMapConvertor.getSectionByteSize(sectionWidth, sectionHeight);
    int[] outputOffsets = new int[sectionCount];
    int[] sourceOffsets;
    if (map.TryGetNativeSectionOffsets(sectionWidth, sectionHeight, sectionCount, out sourceOffsets))
    {
      for (int index = 0; index < sectionCount; ++index)
      {
        if (sectionWriteMask[index] && sourceOffsets[index] > 0)
          outputOffsets[index] = sourceOffsets[index];
      }
    }
    int nextOffset = headerSize;
    for (int index = 0; index < sectionCount; ++index)
    {
      if (outputOffsets[index] > 0)
        nextOffset = Math.Max(nextOffset, outputOffsets[index] + sectionByteSize);
    }
    for (int index = 0; index < sectionCount; ++index)
    {
      if (sectionWriteMask[index] && outputOffsets[index] == 0)
      {
        outputOffsets[index] = nextOffset;
        nextOffset += sectionByteSize;
      }
    }
    return outputOffsets;
  }

  private static void applyNativeFileLengthHint(
    MapInfo map,
    BinaryWriter writer,
    int[] sectionOffsets,
    int sectionWidth,
    int sectionHeight)
  {
    int sectionByteSize = PkoMapConvertor.getSectionByteSize(sectionWidth, sectionHeight);
    long num = 20 + sectionOffsets.Length * 4;
    foreach (int sectionOffset in sectionOffsets)
    {
      if (sectionOffset > 0)
        num = Math.Max(num, (long) sectionOffset + (long) sectionByteSize);
    }
    if (map.NativeFileSize > num)
      num = map.NativeFileSize;
    writer.BaseStream.SetLength(num);
  }

  private static bool isDefaultSection(
    MapInfo map,
    int sectionX,
    int sectionY,
    int sectionWidth,
    int sectionHeight)
  {
    SeaLandMapData customData1 = (SeaLandMapData) map.GetCustomData(SeaLandMapData.KEY);
    SolidMapData customData2 = (SolidMapData) map.GetCustomData(SolidMapData.KEY);
    HeightMapData customData3 = (HeightMapData) map.GetCustomData(HeightMapData.KEY);
    TextureMapData customData4 = (TextureMapData) map.GetCustomData(TextureMapData.KEY);
    PvpMapData customData5 = (PvpMapData) map.GetCustomData(PvpMapData.KEY);
    AreaMapData customData6 = (AreaMapData) map.GetCustomData(AreaMapData.KEY);
    SafeZoneMapData customData7 = (SafeZoneMapData) map.GetCustomData(SafeZoneMapData.KEY);
    SolidHeightMapData customData8 = (SolidHeightMapData) map.GetCustomData(SolidHeightMapData.KEY);
    ColorMapData customData9 = (ColorMapData) map.GetCustomData(ColorMapData.KEY);
    TextureBlendingMapData customData10 = (TextureBlendingMapData) map.GetCustomData(TextureBlendingMapData.KEY);
    if (customData1 == null)
      return false;
    for (int index1 = 0; index1 < sectionHeight; ++index1)
    {
      for (int index2 = 0; index2 < sectionWidth; ++index2)
      {
        int x = sectionX * sectionWidth + index2;
        int y = sectionY * sectionHeight + index1;
        if ((int) customData1[x, y] != (int) SeaLandMapData.TYPE_SEA)
          return false;
        if (customData2 != null && (customData2[x, y, 0, 0] || customData2[x, y, 1, 0] || customData2[x, y, 0, 1] || customData2[x, y, 1, 1]))
          return false;
        if (customData3 != null && (int) customData3[x, y] != -100)
          return false;
        if (customData4 != null && customData4[x, y] != (byte) 0)
          return false;
        if (customData5 != null && customData5[x, y])
          return false;
        if (customData6 != null && customData6[x, y] != (byte) 0)
          return false;
        if (customData7 != null && customData7[x, y])
          return false;
        if (customData8 != null && ((int) customData8[x, y, 0, 0] != 0 || (int) customData8[x, y, 1, 0] != 0 || (int) customData8[x, y, 0, 1] != 0 || (int) customData8[x, y, 1, 1] != 0))
          return false;
        if (customData9 != null && customData9[x, y] != ushort.MaxValue)
          return false;
        if (customData10 != null && customData10[x, y] != 0U)
          return false;
      }
    }
    return true;
  }

  private static void writeMapData(
    MapInfo map,
    BinaryWriter writer,
    int[] sectionOffsets,
    int sectionWidth,
    int sectionHeight)
  {
    int num1 = map.Width / sectionWidth;
    int num2 = map.Height / sectionHeight;
    int sectionIndex = 0;
    SeaLandMapData customData1 = (SeaLandMapData) map.GetCustomData(SeaLandMapData.KEY);
    SolidMapData customData2 = (SolidMapData) map.GetCustomData(SolidMapData.KEY);
    HeightMapData customData3 = (HeightMapData) map.GetCustomData(HeightMapData.KEY);
    TextureMapData customData4 = (TextureMapData) map.GetCustomData(TextureMapData.KEY);
    PvpMapData customData5 = (PvpMapData) map.GetCustomData(PvpMapData.KEY);
    AreaMapData customData6 = (AreaMapData) map.GetCustomData(AreaMapData.KEY);
    SafeZoneMapData customData7 = (SafeZoneMapData) map.GetCustomData(SafeZoneMapData.KEY);
    SolidHeightMapData customData8 = (SolidHeightMapData) map.GetCustomData(SolidHeightMapData.KEY);
    ColorMapData customData9 = (ColorMapData) map.GetCustomData(ColorMapData.KEY);
    TextureBlendingMapData customData10 = (TextureBlendingMapData) map.GetCustomData(TextureBlendingMapData.KEY);
    for (int index1 = 0; index1 < num2; ++index1)
    {
      for (int index2 = 0; index2 < num1; ++index2)
      {
        if (sectionOffsets[sectionIndex] == 0)
        {
          ++sectionIndex;
          continue;
        }
        writer.BaseStream.Seek((long) sectionOffsets[sectionIndex], SeekOrigin.Begin);
        for (int index3 = 0; index3 < sectionHeight; ++index3)
        {
          for (int index4 = 0; index4 < sectionWidth; ++index4)
          {
            int x = index2 * sectionWidth + index4;
            int y = index1 * sectionHeight + index3;
            byte num4 = customData4[x, y];
            if (customData10 == null)
            {
              if ((int) customData1[x, y] == (int) SeaLandMapData.TYPE_SEA)
                writer.Write(62935040 /*0x03C05000*/);
              else
                writer.Write(0);
            }
            else
              writer.Write(customData10[x, y]);
            writer.Write(num4);
            writer.Write(customData9[x, y]);
            writer.Write(customData3[x, y]);
            short num5 = 0;
            if ((int) customData1[x, y] == (int) SeaLandMapData.TYPE_BRIDGE)
              num5 |= (short) 8;
            else if ((int) customData1[x, y] == (int) SeaLandMapData.TYPE_LAND)
              num5 |= (short) 1;
            if (customData5[x, y])
              num5 |= (short) 64 /*0x40*/;
            if (customData7[x, y])
              num5 |= (short) 2;
            writer.Write(num5);
            writer.Write(customData6[x, y]);
            sbyte num6;
            sbyte num7;
            sbyte num8;
            sbyte num9;
            if (customData8 != null)
            {
              num6 = customData8[x, y, 0, 0];
              num7 = customData8[x, y, 1, 0];
              num8 = customData8[x, y, 0, 1];
              num9 = customData8[x, y, 1, 1];
            }
            else
            {
              num6 = customData3[x, y];
              num7 = customData3[x, y];
              num8 = customData3[x, y];
              num9 = customData3[x, y];
            }
            byte solidHeightForPko1 = PkoMapConvertor.GetSolidHeightForPko(num6, customData2[x, y, 0, 0]);
            byte solidHeightForPko2 = PkoMapConvertor.GetSolidHeightForPko(num7, customData2[x, y, 1, 0]);
            byte solidHeightForPko3 = PkoMapConvertor.GetSolidHeightForPko(num8, customData2[x, y, 0, 1]);
            byte solidHeightForPko4 = PkoMapConvertor.GetSolidHeightForPko(num9, customData2[x, y, 1, 1]);
            writer.Write(solidHeightForPko1);
            writer.Write(solidHeightForPko2);
            writer.Write(solidHeightForPko3);
            writer.Write(solidHeightForPko4);
          }
        }
        ++sectionIndex;
      }
    }
  }

  public static void WriteATR(string filename, MapInfo map)
  {
    int num1 = (int) MessageBox.Show("For the time being I'm focusing on creating new maps.\nGenerating .ATR for an official map might result in a bit of behavioural changes that might in most of the case be unseen (maybe issues with mining areas).");
    PerfTimer.Instance.Start($"Write ATR [{filename}]");
    FileStream output = File.Create(filename);
    BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
    binaryWriter.Write(map.Width);
    binaryWriter.Write(map.Height);
    SeaLandMapData customData1 = (SeaLandMapData) map.GetCustomData(SeaLandMapData.KEY);
    PvpMapData customData2 = (PvpMapData) map.GetCustomData(PvpMapData.KEY);
    AreaMapData customData3 = (AreaMapData) map.GetCustomData(AreaMapData.KEY);
    SafeZoneMapData customData4 = (SafeZoneMapData) map.GetCustomData(SafeZoneMapData.KEY);
    for (int y = 0; y < map.Height; ++y)
    {
      for (int x = 0; x < map.Width; ++x)
      {
        short num2 = 0;
        if ((int) customData1[x, y] == (int) SeaLandMapData.TYPE_BRIDGE)
          num2 |= (short) 8;
        else if ((int) customData1[x, y] == (int) SeaLandMapData.TYPE_LAND)
          num2 |= (short) 1;
        if (customData2[x, y])
          num2 |= (short) 64 /*0x40*/;
        if (customData4[x, y])
          num2 |= (short) 2;
        binaryWriter.Write(num2);
        binaryWriter.Write(customData3[x, y]);
      }
    }
    binaryWriter.Close();
    output.Close();
    PerfTimer.Instance.Stop($"Write ATR [{filename}]");
    PerfTimer.Instance.PrintStats();
  }

  public static void WriteBLK(string filename, MapInfo map)
  {
    PerfTimer.Instance.Start($"Write BLK [{filename}]");
    FileStream output = File.Create(filename);
    BinaryWriter binaryWriter = new BinaryWriter((Stream) output);
    SolidMapData customData = (SolidMapData) map.GetCustomData(SolidMapData.KEY);
    binaryWriter.Write(map.Width * 2);
    binaryWriter.Write(map.Height * 2);
    int num1 = map.Width * 2 * map.Height * 2 / 8;
    for (int index = 0; index < num1; ++index)
    {
      byte num2 = 0;
      if (customData[index * 8])
        num2 |= (byte) 128 /*0x80*/;
      if (customData[index * 8 + 1])
        num2 |= (byte) 64 /*0x40*/;
      if (customData[index * 8 + 2])
        num2 |= (byte) 32 /*0x20*/;
      if (customData[index * 8 + 3])
        num2 |= (byte) 16 /*0x10*/;
      if (customData[index * 8 + 4])
        num2 |= (byte) 8;
      if (customData[index * 8 + 5])
        num2 |= (byte) 4;
      if (customData[index * 8 + 6])
        num2 |= (byte) 2;
      if (customData[index * 8 + 7])
        num2 |= (byte) 1;
      binaryWriter.Write(num2);
    }
    binaryWriter.Close();
    output.Close();
    PerfTimer.Instance.Stop($"Write BLK [{filename}]");
    PerfTimer.Instance.PrintStats();
  }
}
