using System;
using System.Collections.Generic;
using YetAnotherMapManager.Common;
using YetAnotherMapManager.MapData;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Data;

[Serializable]
public class AreaMapData : IMapData
{
  private ByteArray_2D areas;
  private List<byte> areaList;
  public static string KEY = "AREA";

  public List<byte> AreaList => this.areaList;

  public byte this[int x, int y]
  {
    set
    {
      if (!this.areaList.Contains(value))
        this.areaList.Add(value);
      this.areas[x, y] = value;
    }
    get => this.areas[x, y];
  }

  public void InitializeData(int width, int height)
  {
    this.areas = new ByteArray_2D(width, height, (byte) 0);
  }

  public void InitializeData(MapInfo map) => this.InitializeData(map.Width, map.Height);

  public bool OverwriteData(ByteArray_2D input)
  {
    if (input.Width != this.areas.Width || input.Height != this.areas.Height)
      return false;
    this.areas = input;
    for (int x = 0; x < this.areas.Width; ++x)
    {
      for (int y = 0; y < this.areas.Height; ++y)
      {
        byte area = this.areas[x, y];
        if (!this.areaList.Contains(area))
          this.areaList.Add(area);
      }
    }
    return true;
  }

  public void CopyArea(
    IMapData toMapData,
    int toX,
    int toY,
    int fromX,
    int fromY,
    int width,
    int height)
  {
    for (int index1 = 0; index1 < width; ++index1)
    {
      for (int index2 = 0; index2 < height; ++index2)
        ((AreaMapData) toMapData)[toX + index1, toY + index2] = this[fromX + index1, fromY + index2];
    }
  }

  public IMapData CloneArea(int fromX, int fromY, int width, int height)
  {
    AreaMapData toMapData = new AreaMapData();
    toMapData.InitializeData(width, height);
    this.CopyArea((IMapData) toMapData, 0, 0, fromX, fromY, width, height);
    return (IMapData) toMapData;
  }

  public AreaMapData() => this.areaList = new List<byte>();
}
