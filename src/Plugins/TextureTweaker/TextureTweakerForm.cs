using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using YetAnotherMapManager.Core;
using YetAnotherMapManager.Plugins.Native.Data;

#nullable disable
namespace TextureTweaker;

public class TextureTweakerForm : Form
{
  private static int CELL_SHOWN = 8;
  private static int CELL_SIZE = 64 /*0x40*/;
  private int posX;
  private int posY;
  private bool showGrid = true;
  private List<TextureDef> textures;
  private TextureMapData mapText;
  private TextureBlendingMapData mapBlend;
  private SeaLandMapData mapSea;
  private Panel drawingPanel;

  public TextureTweakerForm(
    List<TextureDef> textures,
    TextureMapData mapText,
    TextureBlendingMapData mapBlend,
    SeaLandMapData mapSea)
  {
    this.textures = textures;
    this.mapText = mapText;
    this.mapBlend = mapBlend;
    this.mapSea = mapSea;
    this.drawingPanel = new Panel();
    this.drawingPanel.Width = TextureTweakerForm.CELL_SHOWN * TextureTweakerForm.CELL_SIZE;
    this.drawingPanel.Height = TextureTweakerForm.CELL_SHOWN * TextureTweakerForm.CELL_SIZE;
    this.drawingPanel.BackColor = Color.Red;
    this.drawingPanel.BorderStyle = BorderStyle.FixedSingle;
    this.drawingPanel.Paint += new PaintEventHandler(this.PanelPaint);
    this.Width = this.drawingPanel.Width + 20;
    this.Height = this.drawingPanel.Height + 20;
    this.KeyDown += new KeyEventHandler(this.HandleKeyDown);
    this.Controls.Add((Control) this.drawingPanel);
    this.drawingPanel.Invalidate();
  }

  private void HandleKeyDown(object sender, KeyEventArgs e)
  {
    if (e.KeyCode == Keys.Up)
      --this.posY;
    else if (e.KeyCode == Keys.Down)
      ++this.posY;
    else if (e.KeyCode == Keys.Left)
      --this.posX;
    else if (e.KeyCode == Keys.Right)
      ++this.posX;
    this.drawingPanel.Invalidate();
  }

  public void PanelPaint(object sender, PaintEventArgs e)
  {
    this.SuspendLayout();
    using (Graphics graphics = e.Graphics)
    {
      for (int index1 = 0; index1 < TextureTweakerForm.CELL_SHOWN; ++index1)
      {
        for (int index2 = 0; index2 < TextureTweakerForm.CELL_SHOWN; ++index2)
        {
          Rectangle rect = new Rectangle(index1 * TextureTweakerForm.CELL_SIZE, index2 * TextureTweakerForm.CELL_SIZE, TextureTweakerForm.CELL_SIZE, TextureTweakerForm.CELL_SIZE);
          int x = index1 + this.posX;
          int y = index2 + this.posY;
          if (this.textures[(int) this.mapText[x, y]].Texture != null)
          {
            if ((int) this.mapSea[x, y] != (int) SeaLandMapData.TYPE_LAND)
              graphics.FillRectangle((Brush) new SolidBrush(Color.Aqua), rect);
            else
              graphics.DrawImage((Image) this.textures[(int) this.mapText[x, y]].Texture, rect);
          }
          else
            graphics.FillRectangle((Brush) new SolidBrush(Color.Red), rect);
          if (this.showGrid)
            graphics.DrawRectangle(new Pen(Color.Black), rect);
        }
      }
    }
    this.ResumeLayout();
  }
}
