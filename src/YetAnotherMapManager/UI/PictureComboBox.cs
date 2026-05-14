using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace YetAnotherMapManager.UI;

internal class PictureComboBox : ComboBox
{
  private ImageList imageList;

  public ImageList ImageList
  {
    get => this.imageList;
    set => this.imageList = value;
  }

  public PictureComboBox() => this.DrawMode = DrawMode.OwnerDrawFixed;

  protected override void OnDrawItem(DrawItemEventArgs ea)
  {
    ea.DrawBackground();
    ea.DrawFocusRectangle();
    Size imageSize = this.imageList.ImageSize;
    Rectangle bounds = ea.Bounds;
    try
    {
      PictureComboBoxItem pictureComboBoxItem = (PictureComboBoxItem) this.Items[ea.Index];
      if (pictureComboBoxItem.ImageIndex != -1)
      {
        this.imageList.Draw(ea.Graphics, bounds.Left, bounds.Top, pictureComboBoxItem.ImageIndex);
        ea.Graphics.DrawString(pictureComboBoxItem.Text, ea.Font, (Brush) new SolidBrush(ea.ForeColor), (float) (bounds.Left + imageSize.Width), (float) bounds.Top);
      }
      else
        ea.Graphics.DrawString(pictureComboBoxItem.Text, ea.Font, (Brush) new SolidBrush(ea.ForeColor), (float) bounds.Left, (float) bounds.Top);
    }
    catch
    {
      if (ea.Index != -1)
        ea.Graphics.DrawString(this.Items[ea.Index].ToString(), ea.Font, (Brush) new SolidBrush(ea.ForeColor), (float) bounds.Left, (float) bounds.Top);
      else
        ea.Graphics.DrawString(this.Text, ea.Font, (Brush) new SolidBrush(ea.ForeColor), (float) bounds.Left, (float) bounds.Top);
    }
    base.OnDrawItem(ea);
  }
}
