using System;

#nullable disable
namespace YetAnotherMapManager.Common;

[Serializable]
public class ByteArray_2D
{
  private byte[] data;
  private int width;
  private int height;

  public int Width => this.width;

  public int Height => this.height;

  public ByteArray_2D(int width, int height, byte defaultValue)
  {
    this.width = width;
    this.height = height;
    int length = width * height;
    this.data = new byte[length];
    for (int index = 0; index < length; ++index)
      this.data[index] = defaultValue;
  }

  public byte this[int x, int y]
  {
    get => this.data[y * this.width + x];
    set => this.data[y * this.width + x] = value;
  }
}
