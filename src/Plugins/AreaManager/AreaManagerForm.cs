using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#nullable disable
namespace AreaManager;

public class AreaManagerForm : Form
{
  private IContainer components = new Container();
  private Button CloseButton;
  private Panel panel1;
  private TabControl tabControl1;
  private TabPage Tab_AreaType;
  private SplitContainer splitContainer1;
  private TreeView AreaTypes_Tree;
  private TabPage Tab_Areas;
  private ContextMenuStrip AreaGroupContextMenu;
  private ToolStripMenuItem toolStripMenuItem1;
  private ToolStripMenuItem toolStripMenuItem2;
  private ToolStripSeparator toolStripSeparator2;

  public AreaManagerForm() => this.InitializeComponent();

  private void CloseButton_Click(object sender, EventArgs e) => this.Close();

  private void AreaTypes_Tree_AfterSelect(object sender, TreeViewEventArgs e)
  {
  }

  private void AreaTypes_Tree_MouseUp(object sender, MouseEventArgs e)
  {
    if (e.Button != MouseButtons.Right)
      return;
    Point point = new Point(e.X, e.Y);
    this.AreaTypes_Tree.SelectedNode = this.AreaTypes_Tree.GetNodeAt(point);
    if (this.AreaTypes_Tree.SelectedNode == null || this.AreaTypes_Tree.SelectedNode.Tag == null || !this.AreaTypes_Tree.SelectedNode.Tag.Equals((object) "AreaGroup"))
      return;
    this.AreaGroupContextMenu.Items[0].Text = this.AreaTypes_Tree.SelectedNode.Text;
    this.AreaGroupContextMenu.Show((Control) this.AreaTypes_Tree, point);
  }

