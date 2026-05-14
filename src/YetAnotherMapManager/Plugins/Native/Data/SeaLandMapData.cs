using System;
using YetAnotherMapManager.Common;
using YetAnotherMapManager.MapData;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Data;

[Serializable]
public class SeaLandMapData : IMapData
{
  private TwoBitArray_2D seaLand;
  public static string KEY = "SEA/LAND";
  public static byte TYPE_SEA = 0;
  public static byte TYPE_LAND = 1;
  public static byte TYPE_BRIDGE = 2;

  public byte this[int x, int y] => this.seaLand[x, y];

  public byte GetTileType(int x, int y) => this.seaLand[x, y];

  public void SetTileType(int x, int y, byte type) => this.seaLand[x, y] = type;

  public void InitializeData(int width, int height)
  {
    this.seaLand = new TwoBitArray_2D(width, height);
  }

  public void InitializeData(MapInfo map) => this.InitializeData(map.Width, map.Height);

  public bool OverwriteData(TwoBitArray_2D input)
  {
    if (input.Width != this.seaLand.Width || input.Height != this.seaLand.Height)
      return false;
    this.seaLand = input;
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
    SeaLandMapData seaLandMapData = (SeaLandMapData) toMapData;
    for (int index1 = 0; index1 < width; ++index1)
    {
      for (int index2 = 0; index2 < height; ++index2)
        seaLandMapData.SetTileType(toX + index1, toY + index2, this.GetTileType(fromX + index1, fromY + index2));
    }
  }

  public IMapData CloneArea(int fromX, int fromY, int width, int height)
  {
    SeaLandMapData toMapData = new SeaLandMapData();
    toMapData.InitializeData(width, height);
    this.CopyArea((IMapData) toMapData, 0, 0, fromX, fromY, width, height);
    return (IMapData) toMapData;
  }
}
