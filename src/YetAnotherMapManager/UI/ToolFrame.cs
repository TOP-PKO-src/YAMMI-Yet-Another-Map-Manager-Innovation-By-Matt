using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace YetAnotherMapManager.UI;

public class ToolFrame : Form
{
  private IContainer components = new Container();
  private Panel container;

  public ToolFrame(string name, UserControl control)
  {
    this.InitializeComponent();
    this.Height = control.Height + 60;
    this.Text = name;
    this.container.Controls.Add((Control) control);
    control.Dock = DockStyle.Fill;
  }

  public void ToggleVisibility(object sender, EventArgs args)
  {
    if (this.Visible)
      this.Hide();
    else
      this.Show();
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.container = new Panel();
    this.SuspendLayout();
    this.container.Dock = DockStyle.Fill;
    this.container.Location = new Point(0, 0);
    this.container.Margin = new Padding(2, 3, 2, 3);
    this.container.Name = "container";
    this.container.Size = new Size(217, 91);
    this.container.TabIndex = 0;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BackgroundImageLayout = ImageLayout.None;
    this.ClientSize = new Size(217, 91);
    this.ControlBox = false;
    this.Controls.Add((Control) this.container);
    this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.Margin = new Padding(2, 3, 2, 3);
    this.MaximizeBox = false;
    this.MinimizeBox = false;
    this.Name = nameof (ToolFrame);
    this.ShowIcon = false;
    this.ShowInTaskbar = false;
    this.StartPosition = FormStartPosition.Manual;
    this.Text = "<Title>";
    this.ResumeLayout(false);
  }
}
