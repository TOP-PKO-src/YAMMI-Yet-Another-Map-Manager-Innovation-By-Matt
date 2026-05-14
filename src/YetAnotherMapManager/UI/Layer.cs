using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace YetAnotherMapManager.UI;

public class Layer : UserControl
{
  private IContainer components = new Container();
  private CheckBox visibleCheckBox;
  private Label NameLabel;
  private TrackBar alphaLevel;

  public string LayerName
  {
    get => this.NameLabel.Text;
    set => this.NameLabel.Text = value;
  }

  public float AlphaLevel
  {
    get => (float) this.alphaLevel.Value / (float) this.alphaLevel.Maximum;
    set => this.alphaLevel.Value = (int) ((double) value * (double) this.alphaLevel.Maximum);
  }

  public bool Displayed
  {
    get => this.visibleCheckBox.Checked;
    set => this.visibleCheckBox.Checked = value;
  }

  public Layer() => this.InitializeComponent();

  public event Layer.SettingsChangedHandler SettingsChanged;

  protected virtual void OnSettingsChanged()
  {
    SettingsChangedEventArgs args = new SettingsChangedEventArgs();
    args.Name = this.LayerName;
    args.Displayed = this.visibleCheckBox.Checked;
    args.Alpha = this.AlphaLevel;
    if (this.SettingsChanged == null)
      return;
    this.SettingsChanged((object) this, args);
  }

  private void alphaLevel_Scroll(object sender, EventArgs e) => this.OnSettingsChanged();

  private void visibleCheckBox_CheckedChanged(object sender, EventArgs e)
  {
    this.OnSettingsChanged();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.visibleCheckBox = new CheckBox();
    this.NameLabel = new Label();
    this.alphaLevel = new TrackBar();
    this.alphaLevel.BeginInit();
    this.SuspendLayout();
    this.visibleCheckBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
    this.visibleCheckBox.AutoSize = true;
    this.visibleCheckBox.Location = new Point(295, 10);
    this.visibleCheckBox.Name = "visibleCheckBox";
    this.visibleCheckBox.Size = new Size(15, 14);
    this.visibleCheckBox.TabIndex = 0;
    this.visibleCheckBox.UseVisualStyleBackColor = true;
    this.visibleCheckBox.CheckedChanged += new EventHandler(this.visibleCheckBox_CheckedChanged);
    this.NameLabel.AutoSize = true;
    this.NameLabel.Location = new Point(3, 10);
    this.NameLabel.Name = "NameLabel";
    this.NameLabel.Size = new Size(88, 13);
    this.NameLabel.TabIndex = 1;
    this.NameLabel.Text = "<LAYER NAME>";
    this.alphaLevel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.alphaLevel.Location = new Point(113, 3);
    this.alphaLevel.Maximum = 100;
    this.alphaLevel.Name = "alphaLevel";
    this.alphaLevel.Size = new Size(163, 42);
    this.alphaLevel.TabIndex = 2;
    this.alphaLevel.TickFrequency = 26;
    this.alphaLevel.TickStyle = TickStyle.None;
    this.alphaLevel.Value = 100;
    this.alphaLevel.Scroll += new EventHandler(this.alphaLevel_Scroll);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.alphaLevel);
    this.Controls.Add((Control) this.NameLabel);
    this.Controls.Add((Control) this.visibleCheckBox);
    this.Name = nameof (Layer);
    this.Size = new Size(313, 34);
    this.alphaLevel.EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  public delegate void SettingsChangedHandler(object sender, SettingsChangedEventArgs args);
}
