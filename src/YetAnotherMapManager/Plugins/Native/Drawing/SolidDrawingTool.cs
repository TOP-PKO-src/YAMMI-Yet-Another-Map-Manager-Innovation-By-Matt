using System.Collections.Generic;
using System.Drawing;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Plugins.Native.Renderer;
using YetAnotherMapManager.Properties;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Drawing;

public class SolidDrawingTool : IDrawingTool
{
  public static string KEY = "SolidDrawer";
  private static bool MODE_SOLID = true;
  private static bool MODE_NON_SOLID = false;
  private MapInfo map;
  private bool mode = SolidDrawingTool.MODE_SOLID;
  private List<string> RendererForcedUpdateList;

  public bool IsTileLevelTool() => true;

  public bool IsSubTileLevelTool() => false;

  public Bitmap GetIconPicture() => Resources.blocked1;

  public void DrawTileChanges(List<Point> list)
  {
    SolidMapData customData = (SolidMapData) this.map.GetCustomData(SolidMapData.KEY);
    foreach (Point point in list)
    {
      customData[point.X, point.Y, 0, 0] = this.mode;
      customData[point.X, point.Y, 1, 0] = this.mode;
      customData[point.X, point.Y, 0, 1] = this.mode;
      customData[point.X, point.Y, 1, 1] = this.mode;
    }
  }

  public void DrawSubTileChanges(List<SubTilePoint> list)
  {
    SolidMapData customData = (SolidMapData) this.map.GetCustomData(SolidMapData.KEY);
    foreach (SubTilePoint subTilePoint in list)
      customData[subTilePoint.X, subTilePoint.Y, subTilePoint.SubX, subTilePoint.SubY] = this.mode;
  }

  public SolidDrawingTool(MapInfo map)
  {
    this.map = map;
    this.RendererForcedUpdateList = new List<string>(1);
    this.RendererForcedUpdateList.Add(SolidRenderer.KEY);
  }

  public void SetSolid() => this.mode = SolidDrawingTool.MODE_SOLID;

  public void SetNonSolid() => this.mode = SolidDrawingTool.MODE_NON_SOLID;

  public List<string> GetRendererForcedUpdateList() => this.RendererForcedUpdateList;
}
