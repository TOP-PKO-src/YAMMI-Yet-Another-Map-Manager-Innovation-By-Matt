using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

#nullable disable
namespace YetAnotherMapManager.Core;

[XmlRoot(ElementName = "Area", IsNullable = false)]
[Serializable]
public class AreaDef
{
  private int id;
  private string name;
  private Color color;

  public int Id
  {
    get => this.id;
    set => this.id = value;
  }

  public string Name
  {
    get => this.name;
    set => this.name = value;
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

  public AreaDef()
  {
  }

  public AreaDef(int id, string name, Color color)
  {
    this.id = id;
    this.name = name;
    this.color = color;
  }
}
