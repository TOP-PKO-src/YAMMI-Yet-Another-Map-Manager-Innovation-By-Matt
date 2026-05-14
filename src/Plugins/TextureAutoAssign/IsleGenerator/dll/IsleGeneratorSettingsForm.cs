using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace IsleGenerator.dll;

public class IsleGeneratorSettingsForm : Form
{
  private IContainer components = new Container();
  private GroupBox groupBox1;
  private Label label1;
  private GroupBox groupBox2;
  private Label label2;
  private ComboBox HeightInput;
  private ComboBox WidthInput;
  private TrackBar LandCoverageInput;
  private Label label3;
  private TrackBar DensityInput;
  private Label label4;
  private TrackBar CenteringInput;
  private Label label5;
  private TrackBar SmoothingInput;
  private Label label6;
  private Button CloseButton;
  private Button GenerateButton;
  private NumericUpDown StartPointInput;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.groupBox1 = new GroupBox();
    this.HeightInput = new ComboBox();
    this.WidthInput = new ComboBox();
    this.label1 = new Label();
    this.groupBox2 = new GroupBox();
    this.SmoothingInput = new TrackBar();
    this.label6 = new Label();
    this.CenteringInput = new TrackBar();
    this.label5 = new Label();
    this.DensityInput = new TrackBar();
    this.label4 = new Label();
    this.LandCoverageInput = new TrackBar();
    this.label3 = new Label();
    this.StartPointInput = new NumericUpDown();
    this.label2 = new Label();
    this.CloseButton = new Button();
    this.GenerateButton = new Button();
    this.groupBox1.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.SmoothingInput.BeginInit();
    this.CenteringInput.BeginInit();
    this.DensityInput.BeginInit();
    this.LandCoverageInput.BeginInit();
    this.StartPointInput.BeginInit();
    this.SuspendLayout();
    this.groupBox1.Controls.Add((Control) this.HeightInput);
    this.groupBox1.Controls.Add((Control) this.WidthInput);
    this.groupBox1.Controls.Add((Control) this.label1);
    this.groupBox1.Location = new Point(3, 12);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(245, 52);
    this.groupBox1.TabIndex = 1;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Map Size";
    this.HeightInput.FormattingEnabled = true;
    this.HeightInput.Items.AddRange(new object[7]
    {
      (object) "32",
      (object) "64",
      (object) "128",
      (object) "192",
      (object) "256",
      (object) "512",
      (object) "1024"
    });
    this.HeightInput.Location = new Point(136, 19);
    this.HeightInput.Name = "HeightInput";
    this.HeightInput.Size = new Size(93, 21);
    this.HeightInput.TabIndex = 2;
    this.HeightInput.Text = "128";
    this.WidthInput.FormattingEnabled = true;
    this.WidthInput.Items.AddRange(new object[7]
    {
      (object) "32",
      (object) "64",
      (object) "128",
      (object) "192",
      (object) "256",
      (object) "512",
      (object) "1024"
    });
    this.WidthInput.Location = new Point(17, 19);
    this.WidthInput.Name = "WidthInput";
    this.WidthInput.Size = new Size(93, 21);
    this.WidthInput.TabIndex = 1;
    this.WidthInput.Text = "128";
    this.label1.AutoSize = true;
    this.label1.Location = new Point(116, 22);
    this.label1.Name = "label1";
    this.label1.Size = new Size(14, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "X";
    this.groupBox2.Controls.Add((Control) this.SmoothingInput);
    this.groupBox2.Controls.Add((Control) this.label6);
    this.groupBox2.Controls.Add((Control) this.CenteringInput);
    this.groupBox2.Controls.Add((Control) this.label5);
    this.groupBox2.Controls.Add((Control) this.DensityInput);
    this.groupBox2.Controls.Add((Control) this.label4);
    this.groupBox2.Controls.Add((Control) this.LandCoverageInput);
    this.groupBox2.Controls.Add((Control) this.label3);
    this.groupBox2.Controls.Add((Control) this.StartPointInput);
    this.groupBox2.Controls.Add((Control) this.label2);
    this.groupBox2.Location = new Point(4, 71);
    this.groupBox2.Name = "groupBox2";
    this.groupBox2.Size = new Size(243, 206);
    this.groupBox2.TabIndex = 2;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Settings";
    this.SmoothingInput.Location = new Point(118, 162);
    this.SmoothingInput.Name = "SmoothingInput";
    this.SmoothingInput.Size = new Size(110, 42);
    this.SmoothingInput.TabIndex = 9;
    this.SmoothingInput.Value = 3;
    this.label6.AutoSize = true;
    this.label6.Location = new Point(8, 168);
    this.label6.Name = "label6";
    this.label6.Size = new Size(57, 13);
    this.label6.TabIndex = 8;
    this.label6.Text = "Smoothing";
    this.CenteringInput.Location = new Point(119, 126);
    this.CenteringInput.Maximum = 100;
    this.CenteringInput.Name = "CenteringInput";
    this.CenteringInput.Size = new Size(110, 42);
    this.CenteringInput.TabIndex = 7;
    this.CenteringInput.TickFrequency = 25;
    this.CenteringInput.Value = 50;
    this.label5.AutoSize = true;
    this.label5.Location = new Point(9, 132);
    this.label5.Name = "label5";
    this.label5.Size = new Size(52, 13);
    this.label5.TabIndex = 6;
    this.label5.Text = "Centering";
    this.DensityInput.Location = new Point(118, 91);
    this.DensityInput.Maximum = 100;
    this.DensityInput.Name = "DensityInput";
    this.DensityInput.Size = new Size(110, 42);
    this.DensityInput.TabIndex = 5;
    this.DensityInput.TickFrequency = 25;
    this.DensityInput.Value = 30;
    this.label4.AutoSize = true;
    this.label4.Location = new Point(8, 97);
    this.label4.Name = "label4";
    this.label4.Size = new Size(42, 13);
    this.label4.TabIndex = 4;
    this.label4.Text = "Density";
    this.LandCoverageInput.Location = new Point(118, 54);
    this.LandCoverageInput.Maximum = 100;
    this.LandCoverageInput.Name = "LandCoverageInput";
    this.LandCoverageInput.Size = new Size(110, 42);
    this.LandCoverageInput.TabIndex = 3;
    this.LandCoverageInput.TickFrequency = 25;
    this.LandCoverageInput.Value = 30;
    this.label3.AutoSize = true;
    this.label3.Location = new Point(8, 60);
    this.label3.Name = "label3";
    this.label3.Size = new Size(80 /*0x50*/, 13);
    this.label3.TabIndex = 2;
    this.label3.Text = "Land Coverage";
    this.StartPointInput.Location = new Point(119, 28);
    this.StartPointInput.Minimum = new Decimal(new int[4]
    {
      1,
      0,
      0,
      0
    });
    this.StartPointInput.Name = "StartPointInput";
    this.StartPointInput.Size = new Size(109, 20);
    this.StartPointInput.TabIndex = 1;
    this.StartPointInput.Value = new Decimal(new int[4]
    {
      5,
      0,
      0,
      0
    });
    this.label2.AccessibleDescription = "teste";
    this.label2.AutoSize = true;
    this.label2.Location = new Point(8, 30);
    this.label2.Name = "label2";
    this.label2.Size = new Size(60, 13);
    this.label2.TabIndex = 0;
    this.label2.Text = "Start points";
    this.CloseButton.DialogResult = DialogResult.Cancel;
    this.CloseButton.Location = new Point(157, 283);
    this.CloseButton.Name = "CloseButton";
    this.CloseButton.Size = new Size(75, 23);
    this.CloseButton.TabIndex = 3;
    this.CloseButton.Text = "Close";
    this.CloseButton.UseVisualStyleBackColor = true;
    this.CloseButton.Click += new EventHandler(this.CloseButton_Click);
    this.GenerateButton.DialogResult = DialogResult.OK;
    this.GenerateButton.Location = new Point(20, 283);
    this.GenerateButton.Name = "GenerateButton";
    this.GenerateButton.Size = new Size(75, 23);
    this.GenerateButton.TabIndex = 4;
    this.GenerateButton.Text = "Generate";
    this.GenerateButton.UseVisualStyleBackColor = true;
    this.GenerateButton.Click += new EventHandler(this.GenerateButton_Click);
    this.AcceptButton = (IButtonControl) this.GenerateButton;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.CloseButton;
    this.CausesValidation = false;
    this.ClientSize = new Size(251, 311);
    this.ControlBox = false;
    this.Controls.Add((Control) this.GenerateButton);
    this.Controls.Add((Control) this.CloseButton);
    this.Controls.Add((Control) this.groupBox2);
    this.Controls.Add((Control) this.groupBox1);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.Name = nameof (IsleGeneratorSettingsForm);
    this.ShowInTaskbar = false;
    this.StartPosition = FormStartPosition.Manual;
    this.Text = "Isle Generator";
    this.TopMost = true;
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.groupBox2.ResumeLayout(false);
    this.groupBox2.PerformLayout();
    this.SmoothingInput.EndInit();
    this.CenteringInput.EndInit();
    this.DensityInput.EndInit();
    this.LandCoverageInput.EndInit();
    this.StartPointInput.EndInit();
    this.ResumeLayout(false);
  }

  public IsleGeneratorSettingsForm() => this.InitializeComponent();

  private void CloseButton_Click(object sender, EventArgs e) => this.Close();

  public int MapWidth => int.Parse(this.WidthInput.Text);

  public int MapHeight => int.Parse(this.HeightInput.Text);

  public int StartPoints => (int) this.StartPointInput.Value;

  public float LandCoverage => (float) this.LandCoverageInput.Value / 100f;

  public int DensityFactor => this.DensityInput.Value;

  public float DistanceToEdge => (float) this.CenteringInput.Value / 100f;

  public int SmoothingRounds => this.SmoothingInput.Value;

  private void GenerateButton_Click(object sender, EventArgs e)
  {
  }
}
