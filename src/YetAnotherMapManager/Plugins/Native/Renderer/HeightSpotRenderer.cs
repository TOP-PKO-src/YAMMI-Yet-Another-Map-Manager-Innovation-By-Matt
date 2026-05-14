using System.Collections.Generic;
using System.Drawing;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Renderer;

public class HeightSpotRenderer : IRenderer
{
  public static string KEY = "Height Spot Map";
  private MapInfo map;
  private Dictionary<sbyte, PixelData> colorMap;
  private float alphaLevel = 1f;
  private bool displayed = true;

  public int GetScale() => 1;

  public void SetMap(MapInfo map)
  {
    this.map = map;
    this.colorMap = (Dictionary<sbyte, PixelData>) null;
  }

  public bool IsEnabled() => this.displayed;

  public void SetEnabled(bool status) => this.displayed = status;

  public float GetAlphaLevel() => this.alphaLevel;

  public void SetAlphaLevel(float value) => this.alphaLevel = value;

  public virtual Bitmap GetRenderedArea(int originX, int originY, int width, int height)
  {
    Bitmap bitmap = new Bitmap(width, height);
    if (this.map != null)
    {
      if (this.colorMap == null)
        this.ComputeColorMap_ModeB();
      HeightMapSpotData customData = (HeightMapSpotData) this.map.GetCustomData(HeightMapSpotData.KEY);
      if (customData != null)
      {
        FastBitmap fastBitmap = new FastBitmap(bitmap);
        fastBitmap.LockBitmap();
        foreach (Point key in customData.HeightSpotList.Keys)
        {
          int x = key.X - originX;
          int y = key.Y - originY;
          if (x >= 0 && x <= width && y >= 0 && y <= height)
            fastBitmap.SetPixel(x, y, this.colorMap[customData.HeightSpotList[key]]);
        }
        fastBitmap.UnlockBitmap();
      }
    }
    return bitmap;
  }

  private void ComputeColorMap_ModeB()
  {
    this.colorMap = new Dictionary<sbyte, PixelData>(256 /*0x0100*/);
    int num1 = 139;
    int num2 = 218;
    int maxValue1 = (int) byte.MaxValue;
    int num3 = 0;
    int num4 = 39;
    int num5 = 208 /*0xD0*/;
    int num6 = num3 - num1;
    int num7 = num4 - num2;
    int num8 = num5 - maxValue1;
    for (sbyte minValue = sbyte.MinValue; minValue <= (sbyte) 0; ++minValue)
    {
      float num9 = (float) minValue / (float) sbyte.MinValue;
      PixelData pixelData = new PixelData(Color.FromArgb((int) ((double) num1 + (double) num9 * (double) num6), (int) ((double) num2 + (double) num9 * (double) num7), (int) ((double) maxValue1 + (double) num9 * (double) num8)));
      this.colorMap[minValue] = pixelData;
    }
    int num10 = 231;
    int num11 = 185;
    int num12 = 149;
    int num13 = 104;
    int num14 = 49;
    int num15 = 4;
    int num16 = num13 - num10;
    int num17 = num14 - num11;
    int num18 = num15 - num12;
    int num19 = 1;
    for (sbyte index = 0; index <= (sbyte) 30; ++index)
    {
      float num20 = (float) index / 30f;
      PixelData pixelData = new PixelData(Color.FromArgb((int) ((double) num10 + (double) num20 * (double) num16), (int) ((double) num11 + (double) num20 * (double) num17), (int) ((double) num12 + (double) num20 * (double) num18)));
      this.colorMap[(sbyte) ((int) index + num19)] = pixelData;
    }
    int num21 = 195;
    int num22 = 195;
    int num23 = 195;
    int maxValue2 = (int) byte.MaxValue;
    int maxValue3 = (int) byte.MaxValue;
    int maxValue4 = (int) byte.MaxValue;
    int num24 = maxValue2 - num21;
    int num25 = maxValue3 - num22;
    int num26 = maxValue4 - num23;
    int num27 = 32 /*0x20*/;
    for (sbyte index = 0; index <= (sbyte) 95; ++index)
    {
      float num28 = (float) index / 95f;
      PixelData pixelData = new PixelData(Color.FromArgb((int) ((double) num21 + (double) num28 * (double) num24), (int) ((double) num22 + (double) num28 * (double) num25), (int) ((double) num23 + (double) num28 * (double) num26)));
      this.colorMap[(sbyte) (num27 + (int) index)] = pixelData;
    }
  }
}
