using System;
using System.Drawing;
using System.Drawing.Imaging;

#nullable disable
namespace YetAnotherMapManager.Rendering;

public class FastBitmap : IDisposable
{
  private Bitmap bitmap;
  private int _width;
  private BitmapData _bitmapData;
  private unsafe byte* _base = (byte*) null;

  public FastBitmap(Bitmap bitmap) => this.bitmap = bitmap;

  public FastBitmap(int width, int height)
  {
    this.bitmap = new Bitmap(width, height, PixelFormat.Format32bppArgb);
  }

  public void Dispose() => this.bitmap.Dispose();

  private Point PixelSize
  {
    get
    {
      GraphicsUnit pageUnit = GraphicsUnit.Pixel;
      RectangleF bounds = this.bitmap.GetBounds(ref pageUnit);
      return new Point((int) bounds.Width, (int) bounds.Height);
    }
  }

  public unsafe void LockBitmap()
  {
    GraphicsUnit pageUnit = GraphicsUnit.Pixel;
    RectangleF bounds = this.bitmap.GetBounds(ref pageUnit);
    Rectangle rect = new Rectangle((int) bounds.X, (int) bounds.Y, (int) bounds.Width, (int) bounds.Height);
    this._width = (int) bounds.Width * sizeof (PixelData);
    if (this._width % 4 != 0)
      this._width = 4 * (this._width / 4 + 1);
    this._bitmapData = this.bitmap.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
    this._base = (byte*) (void*) this._bitmapData.Scan0;
  }

  public unsafe PixelData GetPixel(int x, int y) => *this.PixelAt(x, y);

  public unsafe void SetPixel(int x, int y, PixelData color) => *this.PixelAt(x, y) = color;

  public unsafe void SetPixel(int x, int y, PixelData color, float alpha)
  {
    PixelData* pixelDataPtr = this.PixelAt(x, y);
    pixelDataPtr->Red = (byte) Math.Min((float) ((double) pixelDataPtr->Red * (1.0 - (double) alpha) + (double) color.Red * (double) alpha), (float) byte.MaxValue);
    pixelDataPtr->Green = (byte) Math.Min((float) ((double) pixelDataPtr->Green * (1.0 - (double) alpha) + (double) color.Green * (double) alpha), (float) byte.MaxValue);
    pixelDataPtr->Blue = (byte) Math.Min((float) ((double) pixelDataPtr->Blue * (1.0 - (double) alpha) + (double) color.Blue * (double) alpha), (float) byte.MaxValue);
  }

  public unsafe void UnlockBitmap()
  {
    this.bitmap.UnlockBits(this._bitmapData);
    this._bitmapData = (BitmapData) null;
    this._base = (byte*) null;
  }

  public unsafe PixelData* PixelAt(int x, int y)
  {
    return (PixelData*) (this._base + y * this._width + x * sizeof (PixelData));
  }

  public void drawHLine(int y, Color color)
  {
    if (color == Color.Transparent || y < 0 || y >= this.bitmap.Height)
      return;
    PixelData color1 = new PixelData(color);
    for (int x = 0; x < this.bitmap.Width; ++x)
      this.SetPixel(x, y, color1);
  }

  public void drawVLine(int x, Color color)
  {
    if (color == Color.Transparent || x < 0 || x >= this.bitmap.Width)
      return;
    PixelData color1 = new PixelData(color);
    for (int y = 0; y < this.bitmap.Height; ++y)
      this.SetPixel(x, y, color1);
  }

  public void fillRectangle(int _x, int _y, int _width, int _height, Color color, float alpha)
  {
    if (color == Color.Transparent || _y < 0 || _y >= this.bitmap.Height || _x < 0 || _x >= this.bitmap.Width)
      return;
    if (_x + _width > this.bitmap.Width)
      _width = this.bitmap.Width - _x - 1;
    if (_y + _height > this.bitmap.Height)
      _height = this.bitmap.Height - _y - 1;
    PixelData color1 = new PixelData(color);
    for (int x = _x; x < _x + _width; ++x)
    {
      for (int y = _y; y < _y + _height; ++y)
        this.SetPixel(x, y, color1, alpha);
    }
  }

  void IDisposable.Dispose() => this.Dispose();
}
