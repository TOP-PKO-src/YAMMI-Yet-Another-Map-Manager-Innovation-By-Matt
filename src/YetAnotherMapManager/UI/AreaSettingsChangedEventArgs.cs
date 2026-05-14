using System;
using System.Drawing;

#nullable disable
namespace YetAnotherMapManager.UI;

public class AreaSettingsChangedEventArgs : EventArgs
{
  public int AreaId;
  public bool Created;
  public bool Deleted;
  public Color RenderingColor;
}
