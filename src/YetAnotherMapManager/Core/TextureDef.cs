using System;
using System.Drawing;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

#nullable disable
namespace YetAnotherMapManager.Core;

[XmlRoot(ElementName = "Texture", IsNullable = false)]
[Serializable]
public class TextureDef
{
  private int id;
  private string filename;
  private int flag1;
  private int flag2;
  private Color color;
  private Bitmap bitmap;
  private string cachedBitmapPath;

  public int Id
  {
    get => this.id;
    set => this.id = value;
  }

  public string Filename
  {
    get => this.filename;
    set => this.filename = value;
  }

  public int Flag1
  {
    get => this.flag1;
    set => this.flag1 = value;
  }

  public int Flag2
  {
    get => this.flag2;
    set => this.flag2 = value;
  }

  public string CachedBitmapPath
  {
    get => this.cachedBitmapPath;
    set => this.cachedBitmapPath = value;
  }

  [XmlIgnore]
  public Color RenderColor
  {
    get => this.color;
    set => this.color = value;
  }

  public string DisplayColor
  {
    get => $"A={this.color.A}:R={this.color.R}:G={this.color.G}:B={this.color.B}";
    set
    {
      Match match = new Regex("^A=(.+):R=(.+):G=(.+):B=(.+)$").Match(value);
      if (match.Success)
        this.color = Color.FromArgb(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), int.Parse(match.Groups[3].Value), int.Parse(match.Groups[4].Value));
      else
        this.color = Color.Transparent;
    }
  }

  [XmlIgnore]
  public Bitmap Texture
  {
    get
    {
      if (this.bitmap == null && this.cachedBitmapPath != null)
      {
        if (File.Exists(this.cachedBitmapPath))
        {
          try
          {
            this.bitmap = new Bitmap(this.cachedBitmapPath);
          }
          catch (Exception ex)
          {
            Console.WriteLine($"{ex.Message}\n{ex.StackTrace}\n---------------");
          }
        }
      }
      return this.bitmap;
    }
    set => this.bitmap = value;
  }

  public TextureDef()
  {
  }

  public TextureDef(int id, string filename, int flag1, int flag2, Color color)
  {
    this.id = id;
    this.filename = filename;
    this.flag1 = flag1;
    this.flag2 = flag2;
    this.color = color;
  }
}
