using System.Collections.Generic;
using System.Drawing;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Plugins.Native.Renderer;
using YetAnotherMapManager.Properties;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Drawing;

public class HeightSpotDrawingTool : IDrawingTool
{
  public static string KEY = "HeightSpotDrawer";
  public static sbyte MIN_HEIGHT = sbyte.MinValue;
  public static sbyte MAX_HEIGHT = sbyte.MaxValue;
  private MapInfo map;
  private sbyte height;
  private List<string> RendererForcedUpdateList;

  public bool IsTileLevelTool() => true;

  public bool IsSubTileLevelTool() => false;

  public Bitmap GetIconPicture() => Resources.heigh;

  public void DrawTileChanges(List<Point> list)
  {
    HeightMapSpotData data = (HeightMapSpotData) this.map.GetCustomData(HeightMapSpotData.KEY);
    if (data == null)
    {
      data = new HeightMapSpotData();
      this.map.AddCustomData(HeightMapSpotData.KEY, (IMapData) data);
    }
    foreach (Point key in list)
      data.HeightSpotList[key] = this.height;
  }

  public void DrawSubTileChanges(List<SubTilePoint> list)
  {
  }

  public HeightSpotDrawingTool(MapInfo map)
  {
    this.map = map;
    this.RendererForcedUpdateList = new List<string>(1);
    this.RendererForcedUpdateList.Add(HeightSpotRenderer.KEY);
  }

  public void SetHeight(sbyte height)
  {
    if ((int) height < (int) HeightSpotDrawingTool.MIN_HEIGHT)
      height = HeightSpotDrawingTool.MIN_HEIGHT;
    if ((int) height > (int) HeightSpotDrawingTool.MAX_HEIGHT)
      height = HeightSpotDrawingTool.MAX_HEIGHT;
    this.height = height;
  }

  public List<string> GetRendererForcedUpdateList() => this.RendererForcedUpdateList;
}
