using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace YetAnotherMapManager.UI;

public class NewMapInputForm : Form
{
  private IContainer components = new Container();
  private Label MapWidthLabel;
  private Label MapHeightLabel;
  private Button OkButton;
  private new Button CancelButton;
  private NumericUpDown MapWidthInput;
  private NumericUpDown MapHeightInput;

  public NewMapInputForm() => this.InitializeComponent();

  public int MapWidth => (int) this.MapWidthInput.Value;

  public int MapHeight => (int) this.MapHeightInput.Value;

  private void OkButton_Click(object sender, EventArgs e)
  {
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.MapWidthLabel = new Label();
    this.MapHeightLabel = new Label();
    this.OkButton = new Button();
    this.CancelButton = new Button();
    this.MapWidthInput = new NumericUpDown();
    this.MapHeightInput = new NumericUpDown();
    this.MapWidthInput.BeginInit();
    this.MapHeightInput.BeginInit();
    this.SuspendLayout();
    this.MapWidthLabel.AutoSize = true;
    this.MapWidthLabel.Location = new Point(12, 15);
    this.MapWidthLabel.Name = "MapWidthLabel";
    this.MapWidthLabel.Size = new Size(59, 13);
    this.MapWidthLabel.TabIndex = 1;
    this.MapWidthLabel.Text = "Map Width";
    this.MapHeightLabel.AutoSize = true;
    this.MapHeightLabel.Location = new Point(12, 50);
    this.MapHeightLabel.Name = "MapHeightLabel";
    this.MapHeightLabel.Size = new Size(62, 13);
    this.MapHeightLabel.TabIndex = 2;
    this.MapHeightLabel.Text = "Map Height";
    this.OkButton.DialogResult = DialogResult.OK;
    this.OkButton.Location = new Point(12, 83);
    this.OkButton.Name = "OkButton";
    this.OkButton.Size = new Size(75, 23);
    this.OkButton.TabIndex = 4;
    this.OkButton.Text = "Ok";
    this.OkButton.UseVisualStyleBackColor = true;
    this.OkButton.Click += new EventHandler(this.OkButton_Click);
    this.CancelButton.DialogResult = DialogResult.Cancel;
    this.CancelButton.Location = new Point(95, 83);
    this.CancelButton.Name = "CancelButton";
    this.CancelButton.Size = new Size(75, 23);
    this.CancelButton.TabIndex = 5;
    this.CancelButton.Text = "Cancel";
    this.CancelButton.UseVisualStyleBackColor = true;
    this.MapWidthInput.Increment = new Decimal(new int[4]
    {
      8,
      0,
      0,
      0
    });
    this.MapWidthInput.Location = new Point(95, 13);
    this.MapWidthInput.Maximum = new Decimal(new int[4]
    {
      4096 /*0x1000*/,
      0,
      0,
      0
    });
    this.MapWidthInput.Minimum = new Decimal(new int[4]
    {
      8,
      0,
      0,
      0
    });
    this.MapWidthInput.Name = "MapWidthInput";
    this.MapWidthInput.Size = new Size(75, 20);
    this.MapWidthInput.TabIndex = 6;
    this.MapWidthInput.Value = new Decimal(new int[4]
    {
      128 /*0x80*/,
      0,
      0,
      0
    });
    this.MapHeightInput.Increment = new Decimal(new int[4]
    {
      8,
      0,
      0,
      0
    });
    this.MapHeightInput.Location = new Point(95, 48 /*0x30*/);
    this.MapHeightInput.Maximum = new Decimal(new int[4]
    {
      4096 /*0x1000*/,
      0,
      0,
      0
    });
    this.MapHeightInput.Minimum = new Decimal(new int[4]
    {
      8,
      0,
      0,
      0
    });
    this.MapHeightInput.Name = "MapHeightInput";
    this.MapHeightInput.Size = new Size(75, 20);
    this.MapHeightInput.TabIndex = 7;
    this.MapHeightInput.Value = new Decimal(new int[4]
    {
      128 /*0x80*/,
      0,
      0,
      0
    });
    this.AcceptButton = (IButtonControl) this.OkButton;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(182, 118);
    this.ControlBox = false;
    this.Controls.Add((Control) this.MapHeightInput);
    this.Controls.Add((Control) this.MapWidthInput);
    this.Controls.Add((Control) this.CancelButton);
    this.Controls.Add((Control) this.OkButton);
    this.Controls.Add((Control) this.MapHeightLabel);
    this.Controls.Add((Control) this.MapWidthLabel);
    this.FormBorderStyle = FormBorderStyle.FixedSingle;
    this.Name = nameof (NewMapInputForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "New Map Creation";
    this.MapWidthInput.EndInit();
    this.MapHeightInput.EndInit();
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
