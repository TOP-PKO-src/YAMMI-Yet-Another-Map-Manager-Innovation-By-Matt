using System.Collections.Generic;
using System.Drawing;
using YetAnotherMapManager.Core;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Plugins.Native.Renderer;
using YetAnotherMapManager.Properties;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Drawing;

public class AreaDrawingTool : IDrawingTool
{
  public static string KEY = "AreaDrawer";
  private MapInfo map;
  private AreaDef area;
  private List<string> RendererForcedUpdateList;

  public bool IsTileLevelTool() => true;

  public bool IsSubTileLevelTool() => false;

  public Bitmap GetIconPicture() => Resources.area;

  public void DrawTileChanges(List<Point> list)
  {
    if (this.area == null)
      return;
    AreaMapData customData = (AreaMapData) this.map.GetCustomData(AreaMapData.KEY);
    foreach (Point point in list)
      customData[point.X, point.Y] = (byte) this.area.Id;
  }

  public void DrawSubTileChanges(List<SubTilePoint> list)
  {
  }

  public AreaDrawingTool(MapInfo map)
  {
    this.map = map;
    this.RendererForcedUpdateList = new List<string>(1);
    this.RendererForcedUpdateList.Add(AreaRenderer.KEY);
  }

  public void SetArea(AreaDef areaDef) => this.area = areaDef;

  public List<string> GetRendererForcedUpdateList() => this.RendererForcedUpdateList;
}
