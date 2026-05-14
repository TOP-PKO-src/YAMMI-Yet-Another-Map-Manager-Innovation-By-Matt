using System.Collections.Generic;
using System.Drawing;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Renderer;

public class SeaLandRenderer : IRenderer
{
  public static string KEY = "Sea / Land";
  private MapInfo map;
  private Dictionary<short, PixelData> typeColorMap;
  private float alphaLevel = 1f;
  private bool displayed = true;

  public SeaLandRenderer()
  {
    this.typeColorMap = new Dictionary<short, PixelData>(3);
    this.typeColorMap.Add((short) SeaLandMapData.TYPE_BRIDGE, new PixelData(Color.DarkKhaki));
    this.typeColorMap.Add((short) SeaLandMapData.TYPE_LAND, new PixelData(Color.Peru));
    this.typeColorMap.Add((short) SeaLandMapData.TYPE_SEA, new PixelData(Color.DarkBlue));
  }

  public void NotifyChangesOn(string key)
  {
  }

  public int GetScale() => 1;

  public void SetMap(MapInfo map) => this.map = map;

  public bool IsEnabled() => this.displayed;

  public void SetEnabled(bool status) => this.displayed = status;

  public float GetAlphaLevel() => this.alphaLevel;

  public void SetAlphaLevel(float value) => this.alphaLevel = value;

  public virtual Bitmap GetRenderedArea(int originX, int originY, int width, int height)
  {
    Bitmap bitmap = new Bitmap(width, height);
    if (this.map != null)
    {
      SeaLandMapData customData = (SeaLandMapData) this.map.GetCustomData(SeaLandMapData.KEY);
      if (customData != null)
      {
        FastBitmap fastBitmap = new FastBitmap(bitmap);
        fastBitmap.LockBitmap();
        for (int x1 = 0; x1 < width; ++x1)
        {
          for (int y1 = 0; y1 < height; ++y1)
          {
            int x2 = originX + x1;
            int y2 = originY + y1;
            if (x2 < this.map.Width && y2 < this.map.Height)
            {
              short tileType = (short) customData.GetTileType(x2, y2);
              fastBitmap.SetPixel(x1, y1, this.typeColorMap[tileType]);
            }
          }
        }
        fastBitmap.UnlockBitmap();
      }
    }
    return bitmap;
  }
}
