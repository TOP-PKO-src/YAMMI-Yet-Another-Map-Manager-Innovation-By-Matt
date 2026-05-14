using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using YetAnotherMapManager.Core.Security;

#nullable disable
namespace YetAnotherMapManager.Core;

[XmlRoot(ElementName = "Application", IsNullable = false)]
[Serializable]
public class Settings
{
  private License l;
  private string gameClient;
  public static int DATA_FORMAT_VERSION = 1;
  private string licenseKey;
  private List<TextureDef> textures;
  private List<AreaDef> areas;

  [XmlIgnore]
  public License L
  {
    get => this.l;
    set => this.l = value;
  }

  public string LicenseKey
  {
    set => this.licenseKey = value;
    get => this.licenseKey;
  }

  public string GameClient
  {
    get => this.gameClient;
    set => this.gameClient = value;
  }

  [XmlArrayItem("Texture")]
  [XmlArray("TextureList")]
  public List<TextureDef> Textures
  {
    get => this.textures;
    set => this.textures = value;
  }

  [XmlArray("AreaList")]
  [XmlArrayItem("Area")]
  public List<AreaDef> Areas
  {
    get => this.areas;
    set => this.areas = value;
  }

  public Settings()
  {
    this.textures = new List<TextureDef>();
    this.areas = new List<AreaDef>();
  }

  public void AddTextureDef(TextureDef texture) => this.textures.Add(texture);

  public void ClearTextures() => this.textures.Clear();

  public void AddAreaDef(AreaDef area) => this.areas.Add(area);

  public void ClearAreas() => this.areas.Clear();
}
