using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using YetAnotherMapManager.Core;

#nullable disable
namespace YetAnotherMapManager.UI;

public class AreaView : UserControl
{
  private AreaDef areaDef;
  private IContainer components = new Container();
  private TextBox areaName;
  private Button ColorButton;

  public AreaDef AreaDef => this.areaDef;

  public string AreaName => this.areaName.Text;

  public Color AreaColor => this.ColorButton.BackColor;

  public AreaView(AreaDef areaDef)
  {
    this.InitializeComponent();
    this.areaDef = areaDef;
    this.areaName.Text = areaDef.Name;
    this.ColorButton.BackColor = areaDef.RenderColor;
  }

  private void button1_Click(object sender, EventArgs e)
  {
    ColorDialog colorDialog = new ColorDialog();
    if (colorDialog.ShowDialog() == DialogResult.OK)
      this.ColorButton.BackColor = colorDialog.Color;
    this.areaDef.RenderColor = colorDialog.Color;
    colorDialog.Dispose();
    this.OnAreaSettingsChanged();
  }

  public event AreaView.AreaSettingsChangedHandler AreaSettingsChanged;

  protected virtual void OnAreaSettingsChanged()
  {
    AreaSettingsChangedEventArgs args = new AreaSettingsChangedEventArgs();
    args.AreaId = this.areaDef.Id;
    args.RenderingColor = this.areaDef.RenderColor;
    args.Deleted = false;
    args.Created = false;
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
    this.areaName = new TextBox();
    this.ColorButton = new Button();
    this.SuspendLayout();
    this.areaName.Enabled = false;
    this.areaName.Location = new Point(79, 11);
    this.areaName.Name = "areaName";
    this.areaName.Size = new Size(137, 20);
    this.areaName.TabIndex = 0;
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
    this.Controls.Add((Control) this.areaName);
    this.Name = nameof (AreaView);
    this.Size = new Size(219, 41);
    this.ResumeLayout(false);
    this.PerformLayout();
  }

  public delegate void AreaSettingsChangedHandler(object sender, AreaSettingsChangedEventArgs args);
}
