using System.Drawing;
using System.Windows.Forms;
using YetAnotherMapManager.Core;

#nullable disable
namespace YetAnotherMapManager;

internal class AreaToolStripItem : ToolStripButton
{
  private AreaDef areaDef;

  public AreaDef AreaDef => this.areaDef;

  public AreaToolStripItem(AreaDef area)
    : base(area.Name, (Image) null)
  {
    this.areaDef = area;
    this.Visible = true;
    this.DisplayStyle = ToolStripItemDisplayStyle.Text;
  }
}
