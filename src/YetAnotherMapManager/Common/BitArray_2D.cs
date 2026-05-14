using System;
using System.Collections;

#nullable disable
namespace YetAnotherMapManager.Common;

[Serializable]
public class BitArray_2D
{
  private BitArray data;
  private int width;
  private int height;

  public int Width => this.width;

  public int Height => this.height;

  public BitArray_2D(int width, int height, bool value)
  {
    this.width = width;
    this.height = height;
    this.data = new BitArray(width * height, value);
  }

  public bool this[int x, int y]
  {
    get => this.data[y * this.width + x];
    set => this.data[y * this.width + x] = value;
  }

  public bool this[int i] => this.data[i];
}
