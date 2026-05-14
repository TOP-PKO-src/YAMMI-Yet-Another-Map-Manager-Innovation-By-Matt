using System;
using System.Xml.Serialization;

#nullable disable
namespace YetAnotherMapManager.Core.Security;

[XmlRoot(ElementName = "LICENSE", IsNullable = false)]
public class License
{
  [XmlAttribute("ID")]
  public string licenseId;
  [XmlAttribute("HARDWARE_KEY")]
  public string hardwareKey;
  [XmlAttribute("DELIVERED_TO")]
  public string deliveredTo;
  [XmlAttribute("SERVER_URL")]
  public string serverUrl;
  [XmlAttribute("END_OF_VALIDITY")]
  public DateTime endTime;
}
