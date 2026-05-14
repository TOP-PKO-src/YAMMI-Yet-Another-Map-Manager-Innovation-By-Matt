using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace YetAnotherMapManager.Rendering;

public class ToolBar : UserControl
{
  private IContainer components = new Container();
  private Button button1;
  private Button button2;

  public ToolBar() => this.InitializeComponent();

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.button1 = new Button();
    this.button2 = new Button();
    this.SuspendLayout();
    this.button1.Location = new Point(0, 0);
    this.button1.Name = "button1";
    this.button1.Size = new Size(32 /*0x20*/, 32 /*0x20*/);
    this.button1.TabIndex = 0;
    this.button1.Text = "button1";
    this.button1.UseVisualStyleBackColor = true;
    this.button2.Location = new Point(35, 0);
    this.button2.Name = "button2";
    this.button2.Size = new Size(32 /*0x20*/, 32 /*0x20*/);
    this.button2.TabIndex = 1;
    this.button2.Text = "button2";
    this.button2.UseVisualStyleBackColor = true;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.button2);
    this.Controls.Add((Control) this.button1);
    this.Name = nameof (ToolBar);
    this.Size = new Size(66, 299);
    this.ResumeLayout(false);
  }
}
