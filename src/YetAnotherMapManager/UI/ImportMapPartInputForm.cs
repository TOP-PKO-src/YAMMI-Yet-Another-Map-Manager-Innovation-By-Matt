using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace YetAnotherMapManager.UI;

public class ImportMapPartInputForm : Form
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
  private GroupBox groupBox1;
  private TextBox I_fileName;
  private Button button1;
  private OpenFileDialog openFileDialog1;
  private GroupBox groupBox2;
  private Label label3;
  private TextBox I_Target_Y;
  private TextBox I_Target_X;

  public ImportMapPartInputForm() => this.InitializeComponent();

  public int From_X => int.Parse(this.I_FROM_X.Text);

  public int To_X => int.Parse(this.I_TO_X.Text);

  public int From_Y => int.Parse(this.I_FROM_Y.Text);

  public int To_Y => int.Parse(this.I_TO_Y.Text);

  public int Target_X => int.Parse(this.I_Target_X.Text);

  public int Target_Y => int.Parse(this.I_Target_Y.Text);

  public string Filename => this.I_fileName.Text;

  public new bool Validate()
  {
    bool flag = true;
    int num1 = 0;
    int num2 = 0;
    int num3 = 0;
    int num4 = 0;
    if (this.I_fileName.Text == "")
    {
      this.I_fileName.BackColor = Color.Red;
      flag = false;
    }
    else
      this.I_fileName.BackColor = Color.White;
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
    try
    {
      int.Parse(this.I_Target_X.Text);
      this.I_Target_X.BackColor = Color.White;
    }
    catch
    {
      this.I_Target_X.BackColor = Color.Red;
      flag = false;
    }
    try
    {
      int.Parse(this.I_Target_Y.Text);
      this.I_Target_Y.BackColor = Color.White;
    }
    catch
    {
      this.I_Target_Y.BackColor = Color.Red;
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

  private void button1_Click(object sender, EventArgs e)
  {
    if (this.openFileDialog1.ShowDialog() != DialogResult.OK)
      return;
    this.I_fileName.Text = this.openFileDialog1.FileName;
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
    this.groupBox1 = new GroupBox();
    this.I_fileName = new TextBox();
    this.button1 = new Button();
    this.openFileDialog1 = new OpenFileDialog();
    this.groupBox2 = new GroupBox();
    this.I_Target_X = new TextBox();
    this.I_Target_Y = new TextBox();
    this.label3 = new Label();
    this.groupBox1.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.SuspendLayout();
    this.MapWidthLabel.AutoSize = true;
    this.MapWidthLabel.Location = new Point(10, 74);
    this.MapWidthLabel.Name = "MapWidthLabel";
    this.MapWidthLabel.Size = new Size(87, 13);
    this.MapWidthLabel.TabIndex = 1;
    this.MapWidthLabel.Text = "Upper left Corner";
    this.MapHeightLabel.AutoSize = true;
    this.MapHeightLabel.Location = new Point(10, 99);
    this.MapHeightLabel.Name = "MapHeightLabel";
    this.MapHeightLabel.Size = new Size(102, 13);
    this.MapHeightLabel.TabIndex = 2;
    this.MapHeightLabel.Text = "Bottom Right Corner";
    this.OkButton.Location = new Point(12, 197);
    this.OkButton.Name = "OkButton";
    this.OkButton.Size = new Size(75, 23);
    this.OkButton.TabIndex = 4;
    this.OkButton.Text = "Ok";
    this.OkButton.UseVisualStyleBackColor = true;
    this.OkButton.Click += new EventHandler(this.OkButton_Click);
    this.CancelButton.DialogResult = DialogResult.Cancel;
    this.CancelButton.Location = new Point(93, 197);
    this.CancelButton.Name = "CancelButton";
    this.CancelButton.Size = new Size(75, 23);
    this.CancelButton.TabIndex = 5;
    this.CancelButton.Text = "Cancel";
    this.CancelButton.UseVisualStyleBackColor = true;
    this.CancelButton.Click += new EventHandler(this.CancelButton_Click);
    this.I_FROM_X.Location = new Point(142, 74);
    this.I_FROM_X.Name = "I_FROM_X";
    this.I_FROM_X.Size = new Size(63 /*0x3F*/, 20);
    this.I_FROM_X.TabIndex = 1;
    this.I_TO_X.Location = new Point(142, 96 /*0x60*/);
    this.I_TO_X.Name = "I_TO_X";
    this.I_TO_X.Size = new Size(63 /*0x3F*/, 20);
    this.I_TO_X.TabIndex = 3;
    this.I_FROM_Y.Location = new Point(212, 74);
    this.I_FROM_Y.Name = "I_FROM_Y";
    this.I_FROM_Y.Size = new Size(63 /*0x3F*/, 20);
    this.I_FROM_Y.TabIndex = 2;
    this.I_TO_Y.Location = new Point(212, 96 /*0x60*/);
    this.I_TO_Y.Name = "I_TO_Y";
    this.I_TO_Y.Size = new Size(63 /*0x3F*/, 20);
    this.I_TO_Y.TabIndex = 4;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(158, 58);
    this.label1.Name = "label1";
    this.label1.Size = new Size(32 /*0x20*/, 13);
    this.label1.TabIndex = 10;
    this.label1.Text = "-- X --";
    this.label2.AutoSize = true;
    this.label2.Location = new Point(229, 58);
    this.label2.Name = "label2";
    this.label2.Size = new Size(32 /*0x20*/, 13);
    this.label2.TabIndex = 11;
    this.label2.Text = "-- Y --";
    this.groupBox1.Controls.Add((Control) this.I_fileName);
    this.groupBox1.Controls.Add((Control) this.button1);
    this.groupBox1.Controls.Add((Control) this.I_TO_X);
    this.groupBox1.Controls.Add((Control) this.label2);
    this.groupBox1.Controls.Add((Control) this.MapWidthLabel);
    this.groupBox1.Controls.Add((Control) this.label1);
    this.groupBox1.Controls.Add((Control) this.MapHeightLabel);
    this.groupBox1.Controls.Add((Control) this.I_TO_Y);
    this.groupBox1.Controls.Add((Control) this.I_FROM_X);
    this.groupBox1.Controls.Add((Control) this.I_FROM_Y);
    this.groupBox1.Location = new Point(2, 1);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(285, 129);
    this.groupBox1.TabIndex = 12;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Source";
    this.I_fileName.Location = new Point(13, 19);
    this.I_fileName.Name = "I_fileName";
    this.I_fileName.ReadOnly = true;
    this.I_fileName.Size = new Size(230, 20);
    this.I_fileName.TabIndex = 13;
    this.button1.Location = new Point(249, 19);
    this.button1.Name = "button1";
    this.button1.Size = new Size(26, 23);
    this.button1.TabIndex = 12;
    this.button1.Text = "...";
    this.button1.UseVisualStyleBackColor = true;
    this.button1.Click += new EventHandler(this.button1_Click);
    this.openFileDialog1.FileName = "openFileDialog1";
    this.openFileDialog1.Filter = "Map Files|*.map";
    this.openFileDialog1.Title = "Select Soure Map File";
    this.groupBox2.Controls.Add((Control) this.label3);
    this.groupBox2.Controls.Add((Control) this.I_Target_Y);
    this.groupBox2.Controls.Add((Control) this.I_Target_X);
    this.groupBox2.Location = new Point(2, 137);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(285, 54);
    this.groupBox2.TabIndex = 13;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Target";
    this.I_Target_X.Location = new Point(142, 19);
    this.I_Target_X.Name = "I_Target_X";
    this.I_Target_X.Size = new Size(63 /*0x3F*/, 20);
    this.I_Target_X.TabIndex = 0;
    this.I_Target_Y.Location = new Point(211, 19);
    this.I_Target_Y.Name = "I_Target_Y";
    this.I_Target_Y.Size = new Size(63 /*0x3F*/, 20);
    this.I_Target_Y.TabIndex = 1;
    this.label3.AutoSize = true;
    this.label3.Location = new Point(13, 25);
    this.label3.Name = "label3";
    this.label3.Size = new Size(86, 13);
    this.label3.TabIndex = 2;
    this.label3.Text = "Upper left corner";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(289, 232);
    this.ControlBox = false;
    this.Controls.Add((Control) this.groupBox2);
    this.Controls.Add((Control) this.groupBox1);
    this.Controls.Add((Control) this.CancelButton);
    this.Controls.Add((Control) this.OkButton);
    this.FormBorderStyle = FormBorderStyle.FixedSingle;
    this.Name = nameof (ImportMapPartInputForm);
    this.StartPosition = FormStartPosition.CenterParent;
    this.Text = "Import Map Part";
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.groupBox2.ResumeLayout(false);
    this.groupBox2.PerformLayout();
    this.ResumeLayout(false);
  }
}
