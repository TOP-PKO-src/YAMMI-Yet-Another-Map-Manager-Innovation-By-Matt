using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

#nullable disable
namespace YetAnotherMapManager.Core.Security;

internal class Serializer
{
  private Type type;

  public Serializer(Type type) => this.type = type;

  private string UTF8ByteArrayToString(byte[] characters)
  {
    return new UTF8Encoding().GetString(characters);
  }

  private byte[] StringToUTF8ByteArray(string pXmlString)
  {
    return new UTF8Encoding().GetBytes(pXmlString);
  }

  public virtual string SerializeObject(object obj)
  {
    try
    {
      MemoryStream w = new MemoryStream();
      XmlSerializer xmlSerializer = new XmlSerializer(this.type);
      XmlTextWriter xmlTextWriter = new XmlTextWriter((Stream) w, Encoding.UTF8);
      xmlSerializer.Serialize((XmlWriter) xmlTextWriter, obj);
      return this.UTF8ByteArrayToString(((MemoryStream) xmlTextWriter.BaseStream).ToArray());
    }
    catch (Exception ex)
    {
      Console.WriteLine((object) ex);
      return (string) null;
    }
  }

  public virtual object DeserializeObject(string pXmlizedString)
  {
    XmlSerializer xmlSerializer = new XmlSerializer(this.type);
    MemoryStream w = new MemoryStream(this.StringToUTF8ByteArray(pXmlizedString));
    XmlTextWriter xmlTextWriter = new XmlTextWriter((Stream) w, Encoding.UTF8);
    return xmlSerializer.Deserialize((Stream) w);
  }
}
