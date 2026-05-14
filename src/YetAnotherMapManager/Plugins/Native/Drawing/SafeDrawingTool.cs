using System.Collections.Generic;
using System.Drawing;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Plugins.Native.Renderer;
using YetAnotherMapManager.Properties;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Drawing;

public class SafeDrawingTool : IDrawingTool
{
  public static string KEY = "SafeDrawer";
  private static bool MODE_SAFE = true;
  private static bool MODE_UNSAFE = false;
  private MapInfo map;
  private bool mode = SafeDrawingTool.MODE_SAFE;
  private List<string> RendererForcedUpdateList;

  public bool IsTileLevelTool() => true;

  public bool IsSubTileLevelTool() => false;

  public Bitmap GetIconPicture() => Resources.safe;

  public void DrawTileChanges(List<Point> list)
  {
    SafeZoneMapData customData = (SafeZoneMapData) this.map.GetCustomData(SafeZoneMapData.KEY);
    foreach (Point point in list)
      customData[point.X, point.Y] = this.mode;
  }

  public void DrawSubTileChanges(List<SubTilePoint> list)
  {
  }

  public SafeDrawingTool(MapInfo map)
  {
    this.map = map;
    this.RendererForcedUpdateList = new List<string>(1);
    this.RendererForcedUpdateList.Add(SafeZoneRenderer.KEY);
  }

  public void SetSafe() => this.mode = SafeDrawingTool.MODE_SAFE;

  public void SetUnsafe() => this.mode = SafeDrawingTool.MODE_UNSAFE;

  public List<string> GetRendererForcedUpdateList() => this.RendererForcedUpdateList;
}