  private void AreaGroupContextMenu_Opening(object sender, CancelEventArgs e)
  {
    Console.Out.WriteLine(">>> " + sender.GetType().Name);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.components = (IContainer) new System.ComponentModel.Container();
    TreeNode treeNode1 = new TreeNode("Shared Types");
    TreeNode treeNode2 = new TreeNode("Map Types");
    TreeNode treeNode3 = new TreeNode("All    ", new TreeNode[2]
    {
      treeNode1,
      treeNode2
    });
    this.CloseButton = new Button();
    this.panel1 = new Panel();
    this.tabControl1 = new TabControl();
    this.Tab_AreaType = new TabPage();
    this.splitContainer1 = new SplitContainer();
    this.AreaTypes_Tree = new TreeView();
    this.Tab_Areas = new TabPage();
    this.AreaGroupContextMenu = new ContextMenuStrip(this.components);
    this.toolStripMenuItem1 = new ToolStripMenuItem();
    this.toolStripSeparator2 = new ToolStripSeparator();
    this.toolStripMenuItem2 = new ToolStripMenuItem();
    this.panel1.SuspendLayout();
    this.tabControl1.SuspendLayout();
    this.Tab_AreaType.SuspendLayout();
    this.splitContainer1.Panel1.SuspendLayout();
    this.splitContainer1.SuspendLayout();
    this.AreaGroupContextMenu.SuspendLayout();
    this.SuspendLayout();
    this.CloseButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
    this.CloseButton.DialogResult = DialogResult.Cancel;
    this.CloseButton.Location = new Point(531, 449);
    this.CloseButton.Name = "CloseButton";
    this.CloseButton.Size = new Size(75, 23);
    this.CloseButton.TabIndex = 4;
    this.CloseButton.Text = "Close";
    this.CloseButton.UseVisualStyleBackColor = true;
    this.CloseButton.Click += new EventHandler(this.CloseButton_Click);
    this.panel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
    this.panel1.Controls.Add((Control) this.tabControl1);
    this.panel1.Location = new Point(12, 12);
    this.panel1.Name = "panel1";
    this.panel1.Size = new Size(593, 428);
    this.panel1.TabIndex = 5;
    this.tabControl1.Controls.Add((Control) this.Tab_AreaType);
    this.tabControl1.Controls.Add((Control) this.Tab_Areas);
    this.tabControl1.Dock = DockStyle.Fill;
    this.tabControl1.Location = new Point(0, 0);
    this.tabControl1.Name = "tabControl1";
    this.tabControl1.SelectedIndex = 0;
    this.tabControl1.Size = new Size(593, 428);
    this.tabControl1.TabIndex = 0;
    this.Tab_AreaType.Controls.Add((Control) this.splitContainer1);
    this.Tab_AreaType.Location = new Point(4, 22);
    this.Tab_AreaType.Name = "Tab_AreaType";
    this.Tab_AreaType.Padding = new Padding(3);
    this.Tab_AreaType.Size = new Size(585, 402);
    this.Tab_AreaType.TabIndex = 0;
    this.Tab_AreaType.Text = "Area Types";
    this.Tab_AreaType.UseVisualStyleBackColor = true;
    this.splitContainer1.Dock = DockStyle.Fill;
    this.splitContainer1.Location = new Point(3, 3);
    this.splitContainer1.Name = "splitContainer1";
    this.splitContainer1.Panel1.Controls.Add((Control) this.AreaTypes_Tree);
    this.splitContainer1.Size = new Size(579, 396);
    this.splitContainer1.SplitterDistance = 199;
    this.splitContainer1.TabIndex = 0;
    this.AreaTypes_Tree.Dock = DockStyle.Fill;
    this.AreaTypes_Tree.Location = new Point(0, 0);
    this.AreaTypes_Tree.Name = "AreaTypes_Tree";
    treeNode1.Name = "Node2";
    treeNode1.NodeFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
    treeNode1.Tag = (object) "AreaGroup";
    treeNode1.Text = "Shared Types";
    treeNode2.Name = "Node1";
    treeNode2.NodeFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
    treeNode2.Tag = (object) "AreaGroup";
    treeNode2.Text = "Map Types";
    treeNode3.Name = "Node0";
    treeNode3.NodeFont = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
    treeNode3.Tag = (object) "Root";
    treeNode3.Text = "All    ";
    this.AreaTypes_Tree.Nodes.AddRange(new TreeNode[1]
    {
      treeNode3
    });
    this.AreaTypes_Tree.Size = new Size(199, 396);
    this.AreaTypes_Tree.TabIndex = 0;
    this.AreaTypes_Tree.AfterSelect += new TreeViewEventHandler(this.AreaTypes_Tree_AfterSelect);
    this.AreaTypes_Tree.MouseUp += new MouseEventHandler(this.AreaTypes_Tree_MouseUp);
    this.Tab_Areas.Location = new Point(4, 22);
    this.Tab_Areas.Name = "Tab_Areas";
    this.Tab_Areas.Padding = new Padding(3);
    this.Tab_Areas.Size = new Size(585, 402);
    this.Tab_Areas.TabIndex = 1;
    this.Tab_Areas.Text = "Areas";
    this.Tab_Areas.UseVisualStyleBackColor = true;
    this.AreaGroupContextMenu.Items.AddRange(new ToolStripItem[3]
    {
      (ToolStripItem) this.toolStripMenuItem1,
      (ToolStripItem) this.toolStripSeparator2,
      (ToolStripItem) this.toolStripMenuItem2
    });
    this.AreaGroupContextMenu.Name = "AreaGroupContextMenu";
    this.AreaGroupContextMenu.RenderMode = ToolStripRenderMode.Professional;
    this.AreaGroupContextMenu.ShowImageMargin = false;
    this.AreaGroupContextMenu.Size = new Size(176 /*0xB0*/, 76);
    this.AreaGroupContextMenu.Opening += new CancelEventHandler(this.AreaGroupContextMenu_Opening);
    this.toolStripMenuItem1.AccessibleRole = AccessibleRole.TitleBar;
    this.toolStripMenuItem1.BackColor = Color.White;
    this.toolStripMenuItem1.BackgroundImageLayout = ImageLayout.None;
    this.toolStripMenuItem1.DisplayStyle = ToolStripItemDisplayStyle.Text;
    this.toolStripMenuItem1.Enabled = false;
    this.toolStripMenuItem1.Font = new Font("Tahoma", 8.25f, FontStyle.Bold, GraphicsUnit.Point, (byte) 0);
    this.toolStripMenuItem1.ForeColor = SystemColors.ControlText;
    this.toolStripMenuItem1.Name = "toolStripMenuItem1";
    this.toolStripMenuItem1.Size = new Size(175, 22);
    this.toolStripMenuItem1.Text = "<Dynamic heading>";
    this.toolStripSeparator2.Name = "toolStripSeparator2";
    this.toolStripSeparator2.Size = new Size(172, 6);
    this.toolStripMenuItem2.Name = "toolStripMenuItem2";
    this.toolStripMenuItem2.Size = new Size(175, 22);
    this.toolStripMenuItem2.Text = "Create Area Type";
    this.toolStripMenuItem2.TextAlign = ContentAlignment.MiddleLeft;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(618, 484);
    this.ControlBox = false;
    this.Controls.Add((Control) this.panel1);
    this.Controls.Add((Control) this.CloseButton);
    this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
    this.Name = nameof (AreaManagerForm);
    this.ShowIcon = false;
    this.ShowInTaskbar = false;
    this.Text = nameof (AreaManagerForm);
    this.TopMost = true;
    this.panel1.ResumeLayout(false);
    this.tabControl1.ResumeLayout(false);
    this.Tab_AreaType.ResumeLayout(false);
    this.splitContainer1.Panel1.ResumeLayout(false);
    this.splitContainer1.ResumeLayout(false);
    this.AreaGroupContextMenu.ResumeLayout(false);
    this.ResumeLayout(false);
  }
}
