using System;
using System.Drawing;

#nullable disable
namespace YetAnotherMapManager.UI;

public class TextureSettingsChangedEventArgs : EventArgs
{
  public int TextureId;
  public bool Created;
  public bool Deleted;
  public Color RenderingColor;
}
