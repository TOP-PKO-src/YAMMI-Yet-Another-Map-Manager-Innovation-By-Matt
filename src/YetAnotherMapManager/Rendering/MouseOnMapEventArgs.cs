using System;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace YetAnotherMapManager.Rendering;

public class MouseOnMapEventArgs : EventArgs
{
  public bool InMap;
  public Point Location;
  public Point Coords;
  public Point UiCoords;
  public MouseButtons Button;
}
