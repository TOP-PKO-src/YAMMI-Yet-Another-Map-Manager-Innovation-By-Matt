using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace TileInfoView;

public class InfoViewForm : UserControl
{
  private IContainer components = new Container();
  private GroupBox groupBox1;
  private Label CoordinatesLabel;
  private GroupBox groupBox2;
  private Label TileTypeLabel;
  private Label label1;
  private Panel panel1;
  private Panel panel4;
  private Panel panel3;
  private Panel panel2;
  private Panel panel7;
  private Panel panel6;
  private Panel panel5;
  private Panel panel13;
  private Panel panel12;
  private Panel panel11;
  private Panel panel10;
  private Panel panel9;
  private Panel panel8;
  private Label label3;
  private Label raw;
  private Label label2;
  private Label txtId;
  private Label label4;
  private Label blend;
  private Label mainTextureID;
  private Label area;
  private Label label5;

  public string MapLocation
  {
    set => this.CoordinatesLabel.Text = value;
  }

  public string TileType
  {
    set => this.TileTypeLabel.Text = value;
  }

  public string TileHeight
  {
    set
    {
    }
  }

  public string RawTexture
  {
    set => this.raw.Text = value;
  }

  public string BlendTexture
  {
    set => this.blend.Text = value;
  }

  public string IdMergedTextures
  {
    set => this.txtId.Text = value;
  }

  public string MainTextureID
  {
    set => this.mainTextureID.Text = value;
  }

  public string Area
  {
    set => this.area.Text = value;
  }

