using System.Collections.Generic;
using System.Drawing;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Plugins.Native.Renderer;
using YetAnotherMapManager.Properties;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Drawing;

public class LightColorDrawingTool : IDrawingTool
{
  public static string KEY = "LightColorDrawer";
  private MapInfo map;
  private Color color;
  private List<string> RendererForcedUpdateList;

  public bool IsTileLevelTool() => true;

  public bool IsSubTileLevelTool() => false;

  public Bitmap GetIconPicture() => Resources.texture;

  public void DrawTileChanges(List<Point> list)
  {
    ColorMapData customData = (ColorMapData) this.map.GetCustomData(ColorMapData.KEY);
    foreach (Point point in list)
      customData.setColor(point.X, point.Y, this.color);
  }

  public void DrawSubTileChanges(List<SubTilePoint> list)
  {
  }

  public LightColorDrawingTool(MapInfo map)
  {
    this.map = map;
    this.RendererForcedUpdateList = new List<string>(1);
    this.RendererForcedUpdateList.Add(ColorRenderer.KEY);
  }

  public void SetColor(Color color) => this.color = color;

  public List<string> GetRendererForcedUpdateList() => this.RendererForcedUpdateList;
}
