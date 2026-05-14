using System;
using System.Drawing;
using System.Windows.Forms;
using YetAnotherMapManager.Core;

#nullable disable
namespace YetAnotherMapManager;

internal class TextureToolStripItem : ToolStripButton
{
  private TextureDef textureDef;

  public TextureDef TextureDef => this.textureDef;

  public TextureToolStripItem(TextureDef texture)
    : base(texture.Filename, (Image) texture.Texture, (EventHandler) null)
  {
    this.textureDef = texture;
    this.Visible = true;
    this.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
    this.ImageScaling = ToolStripItemImageScaling.SizeToFit;
  }
}
