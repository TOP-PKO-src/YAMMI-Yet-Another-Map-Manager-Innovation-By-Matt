using System;
using System.Collections;

#nullable disable
namespace YetAnotherMapManager.Common;

[Serializable]
public class TwoBitArray_2D
{
  private BitArray data;
  private int width;
  private int height;

  public int Width => this.width;

  public int Height => this.height;

  public TwoBitArray_2D(int width, int height)
  {
    this.width = width;
    this.height = height;
    this.data = new BitArray(2 * width * height);
  }

  public byte this[int x, int y]
  {
    get
    {
      byte num = 0;
      if (this.data[2 * (y * this.width + x)])
        num |= (byte) 1;
      else if (this.data[2 * (y * this.width + x) + 1])
        num |= (byte) 2;
      return num;
    }
    set
    {
      this.data[2 * (y * this.width + x)] = ((int) value & 1) == 1;
      this.data[2 * (y * this.width + x) + 1] = ((int) value & 2) == 2;
    }
  }
}
