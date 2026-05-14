using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

#nullable disable
namespace YetAnotherMapManager.UI;

public class StackedPanelContainer : UserControl
{
  public Dictionary<Button, UserControl> controlList;
  private IContainer components = new Container();
  private Panel topContainer;
  private Panel bottomContainer;
  private Panel centralContainer;

  public StackedPanelContainer()
  {
    this.InitializeComponent();
    this.controlList = new Dictionary<Button, UserControl>();
  }

  public void AddControl(string title, UserControl control)
  {
    Button key = new Button();
    key.BackColor = Color.LightBlue;
    key.Text = title;
    key.Dock = DockStyle.Top;
    control.Dock = DockStyle.Fill;
    key.Click += new EventHandler(this.headButton_Click);
    this.controlList.Add(key, control);
  }

  private void DisplayControlForButton(object sender)
  {
    this.SuspendLayout();
    this.centralContainer.Controls.Clear();
    this.centralContainer.Controls.Add((Control) this.controlList[(Button) sender]);
    this.topContainer.Controls.Clear();
    this.bottomContainer.Controls.Clear();
    bool flag = false;
    foreach (Button key in this.controlList.Keys)
    {
      if (sender == key)
        flag = true;
      if (flag)
        this.topContainer.Controls.Add((Control) key);
      else
        this.bottomContainer.Controls.Add((Control) key);
    }
    this.ResumeLayout();
  }

  private void headButton_Click(object sender, EventArgs e)
  {
    this.DisplayControlForButton((object) (Button) sender);
  }

  private void StackedPanelContainer_VisibleChanged(object sender, EventArgs e)
  {
    if (!this.Visible || this.controlList.Count <= 0)
      return;
    this.DisplayControlForButton((object) this.controlList.Keys.ElementAt<Button>(0));
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.topContainer = new Panel();
    this.bottomContainer = new Panel();
    this.centralContainer = new Panel();
    this.SuspendLayout();
    this.topContainer.AutoSize = true;
    this.topContainer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.topContainer.Dock = DockStyle.Top;
    this.topContainer.Location = new Point(0, 0);
    this.topContainer.Margin = new Padding(1);
    this.topContainer.Name = "topContainer";
    this.topContainer.Size = new Size(529, 0);
    this.topContainer.TabIndex = 0;
    this.bottomContainer.AutoSize = true;
    this.bottomContainer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.bottomContainer.Dock = DockStyle.Bottom;
    this.bottomContainer.Location = new Point(0, 388);
    this.bottomContainer.Name = "bottomContainer";
    this.bottomContainer.Size = new Size(529, 0);
    this.bottomContainer.TabIndex = 1;
    this.centralContainer.AutoScroll = true;
    this.centralContainer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    this.centralContainer.BorderStyle = BorderStyle.FixedSingle;
    this.centralContainer.Dock = DockStyle.Fill;
    this.centralContainer.Location = new Point(0, 0);
    this.centralContainer.Name = "centralContainer";
    this.centralContainer.Size = new Size(529, 388);
    this.centralContainer.TabIndex = 2;
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.AutoScroll = true;
    this.Controls.Add((Control) this.centralContainer);
    this.Controls.Add((Control) this.bottomContainer);
    this.Controls.Add((Control) this.topContainer);
    this.Name = nameof (StackedPanelContainer);
    this.Size = new Size(529, 388);
    this.VisibleChanged += new EventHandler(this.StackedPanelContainer_VisibleChanged);
    this.ResumeLayout(false);
    this.PerformLayout();
  }
}
