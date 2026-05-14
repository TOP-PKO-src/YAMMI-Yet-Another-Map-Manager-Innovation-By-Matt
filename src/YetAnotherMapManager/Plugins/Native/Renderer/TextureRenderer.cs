using System.Collections.Generic;
using System.Drawing;
using YetAnotherMapManager.Core;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Renderer;

public class TextureRenderer : IRenderer
{
  public static string KEY = "Textures";
  private MapInfo map;
  private Dictionary<byte, PixelData> colorMap;
  private float alphaLevel = 1f;
  private bool displayed = true;

  public int GetScale() => 1;

  public void SetMap(MapInfo map) => this.map = map;

  public bool IsEnabled() => this.displayed;

  public void SetEnabled(bool status) => this.displayed = status;

  public float GetAlphaLevel() => this.alphaLevel;

  public void SetAlphaLevel(float value) => this.alphaLevel = value;

  public virtual Bitmap GetRenderedArea(int originX, int originY, int width, int height)
  {
    Bitmap bitmap = new Bitmap(width, height);
    if (this.map != null && this.colorMap != null)
    {
      TextureMapData customData = (TextureMapData) this.map.GetCustomData(TextureMapData.KEY);
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
              byte key = customData[x2, y2];
              if (key != (byte) 0 && this.colorMap.ContainsKey(key))
                fastBitmap.SetPixel(x1, y1, this.colorMap[key]);
            }
          }
        }
        fastBitmap.UnlockBitmap();
      }
    }
    return bitmap;
  }

  public virtual void SetColorMap(Settings settings)
  {
    this.colorMap = new Dictionary<byte, PixelData>(settings.Textures.Count);
    foreach (TextureDef texture in settings.Textures)
    {
      PixelData pixelData = new PixelData(texture.RenderColor);
      this.colorMap[(byte) texture.Id] = pixelData;
    }
  }
}
