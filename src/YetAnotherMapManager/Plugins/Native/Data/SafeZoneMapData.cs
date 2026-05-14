using System;
using YetAnotherMapManager.Common;
using YetAnotherMapManager.MapData;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Data;

[Serializable]
public class SafeZoneMapData : IMapData
{
  private BitArray_2D pkFlags;
  public static string KEY = "SAFE";

  public bool this[int x, int y]
  {
    get => this.pkFlags[x, y];
    set => this.pkFlags[x, y] = value;
  }

  public void InitializeData(int width, int height)
  {
    this.pkFlags = new BitArray_2D(width, height, false);
  }

  public void InitializeData(MapInfo map) => this.InitializeData(map.Width, map.Height);

  public bool OverwriteData(BitArray_2D input)
  {
    if (input.Width != this.pkFlags.Width || input.Height != this.pkFlags.Height)
      return false;
    this.pkFlags = input;
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
        ((SafeZoneMapData) toMapData)[toX + index1, toY + index2] = this[fromX + index1, fromY + index2];
    }
  }

  public IMapData CloneArea(int fromX, int fromY, int width, int height)
  {
    SafeZoneMapData toMapData = new SafeZoneMapData();
    toMapData.InitializeData(width, height);
    this.CopyArea((IMapData) toMapData, 0, 0, fromX, fromY, width, height);
    return (IMapData) toMapData;
  }
}
