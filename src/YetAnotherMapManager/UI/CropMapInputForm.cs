using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace YetAnotherMapManager.UI;

public class CropMapInputForm : Form
{
  private IContainer components = new Container();
  private Label MapWidthLabel;
  private Label MapHeightLabel;
  private Button OkButton;
  private new Button CancelButton;
  private TextBox I_FROM_X;
  private TextBox I_TO_X;
  private TextBox I_FROM_Y;
  private TextBox I_TO_Y;
  private Label label1;
  private Label label2;

  public CropMapInputForm() => this.InitializeComponent();

  public int From_X => int.Parse(this.I_FROM_X.Text);

  public int To_X => int.Parse(this.I_TO_X.Text);

  public int From_Y => int.Parse(this.I_FROM_Y.Text);

  public int To_Y => int.Parse(this.I_TO_Y.Text);

  public new bool Validate()
  {
    bool flag = true;
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    try
    {
      num1 = int.Parse(this.I_FROM_X.Text);
      this.I_FROM_X.BackColor = Color.White;
    }
    catch
    {
      this.I_FROM_X.BackColor = Color.Red;
      flag = false;
    }
    try
    {
      num2 = int.Parse(this.I_FROM_Y.Text);
      this.I_FROM_Y.BackColor = Color.White;
    }
    catch
    {
      this.I_FROM_Y.BackColor = Color.Red;
      flag = false;
    }
    try
    {
      num3 = int.Parse(this.I_TO_X.Text);
      this.I_TO_X.BackColor = Color.White;
    }
    catch
    {
      this.I_TO_X.BackColor = Color.Red;
      flag = false;
    }
    try
    {
      num4 = int.Parse(this.I_TO_Y.Text);
      this.I_TO_Y.BackColor = Color.White;
    }
    catch
    {
      this.I_TO_Y.BackColor = Color.Red;
      flag = false;
    }
    if (flag)
    {
      if (num1 >= num3)
      {
        this.I_TO_X.BackColor = Color.Red;
        this.I_FROM_X.BackColor = Color.Red;
        flag = false;
      }
      if (num2 >= num4)
      {
        this.I_TO_Y.BackColor = Color.Red;
        this.I_FROM_Y.BackColor = Color.Red;
        flag = false;
      }
    }
    return flag;
  }

  private void OkButton_Click(object sender, EventArgs e)
  {
    if (!this.Validate())
      return;
    this.DialogResult = DialogResult.OK;
    this.Close();
  }

  private void CancelButton_Click(object sender, EventArgs e)
  {
    this.DialogResult = DialogResult.Cancel;
    this.Close();
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
    this.I_FROM_X = new TextBox();
    this.I_TO_X = new TextBox();
    this.I_FROM_Y = new TextBox();
    this.I_TO_Y = new TextBox();
    this.label1 = new Label();
    this.label2 = new Label();
    this.SuspendLayout();
    this.MapWidthLabel.AutoSize = true;
    this.MapWidthLabel.Location = new Point(12, 27);
    this.MapWidthLabel.Name = "MapWidthLabel";
    this.MapWidthLabel.Size = new Size(87, 13);
    this.MapWidthLabel.TabIndex = 1;
    this.MapWidthLabel.Text = "Upper left Corner";
    this.MapHeightLabel.AutoSize = true;
    this.MapHeightLabel.Location = new Point(12, 52);
    this.MapHeightLabel.Name = "MapHeightLabel";
    this.MapHeightLabel.Size = new Size(102, 13);
    this.MapHeightLabel.TabIndex = 2;
    this.MapHeightLabel.Text = "Bottom Right Corner";
    this.OkButton.Location = new Point(12, 85);
    this.OkButton.Name = "OkButton";
    this.OkButton.Size = new Size(75, 23);
    this.OkButton.TabIndex = 4;
    this.OkButton.Text = "Ok";
    this.OkButton.UseVisualStyleBackColor = true;
    this.OkButton.Click += new EventHandler(this.OkButton_Click);
    this.CancelButton.DialogResult = DialogResult.Cancel;
    this.CancelButton.Location = new Point(95, 85);
    this.CancelButton.Name = "CancelButton";
    this.CancelButton.Size = new Size(75, 23);
    this.CancelButton.TabIndex = 5;
    this.CancelButton.Text = "Cancel";
    this.CancelButton.UseVisualStyleBackColor = true;
    this.CancelButton.Click += new EventHandler(this.CancelButton_Click);
    this.I_FROM_X.Location = new Point(144 /*0x90*/, 27);
    this.I_FROM_X.Name = "I_FROM_X";
    this.I_FROM_X.Size = new Size(63 /*0x3F*/, 20);
    this.I_FROM_X.TabIndex = 1;
    this.I_TO_X.Location = new Point(144 /*0x90*/, 49);
    this.I_TO_X.Name = "I_TO_X";
    this.I_TO_X.Size = new Size(63 /*0x3F*/, 20);
    this.I_TO_X.TabIndex = 3;
    this.I_FROM_Y.Location = new Point(214, 27);
    this.I_FROM_Y.Name = "I_FROM_Y";
    this.I_FROM_Y.Size = new Size(63 /*0x3F*/, 20);
    this.I_FROM_Y.TabIndex = 2;
    this.I_TO_Y.Location = new Point(214, 49);
    this.I_TO_Y.Name = "I_TO_Y";
    this.I_TO_Y.Size = new Size(63 /*0x3F*/, 20);
    this.I_TO_Y.TabIndex = 4;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(160 /*0xA0*/, 11);
    this.label1.Name = "label1";
    this.label1.Size = new Size(32 /*0x20*/, 13);
    this.label1.TabIndex = 10;
    this.label1.Text = "-- X --";
    this.label2.AutoSize = true;
    this.label2.Location = new Point(231, 11);
    this.label2.Name = "label2";
    this.label2.Size = new Size(32 /*0x20*/, 13);
    this.label2.TabIndex = 11;
    this.label2.Text = "-- Y --";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(289, 117);
    this.ControlBox = false;
    this.Controls.Add((Control) this.label2);
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.I_TO_Y);
    this.Controls.Add((Control) this.I_FROM_Y);
    this.Controls.Add((Control) this.I_TO_X);
    this.Controls.Add((Control) this.I_FROM_X);
    this.Controls.Add((Control) this.CancelButton);
    this.Controls.Add((Control) this.OkButton);
    this.Controls.Add((Control) this.MapHeightLabel);
    this.Controls.Add((Control) this.MapWidthLabel);
    this.FormBorderStyle = FormBorderStyle.FixedSingle;
    this.Name = nameof (CropMapInputForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Map Cropping";
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
