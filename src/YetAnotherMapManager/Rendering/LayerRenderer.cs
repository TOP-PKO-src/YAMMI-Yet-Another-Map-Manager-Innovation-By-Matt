using System;
using System.Drawing;
using YetAnotherMapManager.MapData;

#nullable disable
namespace YetAnotherMapManager.Rendering;

public abstract class LayerRenderer
{
  public static short TYPE_NONE = -1;
  public static short TYPE_TILE = 1;
  public static short TYPE_SUBDIVISION = 2;
  public static short TYPE_COORDINATES = 3;
  public static short TYPE_TEXTURED = 4;
  protected short layerType = -1;
  protected MapInfo map;
  protected float alphaLevel;

  public short LayerType => this.layerType;

  public MapInfo Map
  {
    set => this.map = value;
  }

  public float AlphaLevel
  {
    set
    {
      this.alphaLevel = Math.Min(value, 1f);
      this.alphaLevel = Math.Max(this.alphaLevel, 0.0f);
    }
    get => this.alphaLevel;
  }

  public abstract Color getColorForSubDivision(int tileX, int tileY, int subX, int subY);

  public abstract Color getColorForTile(int tileX, int tileY);
}
