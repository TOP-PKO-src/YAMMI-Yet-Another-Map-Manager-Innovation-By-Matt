using System;
using YetAnotherMapManager.Common;
using YetAnotherMapManager.MapData;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Data;

[Serializable]
public class SolidHeightMapData : IMapData
{
  private SByteArray_2D heightMap;
  public static string KEY = "SOLID_HEIGHT_MAP";

  public sbyte this[int x, int y, int subX, int subY]
  {
    get => this.heightMap[x * 2 + subX, y * 2 + subY];
    set => this.heightMap[x * 2 + subX, y * 2 + subY] = value;
  }

  public void InitializeData(int width, int height)
  {
    this.heightMap = new SByteArray_2D(width * 2, height * 2, (sbyte) 0);
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
        int height = (int) this.heightMap[x, y];
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
      {
        ((SolidHeightMapData) toMapData)[toX + index1, toY + index2, 0, 0] = this[fromX + index1, fromY + index2, 0, 0];
        ((SolidHeightMapData) toMapData)[toX + index1, toY + index2, 0, 1] = this[fromX + index1, fromY + index2, 0, 1];
        ((SolidHeightMapData) toMapData)[toX + index1, toY + index2, 1, 0] = this[fromX + index1, fromY + index2, 1, 0];
        ((SolidHeightMapData) toMapData)[toX + index1, toY + index2, 1, 1] = this[fromX + index1, fromY + index2, 1, 1];
      }
    }
  }

  public IMapData CloneArea(int fromX, int fromY, int width, int height)
  {
    SolidHeightMapData toMapData = new SolidHeightMapData();
    toMapData.InitializeData(width, height);
    this.CopyArea((IMapData) toMapData, 0, 0, fromX, fromY, width, height);
    return (IMapData) toMapData;
  }
}
