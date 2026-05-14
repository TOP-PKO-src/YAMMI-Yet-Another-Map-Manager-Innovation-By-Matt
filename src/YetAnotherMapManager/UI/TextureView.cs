using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using YetAnotherMapManager.Core;

#nullable disable
namespace YetAnotherMapManager.UI;

public class TextureView : UserControl
{
  private TextureDef textureDef;
  private IContainer components = new Container();
  private TextBox textureName;
  private PictureBox TextureSample;
  private Button ColorButton;

  public TextureDef TextureDef => this.textureDef;

  public string TextureName => this.textureName.Text;

  public Color TextureColor => this.ColorButton.BackColor;

  public TextureView(TextureDef textureDef)
  {
    this.InitializeComponent();
    this.textureDef = textureDef;
    this.textureName.Text = textureDef.Filename;
    this.ColorButton.BackColor = textureDef.RenderColor;
    if (textureDef.Texture == null)
      return;
    this.TextureSample.Image = (Image) textureDef.Texture;
  }

  private void button1_Click(object sender, EventArgs e)
  {
    ColorDialog colorDialog = new ColorDialog();
    if (colorDialog.ShowDialog() == DialogResult.OK)
      this.ColorButton.BackColor = colorDialog.Color;
    this.textureDef.RenderColor = colorDialog.Color;
    colorDialog.Dispose();
    this.OnTextureSettingsChanged();
  }

  public event TextureView.TextureSettingsChangedHandler TextureSettingsChanged;

  protected virtual void OnTextureSettingsChanged()
  {
    TextureSettingsChangedEventArgs args = new TextureSettingsChangedEventArgs();
    args.TextureId = this.textureDef.Id;
    args.RenderingColor = this.textureDef.RenderColor;
    args.Deleted = false;
    args.Created = false;
    if (this.TextureSettingsChanged == null)
      return;
    this.TextureSettingsChanged((object) this, args);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.textureName = new TextBox();
    this.TextureSample = new PictureBox();
    this.ColorButton = new Button();
    ((ISupportInitialize) this.TextureSample).BeginInit();
    this.SuspendLayout();
    this.textureName.Enabled = false;
    this.textureName.Location = new Point(79, 11);
    this.textureName.Name = "textureName";
    this.textureName.Size = new Size(137, 20);
    this.textureName.TabIndex = 0;
    this.TextureSample.BorderStyle = BorderStyle.FixedSingle;
    this.TextureSample.Location = new Point(39, 4);
    this.TextureSample.Name = "TextureSample";
    this.TextureSample.Size = new Size(34, 34);
    this.TextureSample.TabIndex = 1;
    this.TextureSample.TabStop = false;
    this.ColorButton.FlatStyle = FlatStyle.Flat;
    this.ColorButton.Location = new Point(3, 4);
    this.ColorButton.Name = "ColorButton";
    this.ColorButton.Size = new Size(34, 34);
    this.ColorButton.TabIndex = 2;
    this.ColorButton.UseVisualStyleBackColor = true;
    this.ColorButton.Click += new EventHandler(this.button1_Click);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.ColorButton);
    this.Controls.Add((Control) this.TextureSample);
    this.Controls.Add((Control) this.textureName);
    this.Name = nameof (TextureView);
    this.Size = new Size(219, 41);
    ((ISupportInitialize) this.TextureSample).EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  public delegate void TextureSettingsChangedHandler(
    object sender,
    TextureSettingsChangedEventArgs args);
}
