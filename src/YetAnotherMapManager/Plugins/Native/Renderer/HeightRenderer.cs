using System.Collections.Generic;
using System.Drawing;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Renderer;

public class HeightRenderer : IRenderer
{
  public static string KEY = "Height Map";
  private Color lowBaseColor = Color.White;
  private Color hightBaseColor = Color.Black;
  private Color waterColor = Color.DarkBlue;
  private MapInfo map;
  private Dictionary<int, PixelData> colorMap;
  private PixelData offshoreColor;
  private float alphaLevel = 1f;
  private bool displayed = true;

  public HeightRenderer() => this.offshoreColor = new PixelData(this.waterColor);

  public int GetScale() => 1;

  public void SetMap(MapInfo map)
  {
    this.map = map;
    this.colorMap = (Dictionary<int, PixelData>) null;
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
      SeaLandMapData customData1 = (SeaLandMapData) this.map.GetCustomData(SeaLandMapData.KEY);
      HeightMapData customData2 = (HeightMapData) this.map.GetCustomData(HeightMapData.KEY);
      if (customData1 != null && customData2 != null)
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
              sbyte key = customData2[x2, y2];
              if (key == (sbyte) 0 && (int) customData1[x2, y2] == (int) SeaLandMapData.TYPE_SEA)
              {
                fastBitmap.SetPixel(x1, y1, this.offshoreColor);
              }
              else
              {
                fastBitmap.SetPixel(x1, y1, this.colorMap[(int) key]);
                if (key < (sbyte) 0)
                  fastBitmap.SetPixel(x1, y1, this.offshoreColor, 0.6f);
              }
            }
          }
        }
        fastBitmap.UnlockBitmap();
      }
    }
    return bitmap;
  }

  private void ComputeColorMap()
  {
    HeightMapData customData = (HeightMapData) this.map.GetCustomData(HeightMapData.KEY);
    int num1 = (int) customData.MaxHeight - (int) customData.MinHeight;
    this.colorMap = new Dictionary<int, PixelData>(num1 + 1);
    if (num1 != 0)
    {
      int num2 = ((int) this.hightBaseColor.R - (int) this.lowBaseColor.R) / num1;
      int num3 = ((int) this.hightBaseColor.G - (int) this.lowBaseColor.G) / num1;
      int num4 = ((int) this.hightBaseColor.B - (int) this.lowBaseColor.B) / num1;
      for (int index = 0; index <= num1; ++index)
      {
        PixelData pixelData = new PixelData(Color.FromArgb((int) this.lowBaseColor.R + num2 * index, (int) this.lowBaseColor.G + num2 * index, (int) this.lowBaseColor.B + num2 * index));
        this.colorMap[(int) customData.MinHeight + index] = pixelData;
      }
    }
    else
      this.colorMap[(int) customData.MinHeight] = new PixelData(this.hightBaseColor);
  }

  private void ComputeColorMap_ModeB()
  {
    this.colorMap = new Dictionary<int, PixelData>(256 /*0x0100*/);
    int num1 = 139;
    int num2 = 218;
    int maxValue1 = (int) byte.MaxValue;
    int num3 = 0;
    int num4 = 39;
    int num5 = 208 /*0xD0*/;
    int num6 = num3 - num1;
    int num7 = num4 - num2;
    int num8 = num5 - maxValue1;
    for (int minValue = (int) sbyte.MinValue; minValue <= 0; ++minValue)
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
    for (int index = 0; index <= 30; ++index)
    {
      float num20 = (float) index / 30f;
      PixelData pixelData = new PixelData(Color.FromArgb((int) ((double) num10 + (double) num20 * (double) num16), (int) ((double) num11 + (double) num20 * (double) num17), (int) ((double) num12 + (double) num20 * (double) num18)));
      this.colorMap[index + num19] = pixelData;
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
    for (int index = 0; index <= 95; ++index)
    {
      float num28 = (float) index / 95f;
      PixelData pixelData = new PixelData(Color.FromArgb((int) ((double) num21 + (double) num28 * (double) num24), (int) ((double) num22 + (double) num28 * (double) num25), (int) ((double) num23 + (double) num28 * (double) num26)));
      this.colorMap[num27 + index] = pixelData;
    }
  }
}
