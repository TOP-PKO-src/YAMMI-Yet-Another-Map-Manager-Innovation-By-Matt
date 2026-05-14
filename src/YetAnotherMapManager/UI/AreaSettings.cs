using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using YetAnotherMapManager.Core;

#nullable disable
namespace YetAnotherMapManager.UI;

public class AreaSettings : UserControl
{
  private Dictionary<int, AreaView> areas;
  private IContainer components = new Container();
  private Panel ContainerPane;
  private Panel toolsPanel;
  private Button NewTextureButton;

  public AreaSettings()
  {
    this.InitializeComponent();
    this.areas = new Dictionary<int, AreaView>();
  }

  public void AddArea(AreaDef areaDef)
  {
    if (this.areas.ContainsKey(areaDef.Id))
    {
      int num = (int) MessageBox.Show($"Duplicate area definition ! [id={(object) areaDef.Id}]");
    }
    else
    {
      AreaView areaView = new AreaView(areaDef);
      this.areas[areaDef.Id] = areaView;
      areaView.Dock = DockStyle.Top;
      areaView.BorderStyle = BorderStyle.None;
      areaView.Margin = new Padding(5);
      areaView.AreaSettingsChanged += new AreaView.AreaSettingsChangedHandler(this.areaView_AreaSettingsChanged);
      this.ContainerPane.Controls.Add((Control) areaView);
    }
  }

  private void areaView_AreaSettingsChanged(object sender, AreaSettingsChangedEventArgs args)
  {
    this.OnAreaSettingsChanged(args);
  }

  public event AreaSettings.AreaSettingsChangedHandler AreaSettingsChanged;

  protected virtual void OnAreaSettingsChanged(AreaSettingsChangedEventArgs args)
  {
    if (this.AreaSettingsChanged == null)
      return;
    this.AreaSettingsChanged((object) this, args);
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
    this.Name = "TextureSettings";
    this.toolsPanel.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  public delegate void AreaSettingsChangedHandler(object sender, AreaSettingsChangedEventArgs args);
}
