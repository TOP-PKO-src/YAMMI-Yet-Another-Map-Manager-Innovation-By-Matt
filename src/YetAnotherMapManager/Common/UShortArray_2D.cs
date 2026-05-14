using System;

#nullable disable
namespace YetAnotherMapManager.Common;

[Serializable]
public class UShortArray_2D
{
  private ushort[] data;
  private int width;
  private int height;

  public int Width => this.width;

  public int Height => this.height;

  public UShortArray_2D(int width, int height, ushort defaultValue)
  {
    this.width = width;
    this.height = height;
    int length = width * height;
    this.data = new ushort[length];
    for (int index = 0; index < length; ++index)
      this.data[index] = defaultValue;
  }

  public ushort this[int x, int y]
  {
    get => this.data[y * this.width + x];
    set => this.data[y * this.width + x] = value;
  }
}
