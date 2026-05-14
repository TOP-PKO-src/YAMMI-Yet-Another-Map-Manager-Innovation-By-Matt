using System;
using YetAnotherMapManager.MapData;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Data;

[Serializable]
public class TextureBlendingMapData : IMapData
{
  private uint[] dataMap;
  private int width;
  private int height;
  public static string KEY = "TEXTURE_BLENDING_MAP";

  public uint this[int x, int y]
  {
    set => this.dataMap[y * this.width + x] = value;
    get => this.dataMap[y * this.width + x];
  }

  public void InitializeData(int width, int height)
  {
    this.dataMap = new uint[width * height];
    this.width = width;
    this.height = height;
  }

  public void InitializeData(MapInfo map) => this.InitializeData(map.Width, map.Height);

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
        ((TextureBlendingMapData) toMapData)[toX + index1, toY + index2] = this[fromX + index1, fromY + index2];
    }
  }

  public IMapData CloneArea(int fromX, int fromY, int width, int height)
  {
    TextureBlendingMapData toMapData = new TextureBlendingMapData();
    toMapData.InitializeData(width, height);
    this.CopyArea((IMapData) toMapData, 0, 0, fromX, fromY, width, height);
    return (IMapData) toMapData;
  }
}
