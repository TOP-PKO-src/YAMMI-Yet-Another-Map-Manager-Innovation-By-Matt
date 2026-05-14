using System.Collections.Generic;
using System.Drawing;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Plugins.Native.Renderer;
using YetAnotherMapManager.Properties;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Drawing;

public class PvpDrawingTool : IDrawingTool
{
  public static string KEY = "PvpDrawer";
  private static bool MODE_PVP = true;
  private static bool MODE_NON_PVP = false;
  private MapInfo map;
  private bool mode = PvpDrawingTool.MODE_PVP;
  private List<string> RendererForcedUpdateList;

  public bool IsTileLevelTool() => true;

  public bool IsSubTileLevelTool() => false;

  public Bitmap GetIconPicture() => Resources.pvp;

  public void DrawTileChanges(List<Point> list)
  {
    PvpMapData customData = (PvpMapData) this.map.GetCustomData(PvpMapData.KEY);
    foreach (Point point in list)
      customData[point.X, point.Y] = this.mode;
  }

  public void DrawSubTileChanges(List<SubTilePoint> list)
  {
  }

  public PvpDrawingTool(MapInfo map)
  {
    this.map = map;
    this.RendererForcedUpdateList = new List<string>(1);
    this.RendererForcedUpdateList.Add(PvpRenderer.KEY);
  }

  public void SetPvp() => this.mode = PvpDrawingTool.MODE_PVP;

  public void SetNonPvp() => this.mode = PvpDrawingTool.MODE_NON_PVP;

  public List<string> GetRendererForcedUpdateList() => this.RendererForcedUpdateList;
}
