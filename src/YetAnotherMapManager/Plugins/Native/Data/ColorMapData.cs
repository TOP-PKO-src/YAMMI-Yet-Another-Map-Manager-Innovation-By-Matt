using System;
using System.Drawing;
using YetAnotherMapManager.Common;
using YetAnotherMapManager.MapData;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Data;

[Serializable]
public class ColorMapData : IMapData
{
  private UShortArray_2D colorMap;
  public static string KEY = "COLOR_MAP";
  private float gRate1 = 4.047619f;
  private float gRate2 = 8.225806f;

  public ushort this[int x, int y]
  {
    set => this.colorMap[x, y] = value;
    get => this.colorMap[x, y];
  }

  public Color getColor(int x, int y)
  {
    ushort num = this[x, y];
    return Color.FromArgb((int) Math.Round((double) ((int) num & 31 /*0x1F*/) * (double) this.gRate2), (int) Math.Round((double) (((int) num & 2016) >> 5) * (double) this.gRate1), (int) Math.Round((double) (((int) num & 63488) >> 11) * (double) this.gRate2));
  }

  public void setColor(int x, int y, Color color)
  {
    ushort num = (ushort) ((uint) (ushort) ((uint) (ushort) (0U | (uint) (ushort) ((uint) (int) Math.Round((double) color.R / (double) this.gRate2) & 31U /*0x1F*/)) | (uint) (ushort) ((int) Math.Round((double) color.G / (double) this.gRate1) << 5 & 2016)) | (uint) (ushort) ((int) Math.Round((double) color.B / (double) this.gRate2) << 11 & 63488));
    this[x, y] = num;
  }

  public void InitializeData(int width, int height)
  {
    this.colorMap = new UShortArray_2D(width, height, ushort.MaxValue);
  }

  public void InitializeData(MapInfo map) => this.InitializeData(map.Width, map.Height);

  public bool OverwriteData(UShortArray_2D input)
  {
    if (input.Width != this.colorMap.Width || input.Height != this.colorMap.Height)
      return false;
    this.colorMap = input;
    return true;
  }

  public void CopyArea(
    IMapData toMapData,
    int toX,
    int toY,
    int fromX,
    int fromY,
    int width,
    int height)
  {
    for (int index1 = 0; index1 < width; ++index1)
    {
      for (int index2 = 0; index2 < height; ++index2)
        ((ColorMapData) toMapData)[toX + index1, toY + index2] = this[fromX + index1, fromY + index2];
    }
  }

  public IMapData CloneArea(int fromX, int fromY, int width, int height)
  {
    ColorMapData toMapData = new ColorMapData();
    toMapData.InitializeData(width, height);
    this.CopyArea((IMapData) toMapData, 0, 0, fromX, fromY, width, height);
    return (IMapData) toMapData;
  }
}
