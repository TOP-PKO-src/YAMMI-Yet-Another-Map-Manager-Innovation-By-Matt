using System;
using YetAnotherMapManager.Common;
using YetAnotherMapManager.MapData;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Data;

[Serializable]
public class PvpMapData : IMapData
{
  private BitArray_2D pvpFlags;
  public static string KEY = "PVP_FLAGS";

  public bool this[int x, int y]
  {
    get => this.pvpFlags[x, y];
    set => this.pvpFlags[x, y] = value;
  }

  public void InitializeData(int width, int height)
  {
    this.pvpFlags = new BitArray_2D(width, height, false);
  }

  public void InitializeData(MapInfo map) => this.InitializeData(map.Width, map.Height);

  public bool OverwriteData(BitArray_2D input)
  {
    if (input.Width != this.pvpFlags.Width || input.Height != this.pvpFlags.Height)
      return false;
    this.pvpFlags = input;
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
        ((PvpMapData) toMapData)[toX + index1, toY + index2] = this[fromX + index1, fromY + index2];
    }
  }

  public IMapData CloneArea(int fromX, int fromY, int width, int height)
  {
    PvpMapData toMapData = new PvpMapData();
    toMapData.InitializeData(width, height);
    this.CopyArea((IMapData) toMapData, 0, 0, fromX, fromY, width, height);
    return (IMapData) toMapData;
  }
}
