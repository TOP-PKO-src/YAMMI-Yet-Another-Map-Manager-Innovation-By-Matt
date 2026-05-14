using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using YetAnotherMapManager.Core;

#nullable disable
namespace YetAnotherMapManager.UI;

public class TextureSettings : UserControl
{
  private Dictionary<int, TextureView> textures;
  private IContainer components = new Container();
  private Panel ContainerPane;
  private Panel toolsPanel;
  private Button NewTextureButton;

  public TextureSettings()
  {
    this.InitializeComponent();
    this.textures = new Dictionary<int, TextureView>();
  }

  public void AddTexture(TextureDef textureDef)
  {
    if (this.textures.ContainsKey(textureDef.Id))
    {
      int num = (int) MessageBox.Show($"Duplicate texture definition ! [id={(object) textureDef.Id}]");
    }
    else
    {
      TextureView textureView = new TextureView(textureDef);
      this.textures[textureDef.Id] = textureView;
      textureView.Dock = DockStyle.Top;
      textureView.BorderStyle = BorderStyle.None;
      textureView.Margin = new Padding(5);
      textureView.TextureSettingsChanged += new TextureView.TextureSettingsChangedHandler(this.textureView_TextureSettingsChanged);
      this.ContainerPane.Controls.Add((Control) textureView);
    }
  }

  private void textureView_TextureSettingsChanged(
    object sender,
    TextureSettingsChangedEventArgs args)
  {
    this.OnTextureSettingsChanged(args);
  }

  public event TextureSettings.TextureSettingsChangedHandler TextureSettingsChanged;

  protected virtual void OnTextureSettingsChanged(TextureSettingsChangedEventArgs args)
  {
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
    this.ContainerPane = new Panel();
    this.toolsPanel = new Panel();
    this.NewTextureButton = new Button();
    this.toolsPanel.SuspendLayout();
    this.SuspendLayout();
    this.ContainerPane.AutoScroll = true;
    this.ContainerPane.Dock = DockStyle.Fill;
    this.ContainerPane.Location = new Point(0, 0);
    this.ContainerPane.Name = "ContainerPane";
    this.ContainerPane.Size = new Size(150, 128 /*0x80*/);
    this.ContainerPane.TabIndex = 0;
    this.toolsPanel.Controls.Add((Control) this.NewTextureButton);
    this.toolsPanel.Dock = DockStyle.Bottom;
    this.toolsPanel.Location = new Point(0, 128 /*0x80*/);
    this.toolsPanel.Name = "toolsPanel";
    this.toolsPanel.Size = new Size(150, 22);
    this.toolsPanel.TabIndex = 1;
    this.NewTextureButton.Dock = DockStyle.Right;
    this.NewTextureButton.Location = new Point(89, 0);
    this.NewTextureButton.Name = "NewTextureButton";
    this.NewTextureButton.Size = new Size(61, 22);
    this.NewTextureButton.TabIndex = 0;
    this.NewTextureButton.Text = "New";
    this.NewTextureButton.UseVisualStyleBackColor = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.ContainerPane);
    this.Controls.Add((Control) this.toolsPanel);
    this.Name = nameof (TextureSettings);
    this.toolsPanel.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  public delegate void TextureSettingsChangedHandler(
    object sender,
    TextureSettingsChangedEventArgs args);
}
