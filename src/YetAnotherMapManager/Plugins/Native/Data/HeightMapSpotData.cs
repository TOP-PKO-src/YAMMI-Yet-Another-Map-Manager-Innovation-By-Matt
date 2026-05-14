using System;
using System.Collections.Generic;
using System.Drawing;
using YetAnotherMapManager.MapData;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Data;

[Serializable]
public class HeightMapSpotData : IMapData
{
  public Dictionary<Point, sbyte> HeightSpotList;
  public static string KEY = "HEIGHT_MAP_SPOT";

  public void InitializeData(MapInfo map)
  {
  }

  public void InitializeData(int width, int height)
  {
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
    HeightMapSpotData heightMapSpotData = (HeightMapSpotData) toMapData;
    foreach (Point key in this.HeightSpotList.Keys)
    {
      if (key.X >= fromX && key.X < fromX + width && key.Y >= fromY && key.Y < fromY + height)
        heightMapSpotData.HeightSpotList[new Point(toX + key.X - fromX, toY + key.Y - fromY)] = this.HeightSpotList[key];
    }
  }

  public IMapData CloneArea(int fromX, int fromY, int width, int height)
  {
    HeightMapSpotData toMapData = new HeightMapSpotData();
    toMapData.InitializeData(width, height);
    this.CopyArea((IMapData) toMapData, 0, 0, fromX, fromY, width, height);
    return (IMapData) toMapData;
  }

  public HeightMapSpotData() => this.HeightSpotList = new Dictionary<Point, sbyte>();
}