  public InfoViewForm() => this.InitializeComponent();

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.groupBox1 = new GroupBox();
    this.CoordinatesLabel = new Label();
    this.area = new Label();
    this.groupBox2 = new GroupBox();
    this.TileTypeLabel = new Label();
    this.label1 = new Label();
    this.panel1 = new Panel();
    this.panel2 = new Panel();
    this.panel3 = new Panel();
    this.panel4 = new Panel();
    this.panel5 = new Panel();
    this.panel6 = new Panel();
    this.panel7 = new Panel();
    this.panel8 = new Panel();
    this.panel9 = new Panel();
    this.panel10 = new Panel();
    this.panel11 = new Panel();
    this.panel12 = new Panel();
    this.panel13 = new Panel();
    this.label2 = new Label();
    this.raw = new Label();
    this.label3 = new Label();
    this.blend = new Label();
    this.label4 = new Label();
    this.txtId = new Label();
    this.label5 = new Label();
    this.mainTextureID = new Label();
    this.groupBox1.SuspendLayout();
    this.groupBox2.SuspendLayout();
    this.panel1.SuspendLayout();
    this.panel2.SuspendLayout();
    this.panel3.SuspendLayout();
    this.panel4.SuspendLayout();
    this.panel5.SuspendLayout();
    this.panel6.SuspendLayout();
    this.panel7.SuspendLayout();
    this.panel8.SuspendLayout();
    this.panel9.SuspendLayout();
    this.panel10.SuspendLayout();
    this.panel11.SuspendLayout();
    this.panel12.SuspendLayout();
    this.panel13.SuspendLayout();
    this.SuspendLayout();
    this.groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.groupBox1.Controls.Add((Control) this.area);
    this.groupBox1.Location = new Point(6, 6);
    this.groupBox1.Name = "groupBox1";
    this.groupBox1.Size = new Size(278, 39);
    this.groupBox1.TabIndex = 0;
    this.groupBox1.TabStop = false;
    this.groupBox1.Text = "Area";
    this.CoordinatesLabel.Anchor = AnchorStyles.Left;
    this.CoordinatesLabel.AutoSize = true;
    this.CoordinatesLabel.ForeColor = SystemColors.ActiveCaption;
    this.CoordinatesLabel.Location = new Point(10, 16 /*0x10*/);
    this.CoordinatesLabel.Name = "CoordinatesLabel";
    this.CoordinatesLabel.TabIndex = 0;
    this.CoordinatesLabel.Text = "~ Not in map ~";
    this.groupBox2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
    this.groupBox2.Controls.Add((Control) this.CoordinatesLabel);
    this.groupBox2.Location = new Point(6, 60);
    this.groupBox2.Name = "groupBox1";
    this.groupBox2.Size = new Size(278, 39);
    this.groupBox2.TabIndex = 0;
    this.groupBox2.TabStop = false;
    this.groupBox2.Text = "Location";
    this.TileTypeLabel.AutoSize = true;
    this.TileTypeLabel.ForeColor = SystemColors.ActiveCaption;
    this.TileTypeLabel.Location = new Point(49, 16 /*0x10*/);
    this.TileTypeLabel.Name = "TileTypeLabel";
    this.TileTypeLabel.Size = new Size(88, 13);
    this.TileTypeLabel.TabIndex = 1;
    this.TileTypeLabel.Text = "";
    this.area.Anchor = AnchorStyles.Left;
    this.area.AutoSize = true;
    this.area.ForeColor = SystemColors.ActiveCaption;
    this.area.Location = new Point(10, 16 /*0x10*/);
    this.area.Name = "TileAreaLabel";
    this.area.TabIndex = 1;
    this.area.Text = "~ Not in map ~";
    this.label1.AutoSize = true;
    this.label1.Location = new Point(6, 16 /*0x10*/);
    this.label1.Name = "label1";
    this.label1.Size = new Size(37, 13);
    this.label1.TabIndex = 0;
    this.label1.Text = "Type: ";
    this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.panel1.BorderStyle = BorderStyle.FixedSingle;
    this.panel1.Controls.Add((Control) this.panel4);
    this.panel1.Controls.Add((Control) this.panel3);
    this.panel1.Controls.Add((Control) this.panel2);
    this.panel1.Location = new Point(12, 21);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(254, 220);
    this.panel1.TabIndex = 0;
    this.panel2.BorderStyle = BorderStyle.FixedSingle;
    this.panel2.Controls.Add((Control) this.panel7);
    this.panel2.Controls.Add((Control) this.panel6);
    this.panel2.Controls.Add((Control) this.panel5);
    this.panel2.Dock = DockStyle.Top;
    this.panel2.Location = new Point(0, 0);
    this.panel2.Name = "panel2";
    this.panel2.Size = new Size(252, 37);
    this.panel2.TabIndex = 0;
    this.panel3.BorderStyle = BorderStyle.FixedSingle;
    this.panel3.Controls.Add((Control) this.panel10);
    this.panel3.Controls.Add((Control) this.panel9);
    this.panel3.Controls.Add((Control) this.panel8);
    this.panel3.Dock = DockStyle.Bottom;
    this.panel3.Location = new Point(0, 179);
    this.panel3.Name = "panel3";
    this.panel3.Size = new Size(252, 39);
    this.panel3.TabIndex = 1;
    this.panel4.BorderStyle = BorderStyle.FixedSingle;
    this.panel4.Controls.Add((Control) this.panel13);
    this.panel4.Controls.Add((Control) this.panel12);
    this.panel4.Controls.Add((Control) this.panel11);
    this.panel4.Dock = DockStyle.Fill;
    this.panel4.Location = new Point(0, 37);
    this.panel4.Name = "panel4";
    this.panel4.Size = new Size(252, 142);
    this.panel4.TabIndex = 2;
    this.panel13.Controls.Add((Control) this.mainTextureID);
    this.panel13.Controls.Add((Control) this.label5);
    this.panel13.Controls.Add((Control) this.txtId);
    this.panel13.Controls.Add((Control) this.label4);
    this.panel13.Controls.Add((Control) this.blend);
    this.panel13.Controls.Add((Control) this.label3);
    this.panel13.Controls.Add((Control) this.raw);
    this.panel13.Controls.Add((Control) this.label2);
    this.panel13.Dock = DockStyle.Fill;
    this.panel13.Location = new Point(48 /*0x30*/, 0);
    this.panel13.Name = "panel13";
    this.panel13.Size = new Size(148, 140);
    this.panel13.TabIndex = 2;
    this.label2.AutoSize = true;
    this.label2.Location = new Point(3, 16 /*0x10*/);
    this.label2.Name = "label2";
    this.label2.Size = new Size(30, 13);
    this.label2.TabIndex = 0;
    this.label2.Text = "raw=";
    this.raw.AutoSize = true;
    this.raw.Location = new Point(31 /*0x1F*/, 16 /*0x10*/);
    this.raw.Name = "raw";
    this.raw.Size = new Size(35, 13);
    this.raw.TabIndex = 1;
    this.raw.Text = "label3";
    this.label3.AutoSize = true;
    this.label3.Location = new Point(3, 29);
    this.label3.Name = "label3";
    this.label3.Size = new Size(39, 13);
    this.label3.TabIndex = 2;
    this.label3.Text = "blend=";
    this.blend.AutoSize = true;
    this.blend.Location = new Point(26, 42);
    this.blend.Name = "blend";
    this.blend.Size = new Size(35, 13);
    this.blend.TabIndex = 3;
    this.blend.Text = "label4";
    this.label4.AutoSize = true;
    this.label4.Location = new Point(3, 66);
    this.label4.Name = "label4";
    this.label4.Size = new Size(66, 13);
    this.label4.TabIndex = 4;
    this.label4.Text = "Merged Ids=";
    this.txtId.AutoSize = true;
    this.txtId.Location = new Point(26, 79);
    this.txtId.Name = "txtId";
    this.txtId.Size = new Size(35, 13);
    this.txtId.TabIndex = 5;
    this.txtId.Text = "label5";
    this.label5.AutoSize = true;
    this.label5.Location = new Point(1, 3);
    this.label5.Name = "label5";
    this.label5.Size = new Size(49, 13);
    this.label5.TabIndex = 6;
    this.label5.Text = "Texture=";
    this.mainTextureID.AutoSize = true;
    this.mainTextureID.Location = new Point(52, 3);
    this.mainTextureID.Name = "mainTextureID";
    this.mainTextureID.Size = new Size(35, 13);
    this.mainTextureID.TabIndex = 7;
    this.mainTextureID.Text = "label6";
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.Controls.Add((Control) this.groupBox2);
    this.Controls.Add((Control) this.groupBox1);
    this.Name = nameof (InfoViewForm);
    this.Size = new Size(295, 372);
    this.groupBox1.ResumeLayout(false);
    this.groupBox1.PerformLayout();
    this.groupBox2.ResumeLayout(false);
    this.groupBox2.PerformLayout();
    this.panel1.ResumeLayout(false);
    this.panel2.ResumeLayout(false);
    this.panel3.ResumeLayout(false);
    this.panel4.ResumeLayout(false);
    this.panel5.ResumeLayout(false);
    this.panel5.PerformLayout();
    this.panel6.ResumeLayout(false);
    this.panel6.PerformLayout();
    this.panel7.ResumeLayout(false);
    this.panel7.PerformLayout();
    this.panel8.ResumeLayout(false);
    this.panel8.PerformLayout();
    this.panel9.ResumeLayout(false);
    this.panel9.PerformLayout();
    this.panel10.ResumeLayout(false);
    this.panel10.PerformLayout();
    this.panel11.ResumeLayout(false);
    this.panel11.PerformLayout();
    this.panel12.ResumeLayout(false);
    this.panel12.PerformLayout();
    this.panel13.ResumeLayout(false);
    this.panel13.PerformLayout();
    this.ResumeLayout(false);
  }
}
