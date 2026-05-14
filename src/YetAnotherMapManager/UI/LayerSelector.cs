using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace YetAnotherMapManager.UI;

public class LayerSelector : UserControl
{
  private List<Layer> layers = new List<Layer>();
  private Layer selectedLayer;
  private IContainer components = new Container();
  private Panel containerPanel;

  public event LayerSelector.SettingsChangedHandler SettingsChanged;

  protected virtual void OnSettingsChanged(SettingsChangedEventArgs args)
  {
    if (this.SettingsChanged == null)
      return;
    this.SettingsChanged((object) this, args);
  }

  public LayerSelector() => this.InitializeComponent();

  public void AddLayer(string name, bool selected, float alphaLevel)
  {
    Layer layer = new Layer();
    layer.Displayed = selected;
    layer.AlphaLevel = alphaLevel;
    layer.LayerName = name;
    layer.Dock = DockStyle.Top;
    layer.BorderStyle = BorderStyle.None;
    layer.Width = this.containerPanel.Width;
    layer.MouseClick += new MouseEventHandler(this.layer_MouseClick);
    layer.SettingsChanged += new Layer.SettingsChangedHandler(this.layer_SettingsChanged);
    this.layers.Add(layer);
    this.containerPanel.Controls.Add((Control) layer);
  }

  private void layer_SettingsChanged(object sender, SettingsChangedEventArgs args)
  {
    this.OnSettingsChanged(args);
  }

  private void layer_MouseClick(object sender, MouseEventArgs e)
  {
    Layer layer = (Layer) sender;
    if (this.selectedLayer != null && this.selectedLayer != layer)
      this.selectedLayer.BackColor = layer.BackColor;
    layer.BackColor = Color.Aqua;
    this.selectedLayer = layer;
  }

  public string SelectedLayer
  {
    get => this.selectedLayer == null ? (string) null : this.selectedLayer.Name;
  }

  public float getLayerAlphaLevel(string name)
  {
    for (int index = 0; index < this.layers.Count; ++index)
    {
      if (this.layers[index].LayerName == name)
        return this.layers[index].AlphaLevel;
    }
    return 1f;
  }

  public List<string> getVisibleLayersList()
  {
    List<string> visibleLayersList = new List<string>(this.layers.Count);
    for (int index = 0; index < this.layers.Count; ++index)
    {
      if (this.layers[index].Displayed)
        visibleLayersList.Add(this.layers[index].LayerName);
    }
    return visibleLayersList;
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.containerPanel = new Panel();
    this.SuspendLayout();
    this.containerPanel.AutoScroll = true;
    this.containerPanel.Dock = DockStyle.Fill;
    this.containerPanel.Location = new Point(0, 0);
    this.containerPanel.Name = "containerPanel";
    this.containerPanel.Size = new Size(300, 170);
    this.containerPanel.TabIndex = 0;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.containerPanel);
    this.Name = nameof (LayerSelector);
    this.Size = new Size(300, 170);
    this.ResumeLayout(false);
  }

  public delegate void SettingsChangedHandler(object sender, SettingsChangedEventArgs args);
}
