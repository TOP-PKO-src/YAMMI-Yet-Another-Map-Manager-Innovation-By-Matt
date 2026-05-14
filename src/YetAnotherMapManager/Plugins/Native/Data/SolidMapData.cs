using System;
using YetAnotherMapManager.Common;
using YetAnotherMapManager.MapData;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Data;

[Serializable]
public class SolidMapData : IMapData
{
  private BitArray_2D solids;
  public static string KEY = "SOLIDS";

  public bool this[int x, int y, int subX, int subY]
  {
    get => this.solids[x * 2 + subX, y * 2 + subY];
    set => this.solids[x * 2 + subX, y * 2 + subY] = value;
  }

  public bool this[int i] => this.solids[i];

  public void InitializeData(int width, int height)
  {
    this.solids = new BitArray_2D(width * 2, height * 2, false);
  }

  public void InitializeData(MapInfo map) => this.InitializeData(map.Width, map.Height);

  public bool OverwriteData(BitArray_2D input)
  {
    if (input.Width != this.solids.Width * 2 || input.Height != this.solids.Height * 2)
      return false;
    this.solids = input;
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
        ((SolidMapData) toMapData)[toX + index1, toY + index2, 0, 0] = this[fromX + index1, fromY + index2, 0, 0];
        ((SolidMapData) toMapData)[toX + index1, toY + index2, 0, 1] = this[fromX + index1, fromY + index2, 0, 1];
        ((SolidMapData) toMapData)[toX + index1, toY + index2, 1, 0] = this[fromX + index1, fromY + index2, 1, 0];
        ((SolidMapData) toMapData)[toX + index1, toY + index2, 1, 1] = this[fromX + index1, fromY + index2, 1, 1];
      }
    }
  }

  public IMapData CloneArea(int fromX, int fromY, int width, int height)
  {
    SolidMapData toMapData = new SolidMapData();
    toMapData.InitializeData(width, height);
    this.CopyArea((IMapData) toMapData, 0, 0, fromX, fromY, width, height);
    return (IMapData) toMapData;
  }
}
