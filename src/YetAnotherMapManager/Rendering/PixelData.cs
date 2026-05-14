using System.Drawing;

#nullable disable
namespace YetAnotherMapManager.Rendering;

public struct PixelData
{
  public byte Blue;
  public byte Green;
  public byte Red;
  public byte Alpha;

  public PixelData(Color color)
  {
    this.Alpha = color.A;
    this.Red = color.R;
    this.Green = color.G;
    this.Blue = color.B;
  }
}
