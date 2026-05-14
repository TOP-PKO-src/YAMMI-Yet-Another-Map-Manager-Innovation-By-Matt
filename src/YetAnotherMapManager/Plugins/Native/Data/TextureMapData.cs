using System;
using System.Collections.Generic;
using YetAnotherMapManager.Common;
using YetAnotherMapManager.MapData;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Data;

[Serializable]
public class TextureMapData : IMapData
{
  private ByteArray_2D textures;
  private List<byte> textureList;
  public static string KEY = "TEXTURE";

  public List<byte> TextureList => this.textureList;

  public byte this[int x, int y]
  {
    set
    {
      if (!this.textureList.Contains(value))
        this.textureList.Add(value);
      this.textures[x, y] = value;
    }
    get => this.textures[x, y];
  }

  public void InitializeData(int width, int height)
  {
    this.textures = new ByteArray_2D(width, height, (byte) 0);
  }

  public void InitializeData(MapInfo map) => this.InitializeData(map.Width, map.Height);

  public bool OverwriteData(ByteArray_2D input)
  {
    if (input.Width != this.textures.Width || input.Height != this.textures.Height)
      return false;
    this.textures = input;
    for (int x = 0; x < this.textures.Width; ++x)
    {
      for (int y = 0; y < this.textures.Height; ++y)
      {
        byte texture = this.textures[x, y];
        if (!this.textureList.Contains(texture))
          this.textureList.Add(texture);
      }
    }
    return true;
  }

  public void CopyArea(
    IMapData toMapData,
    int toX,
    int toY,
    int fromX,
    int fromY,
    int width,
    int height)
  {
    for (int index1 = 0; index1 < width; ++index1)
    {
      for (int index2 = 0; index2 < height; ++index2)
        ((TextureMapData) toMapData)[toX + index1, toY + index2] = this[fromX + index1, fromY + index2];
    }
  }

  public IMapData CloneArea(int fromX, int fromY, int width, int height)
  {
    TextureMapData toMapData = new TextureMapData();
    toMapData.InitializeData(width, height);
    this.CopyArea((IMapData) toMapData, 0, 0, fromX, fromY, width, height);
    return (IMapData) toMapData;
  }

  public TextureMapData() => this.textureList = new List<byte>();
}
