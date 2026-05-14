using System.Windows.Forms;

#nullable disable
namespace YetAnotherMapManager.Rendering;

internal class DoubleBufferedPanel : Panel
{
  public DoubleBufferedPanel() => this.DoubleBuffered = true;
}
