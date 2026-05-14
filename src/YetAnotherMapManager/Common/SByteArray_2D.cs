using System;

#nullable disable
namespace YetAnotherMapManager.Common;

[Serializable]
public class SByteArray_2D
{
  private sbyte[] data;
  private int width;
  private int height;

  public int Width => this.width;

  public int Height => this.height;

  public SByteArray_2D(int width, int height, sbyte defaultValue)
  {
    this.width = width;
    this.height = height;
    int length = width * height;
    this.data = new sbyte[length];
    for (int index = 0; index < length; ++index)
      this.data[index] = defaultValue;
  }

  public sbyte this[int x, int y]
  {
    get => this.data[y * this.width + x];
    set => this.data[y * this.width + x] = value;
  }

  public SByteArray_2D applyConvolution(float[,] matrix)
  {
    SByteArray_2D sbyteArray2D = new SByteArray_2D(this.width, this.height, (sbyte) 0);
    int length1 = matrix.GetLength(0);
    int length2 = matrix.GetLength(1);
    int num1 = length1 / 2;
    int num2 = length2 / 2;
    if (length1 % 2 != 1 || length2 % 2 != 1)
      throw new Exception("Wrong input matrix");
    for (int x1 = 0; x1 < this.width; ++x1)
    {
      for (int y1 = 0; y1 < this.height; ++y1)
      {
        float num3 = 0.0f;
        float num4 = 0.0f;
        for (int index1 = 0; index1 < length1; ++index1)
        {
          for (int index2 = 0; index2 < length2; ++index2)
          {
            int x2 = x1 + (index1 - num1);
            int y2 = y1 + (index2 - num2);
            if (x2 >= 0 && x2 < this.width && y2 >= 0 && y2 < this.height)
            {
              num3 += matrix[index1, index2] * (float) this[x2, y2];
              ++num4;
            }
          }
        }
        sbyteArray2D[x1, y1] = (double) num4 == 0.0 ? this[x1, y1] : (sbyte) ((double) num3 / (double) num4);
      }
    }
    return sbyteArray2D;
  }
}
