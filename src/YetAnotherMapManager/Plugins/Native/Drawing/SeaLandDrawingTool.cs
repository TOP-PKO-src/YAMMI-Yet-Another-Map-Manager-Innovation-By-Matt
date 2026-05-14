using System.Collections.Generic;
using System.Drawing;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Plugins.Native.Renderer;
using YetAnotherMapManager.Properties;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Drawing;

public class SeaLandDrawingTool : IDrawingTool
{
  public static string KEY = "SeaLandDrawer";
  private MapInfo map;
  private byte mode = SeaLandMapData.TYPE_LAND;
  private List<string> RendererForcedUpdateList;

  public bool IsTileLevelTool() => true;

  public bool IsSubTileLevelTool() => false;

  public Bitmap GetIconPicture() => Resources.land;

  public void DrawTileChanges(List<Point> list)
  {
    SeaLandMapData customData = (SeaLandMapData) this.map.GetCustomData(SeaLandMapData.KEY);
    foreach (Point point in list)
      customData.SetTileType(point.X, point.Y, this.mode);
  }

  public void DrawSubTileChanges(List<SubTilePoint> list)
  {
  }

  public SeaLandDrawingTool(MapInfo map)
  {
    this.map = map;
    this.RendererForcedUpdateList = new List<string>(1);
    this.RendererForcedUpdateList.Add(SeaLandRenderer.KEY);
  }

  public void SetLand() => this.mode = SeaLandMapData.TYPE_LAND;

  public void SetSea() => this.mode = SeaLandMapData.TYPE_SEA;

  public void SetBridge() => this.mode = SeaLandMapData.TYPE_BRIDGE;

  public List<string> GetRendererForcedUpdateList() => this.RendererForcedUpdateList;
}
