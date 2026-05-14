using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using YetAnotherMapManager.Core;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Renderer;

public class AreaRenderer : IRenderer
{
  public static string KEY = "Named Areas";
  private MapInfo map;
  private Dictionary<byte, PixelData> colorMap;
  private float alphaLevel = 1f;
  private bool displayed = true;

  public int GetScale() => 1;

  public void SetMap(MapInfo map)
  {
    this.map = map;
    this.colorMap = (Dictionary<byte, PixelData>) null;
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
        this.ComputeColorMap();
      AreaMapData customData = (AreaMapData) this.map.GetCustomData(AreaMapData.KEY);
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
              if (key != (byte) 0)
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
    this.colorMap = new Dictionary<byte, PixelData>(settings.Areas.Count);
    foreach (AreaDef area in settings.Areas)
    {
      PixelData pixelData = new PixelData(area.RenderColor);
      this.colorMap[(byte) area.Id] = pixelData;
    }
  }

  private void ComputeColorMap()
  {
    AreaMapData customData = (AreaMapData) this.map.GetCustomData(AreaMapData.KEY);
    List<Color> colorList = new List<Color>();
    Type type = Color.White.GetType();
    if ((Type) null != type)
    {
      PropertyInfo[] properties = type.GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public);
      int length = properties.Length;
      for (int index = 0; index < length; ++index)
      {
        Color color = (Color) properties[index].GetValue((object) null, (object[]) null);
        if (color != Color.Transparent && color != Color.Red)
          colorList.Add(color);
      }
    }
    this.colorMap = new Dictionary<byte, PixelData>(customData.AreaList.Count + 1);
    int num = 0;
    foreach (byte area in customData.AreaList)
      this.colorMap[area] = new PixelData(colorList[num++]);
  }
}
