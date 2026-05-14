using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace YetAnotherMapManager.UI;

public class LicenseKeyDialog : Form
{
  private IContainer components = new Container();
  private TextBox outText;
  private TextBox inText;
  private Button CloseButton;
  private Button registerButton;
  private Label label1;

  public LicenseKeyDialog(string key)
  {
    this.InitializeComponent();
    this.outText.Text = key;
  }

  private void inText_TextChanged(object sender, EventArgs e)
  {
    if (!(this.inText.Text != ""))
      return;
    this.registerButton.Enabled = true;
  }

  public string LicenseKey => this.inText.Text;

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.outText = new TextBox();
    this.inText = new TextBox();
    this.CloseButton = new Button();
    this.registerButton = new Button();
    this.label1 = new Label();
    this.SuspendLayout();
    this.outText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.outText.Location = new Point(12, 25);
    this.outText.Multiline = true;
    this.outText.Name = "outText";
    this.outText.ReadOnly = true;
    this.outText.Size = new Size(385, 97);
    this.outText.TabIndex = 0;
    this.inText.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.inText.Location = new Point(12, 128 /*0x80*/);
    this.inText.Multiline = true;
    this.inText.Name = "inText";
    this.inText.Size = new Size(385, 97);
    this.inText.TabIndex = 1;
    this.inText.TextChanged += new EventHandler(this.inText_TextChanged);
    this.CloseButton.DialogResult = DialogResult.Cancel;
    this.CloseButton.Location = new Point(322, 231);
    this.CloseButton.Name = "CloseButton";
    this.CloseButton.Size = new Size(75, 23);
    this.CloseButton.TabIndex = 2;
    this.CloseButton.Text = "Close";
    this.CloseButton.UseVisualStyleBackColor = true;
    this.registerButton.DialogResult = DialogResult.OK;
    this.registerButton.Enabled = false;
    this.registerButton.Location = new Point(241, 231);
    this.registerButton.Name = "registerButton";
    this.registerButton.Size = new Size(75, 23);
    this.registerButton.TabIndex = 3;
    this.registerButton.Text = "Register";
    this.registerButton.UseVisualStyleBackColor = true;
    this.label1.AutoSize = true;
    this.label1.Location = new Point(12, 9);
    this.label1.Name = "label1";
    this.label1.Size = new Size(320, 13);
    this.label1.TabIndex = 4;
    this.label1.Text = "Please provide the bellow key in order to be granted a license key.";
    this.AcceptButton = (IButtonControl) this.registerButton;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.CancelButton = (IButtonControl) this.CloseButton;
    this.ClientSize = new Size(409, 260);
    this.ControlBox = false;
    this.Controls.Add((Control) this.label1);
    this.Controls.Add((Control) this.registerButton);
    this.Controls.Add((Control) this.CloseButton);
    this.Controls.Add((Control) this.inText);
    this.Controls.Add((Control) this.outText);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.Name = nameof (LicenseKeyDialog);
    this.StartPosition = FormStartPosition.CenterScreen;
    this.Text = "License Key";
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
