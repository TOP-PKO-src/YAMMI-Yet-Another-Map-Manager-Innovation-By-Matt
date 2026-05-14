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

public class TextureDrawingTool : IDrawingTool
{
  public static string KEY = "TextureDrawer";
  private MapInfo map;
  private TextureDef texture;
  private List<string> RendererForcedUpdateList;

  public bool IsTileLevelTool() => true;

  public bool IsSubTileLevelTool() => false;

  public Bitmap GetIconPicture() => Resources.texture;

  public void DrawTileChanges(List<Point> list)
  {
    if (this.texture == null)
      return;
    TextureMapData customData = (TextureMapData) this.map.GetCustomData(TextureMapData.KEY);
    foreach (Point point in list)
      customData[point.X, point.Y] = (byte) this.texture.Id;
  }

  public void DrawSubTileChanges(List<SubTilePoint> list)
  {
  }

  public TextureDrawingTool(MapInfo map)
  {
    this.map = map;
    this.RendererForcedUpdateList = new List<string>(1);
    this.RendererForcedUpdateList.Add(TextureRenderer.KEY);
  }

  public void SetTexture(TextureDef textureDef) => this.texture = textureDef;

  public List<string> GetRendererForcedUpdateList() => this.RendererForcedUpdateList;
}
