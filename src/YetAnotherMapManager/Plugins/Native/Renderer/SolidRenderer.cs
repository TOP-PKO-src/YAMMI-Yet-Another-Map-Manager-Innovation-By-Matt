using System.Drawing;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Renderer;

public class SolidRenderer : IRenderer
{
  public static string KEY = "Solids";
  private MapInfo map;
  private PixelData solidColor;
  private float alphaLevel = 1f;
  private bool displayed = true;

  public SolidRenderer() => this.solidColor = new PixelData(Color.Red);

  public void NotifyChangesOn(string key)
  {
  }

  public int GetScale() => 2;

  public void SetMap(MapInfo map) => this.map = map;

  public bool IsEnabled() => this.displayed;

  public void SetEnabled(bool status) => this.displayed = status;

  public float GetAlphaLevel() => this.alphaLevel;

  public void SetAlphaLevel(float value) => this.alphaLevel = value;

  public virtual Bitmap GetRenderedArea(int originX, int originY, int width, int height)
  {
    Bitmap bitmap = new Bitmap(width * 2, height * 2);
    if (this.map != null)
    {
      SolidMapData customData = (SolidMapData) this.map.GetCustomData(SolidMapData.KEY);
      if (customData != null)
      {
        FastBitmap fastBitmap = new FastBitmap(bitmap);
        fastBitmap.LockBitmap();
        for (int index1 = 0; index1 < width; ++index1)
        {
          for (int index2 = 0; index2 < height; ++index2)
          {
            int x = originX + index1;
            int y = originY + index2;
            if (x < this.map.Width && y < this.map.Height)
            {
              if (customData[x, y, 0, 0])
                fastBitmap.SetPixel(2 * index1, 2 * index2, this.solidColor);
              if (customData[x, y, 1, 0])
                fastBitmap.SetPixel(2 * index1 + 1, 2 * index2, this.solidColor);
              if (customData[x, y, 0, 1])
                fastBitmap.SetPixel(2 * index1, 2 * index2 + 1, this.solidColor);
              if (customData[x, y, 1, 1])
                fastBitmap.SetPixel(2 * index1 + 1, 2 * index2 + 1, this.solidColor);
            }
          }
        }
        fastBitmap.UnlockBitmap();
      }
    }
    return bitmap;
  }
}
