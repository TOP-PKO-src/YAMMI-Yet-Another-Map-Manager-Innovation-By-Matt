using System;
using YetAnotherMapManager.Common;
using YetAnotherMapManager.MapData;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Data;

[Serializable]
public class HeightMapData : IMapData
{
  private SByteArray_2D heightMap;
  private sbyte minHeight;
  private sbyte maxHeight;
  public static string KEY = "HEIGHT_MAP";

  public sbyte MinHeight => this.minHeight;

  public sbyte MaxHeight => this.maxHeight;

  public sbyte this[int x, int y]
  {
    set
    {
      if ((int) value > (int) this.maxHeight)
        this.maxHeight = value;
      if ((int) value < (int) this.minHeight)
        this.minHeight = value;
      this.heightMap[x, y] = value;
    }
    get => this.heightMap[x, y];
  }

  public void InitializeData(int width, int height)
  {
    this.heightMap = new SByteArray_2D(width, height, (sbyte) -100);
  }

  public void InitializeData(MapInfo map) => this.InitializeData(map.Width, map.Height);

  public bool OverwriteData(SByteArray_2D input)
  {
    if (input.Width != this.heightMap.Width || input.Height != this.heightMap.Height)
      return false;
    this.heightMap = input;
    for (int x = 0; x < this.heightMap.Width; ++x)
    {
      for (int y = 0; y < this.heightMap.Height; ++y)
      {
        sbyte height = this.heightMap[x, y];
        if ((int) height > (int) this.maxHeight)
          this.maxHeight = height;
        if ((int) height < (int) this.minHeight)
          this.minHeight = height;
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
        ((HeightMapData) toMapData)[toX + index1, toY + index2] = this[fromX + index1, fromY + index2];
    }
  }

  public IMapData CloneArea(int fromX, int fromY, int width, int height)
  {
    HeightMapData toMapData = new HeightMapData();
    toMapData.InitializeData(width, height);
    this.CopyArea((IMapData) toMapData, 0, 0, fromX, fromY, width, height);
    return (IMapData) toMapData;
  }
}
