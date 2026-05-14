using System.Drawing;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Renderer;

public class SafeZoneRenderer : IRenderer
{
  public static string KEY = "Safe Zones";
  private MapInfo map;
  private PixelData solidColor;
  private float alphaLevel = 1f;
  private bool displayed = true;

  public SafeZoneRenderer() => this.solidColor = new PixelData(Color.Green);

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
      SafeZoneMapData customData = (SafeZoneMapData) this.map.GetCustomData(SafeZoneMapData.KEY);
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
            if (x2 < this.map.Width && y2 < this.map.Height && customData[x2, y2])
              fastBitmap.SetPixel(x1, y1, this.solidColor);
          }
        }
        fastBitmap.UnlockBitmap();
      }
    }
    return bitmap;
  }
}
