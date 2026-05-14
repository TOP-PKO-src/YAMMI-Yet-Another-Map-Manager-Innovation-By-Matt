using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using YetAnotherMapManager.Core;
using YetAnotherMapManager.Plugins;
using YetAnotherMapManager.Plugins.Native.Drawing;
using YetAnotherMapManager.Properties;
using YetAnotherMapManager.Rendering;
using YetAnotherMapManager.UI;

#nullable disable
namespace YetAnotherMapManager;

public class MainForm : Form
{
  private ApplicationControler coreApp;
  private int fixedToolFrameWidth = 250;
  private ToolFrame toolframe;
  private IDrawingTool CurrentDrawingTool;
  private IContainer components = new Container();
  private MenuStrip mainMenu;
  private ToolStripMenuItem blablaToolStripMenuItem;
  private ToolStripMenuItem pluginsToolStripMenuItem;
  private MapRenderer mapRenderer1;
  private ToolStripMenuItem viewsToolStripMenuItem;
  private ToolStripMenuItem toggleViewsStripMenuItem;
  private ToolStripMenuItem saveProjectToolStripMenuItem;
  private ToolStripMenuItem loadProjectToolStripMenuItem;
  private ToolStripSeparator toolStripSeparator1;
  private ToolStripMenuItem closeToolStripMenuItem;
  private ToolStripMenuItem NewProjectMenuItem;
  private ToolStripMenuItem ImportMapMenuItem;
  private ToolStripSeparator toolStripSeparator3;
  private ToolStripMenuItem ExportMapMenuItem1;
  private ToolStripMenuItem settingsToolStripMenuItem;
  private ToolStripMenuItem refreshTexturesFromGameClientToolStripMenuItem;
  private ToolStripMenuItem refreshAreasFromGameClientToolStripMenuItem;
  private ToolStrip LayerToolBar;
  private ToolStripButton PassableToolBtn;
  private ToolStripButton BlockedToolBtn;
  private ToolStripSeparator toolStripSeparator2;
  private ToolStripButton SafeToolBtn;
  private ToolStripButton UnsafeToolBtn;
  private ToolStripSeparator toolStripSeparator4;
  private ToolStripButton SeaToolBtn;
  private ToolStripButton LandToolBtn;
  private ToolStripButton BridgeToolBtn;
  private ToolStripContainer toolStripContainer1;
  private ToolStrip drawingToolToolBar;
  private ToolStripButton penSquaredButton;
  private ToolStripButton penRoundedButton;
  private ToolStripButton lineButton;
  private ToolStripButton rectangleButton;
  private ToolStripButton ellipseButton;
  private ToolStripSeparator toolStripSeparator5;
  private ToolStripLabel toolStripLabel1;
  private ToolStripComboBox PenSizeCombo;
  private ToolStripDropDownButton TextureToolStripDropDownButton;
  private ToolStripSeparator toolStripSeparator6;
  private ToolStripButton TextureStripButton;
  private ToolStripButton TextureLegendColorButton;
  private ToolStripDropDownButton AreaToolStripDropDownButton;
  private ToolStripSeparator areaStripSeparator;
  private ToolStripButton AreaStripButton;
  private ToolStripButton AreaLegendColorButton;
  private ToolStripButton HeightToolStripButton;
  private ToolStripSeparator toolStripSeparator7;
  private ToolStripComboBox HeightSpotComboBox1;
  private ToolStripMenuItem creditsToolStripMenuItem;
  private ToolStripMenuItem donateToolStripMenuItem;
  private ToolStripMenuItem AtrExporttoolStripMenuItem;
  private ToolStripMenuItem BlkExporttoolStripMenuItem;
  private ToolStripSeparator toolStripSeparator8;
  private ToolStripButton LightToolStripButton;
  private ToolStripButton LightColorPickerToolStripButton;
  private ToolStripMenuItem testToolStripMenuItem;
  private ToolStripButton toolStripButton1;
  private ToolStripSeparator toolStripSeparator9;
  private ToolStripMenuItem toolMenuEntry;
  private ToolStripMenuItem cropMapToolStripMenuItem;
  private ToolStripMenuItem importSubMapToolStripMenuItem;

  public MainForm() => this.InitializeComponent();

  private void MainForm_Load(object sender, EventArgs e)
  {
    this.coreApp = new ApplicationControler(this.mapRenderer1);
    if (!this.coreApp.Init())
    {
      int num = (int) MessageBox.Show("Application initialisation failed. Closing...");
      this.Close();
    }
    Dictionary<string, EventHandler> entryPoints = this.coreApp.EntryPoints;
    foreach (string key in entryPoints.Keys)
      this.pluginsToolStripMenuItem.DropDownItems.Add(key, (Image) null, entryPoints[key]);
    StackedPanelContainer control = new StackedPanelContainer();
    Dictionary<string, UserControl> views = this.coreApp.Views;
    foreach (string key in views.Keys)
    {
      Console.WriteLine("Adding " + key);
      control.AddControl(key, views[key]);
    }
    this.toolframe = new ToolFrame("Views", (UserControl) control);
    this.toolframe.Owner = (Form) this;
    int width = Screen.PrimaryScreen.Bounds.Width;
    this.toolframe.Width = this.fixedToolFrameWidth;
    this.toolframe.Location = new Point(width - this.toolframe.Width - 16 /*0x10*/, 68);
    foreach (TextureDef texture in this.coreApp.TextureList)
      this.TextureToolStripDropDownButton.DropDownItems.Add((ToolStripItem) new TextureToolStripItem(texture));
    this.AreaToolStripDropDownButton.DropDownItems.Add((ToolStripItem) new AreaToolStripItem(new AreaDef()
    {
      Id = 0,
      Name = "*** Erase ***"
    }));
    foreach (AreaDef area in this.coreApp.AreaList)
      this.AreaToolStripDropDownButton.DropDownItems.Add((ToolStripItem) new AreaToolStripItem(area));
    this.coreApp.MapLoaded += new ApplicationControler.MapLoadedHandler(this.coreApp_MapLoaded);
  }

  private void coreApp_MapLoaded(object sender, EventArgs args)
  {
    this.toolframe.Show();
    this.enableMapRelatedMenuEntries();
    this.Focus();
  }

  private void enableMapRelatedMenuEntries()
  {
    this.LayerToolBar.Enabled = true;
    this.toggleViewsStripMenuItem.Enabled = true;
    this.saveProjectToolStripMenuItem.Enabled = true;
    this.ExportMapMenuItem1.Enabled = true;
    this.AtrExporttoolStripMenuItem.Enabled = true;
    this.BlkExporttoolStripMenuItem.Enabled = true;
    this.toolMenuEntry.Enabled = true;
  }

  private void hideToolFrames()
  {
    this.toolframe.Hide();
    this.Focus();
  }

  private void showToolFrames()
  {
    this.toolframe.Show();
    this.Focus();
  }

  private void hideAllToolStripMenuItem_Click(object sender, EventArgs e) => this.hideToolFrames();

  private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
  {
    SaveFileDialog saveFileDialog = new SaveFileDialog();
    saveFileDialog.AddExtension = true;
    saveFileDialog.AutoUpgradeEnabled = true;
    saveFileDialog.DefaultExt = "Gemini Project Map|*.gmp";
    saveFileDialog.Filter = "Gemini Project Map|*.gmp";
    if (saveFileDialog.ShowDialog() == DialogResult.OK)
    {
      this.coreApp.SaveProjectToDisk(saveFileDialog.FileName);
      int num = (int) MessageBox.Show("Project saved to " + saveFileDialog.FileName);
    }
    saveFileDialog.Dispose();
  }

  private void loadProjectToolStripMenuItem_Click(object sender, EventArgs e)
  {
    OpenFileDialog openFileDialog = new OpenFileDialog();
    openFileDialog.AddExtension = true;
    openFileDialog.AutoUpgradeEnabled = true;
    openFileDialog.CheckFileExists = true;
    openFileDialog.CheckPathExists = true;
    openFileDialog.Filter = "Gemini Map Project|*.gmp";
    if (openFileDialog.ShowDialog() == DialogResult.OK)
      this.coreApp.LoadProjectFromDisk(openFileDialog.FileName);
    openFileDialog.Dispose();
  }

  private void closeToolStripMenuItem_Click(object sender, EventArgs e)
  {
    if (!this.coreApp.AllDataSaved && MessageBox.Show("Current work will be lost !", "Warning", MessageBoxButtons.OKCancel) != DialogResult.OK)
      return;
    this.Close();
  }

  private void NewProjectMenuItem_Click(object sender, EventArgs e)
  {
    if (!this.coreApp.AllDataSaved && MessageBox.Show("Current work will be lost !", "Warning", MessageBoxButtons.OKCancel) != DialogResult.OK)
      return;
    using (NewMapInputForm newMapInputForm = new NewMapInputForm())
    {
      if (newMapInputForm.ShowDialog() != DialogResult.OK)
        return;
      this.coreApp.CreateMap(newMapInputForm.MapWidth, newMapInputForm.MapHeight);
    }
  }

  private void ImportMapMenuItem_Click(object sender, EventArgs e)
  {
    OpenFileDialog openFileDialog = new OpenFileDialog();
    openFileDialog.AddExtension = true;
    openFileDialog.AutoUpgradeEnabled = true;
    openFileDialog.CheckFileExists = true;
    openFileDialog.CheckPathExists = true;
    openFileDialog.Filter = "Top/Pko Map File|*.map";
    if (openFileDialog.ShowDialog() == DialogResult.OK)
      this.coreApp.ImportNativeMapFile(openFileDialog.FileName);
    openFileDialog.Dispose();
  }

  private void ExportMapMenuItem1_Click(object sender, EventArgs e)
  {
    SaveFileDialog saveFileDialog = new SaveFileDialog();
    saveFileDialog.AddExtension = true;
    saveFileDialog.AutoUpgradeEnabled = true;
    saveFileDialog.DefaultExt = "Top/PKO Map File|*.map";
    saveFileDialog.Filter = "Top/PKO Map File|*.map";
    if (saveFileDialog.ShowDialog() == DialogResult.OK)
    {
      this.coreApp.ExportNativeMapFile(saveFileDialog.FileName);
      int num = (int) MessageBox.Show("Map saved as " + saveFileDialog.FileName);
    }
    saveFileDialog.Dispose();
  }

  private void AtrExporttoolStripMenuItem_Click(object sender, EventArgs e)
  {
    SaveFileDialog saveFileDialog = new SaveFileDialog();
    saveFileDialog.AddExtension = true;
    saveFileDialog.AutoUpgradeEnabled = true;
    saveFileDialog.DefaultExt = "Top/PKO Atr File|*.atr";
    saveFileDialog.Filter = "Top/PKO Atr File|*.atr";
    if (saveFileDialog.ShowDialog() == DialogResult.OK)
    {
      this.coreApp.ExportNativeAtrFile(saveFileDialog.FileName);
      int num = (int) MessageBox.Show("Map Attribute file saved as " + saveFileDialog.FileName);
    }
    saveFileDialog.Dispose();
  }

  private void BlkExporttoolStripMenuItem_Click(object sender, EventArgs e)
  {
    SaveFileDialog saveFileDialog = new SaveFileDialog();
    saveFileDialog.AddExtension = true;
    saveFileDialog.AutoUpgradeEnabled = true;
    saveFileDialog.DefaultExt = "Top/PKO Blk File|*.blk";
    saveFileDialog.Filter = "Top/PKO Blk File|*.blk";
    if (saveFileDialog.ShowDialog() == DialogResult.OK)
    {
      this.coreApp.ExportNativeBlkFile(saveFileDialog.FileName);
      int num = (int) MessageBox.Show("Map blocking file saved as " + saveFileDialog.FileName);
    }
    saveFileDialog.Dispose();
  }

  private void refreshTexturesFromGameClientToolStripMenuItem_Click(object sender, EventArgs e)
  {
    this.coreApp.RefreshTexturesFromGameClient();
  }

  private void refreshAreasFromGameClientToolStripMenuItem_Click(object sender, EventArgs e)
  {
    this.coreApp.RefreshAreaListFromGameClient();
  }

  private void toggleViewsStripMenuItem_Click(object sender, EventArgs e)
  {
    if (this.toolframe.Visible)
    {
      this.toolframe.Visible = false;
      ((ToolStripItem) sender).Text = "Show views";
    }
    else
    {
      this.toolframe.Visible = true;
      ((ToolStripItem) sender).Text = "Hide views";
    }
  }

  private void BlockedToolBtn_Click(object sender, EventArgs e)
  {
    SolidDrawingTool DrawingTool = new SolidDrawingTool(this.coreApp.GetMapForEdition());
    DrawingTool.SetSolid();
    this.SetDrawingTool((IDrawingTool) DrawingTool);
    this.UpdateDrawingToolBarStatus(sender);
  }

  private void UpdateDrawingToolBarStatus(object sender)
  {
    for (int index = 0; index < this.LayerToolBar.Items.Count; ++index)
    {
      if (this.LayerToolBar.Items[index] is ToolStripButton)
        ((ToolStripButton) this.LayerToolBar.Items[index]).Checked = false;
    }
    ((ToolStripButton) sender).Checked = true;
    this.TextureToolStripDropDownButton.Visible = false;
    this.TextureLegendColorButton.Visible = false;
    this.AreaToolStripDropDownButton.Visible = false;
    this.AreaLegendColorButton.Visible = false;
    this.HeightSpotComboBox1.Visible = false;
    this.LightColorPickerToolStripButton.Visible = false;
  }

  private void SetDrawingTool(IDrawingTool DrawingTool)
  {
    if (DrawingTool == null)
    {
      this.mapRenderer1.EndEditMode();
      this.mapRenderer1.MapDrawnAtTileLevel -= new MapRenderer.MapDrawnAtTileLevelHandler(this.ApplyTileMapDrawing);
    }
    else
    {
      this.mapRenderer1.MapDrawnAtTileLevel += new MapRenderer.MapDrawnAtTileLevelHandler(this.ApplyTileMapDrawing);
      this.mapRenderer1.StartEditMode();
    }
    this.CurrentDrawingTool = DrawingTool;
  }

  private void ApplyTileMapDrawing(List<Point> list)
  {
    if (this.CurrentDrawingTool == null)
      return;
    this.coreApp.ApplyTileMapDrawing(this.CurrentDrawingTool, list);
  }

  private void PassableToolBtn_Click(object sender, EventArgs e)
  {
    SolidDrawingTool DrawingTool = new SolidDrawingTool(this.coreApp.GetMapForEdition());
    DrawingTool.SetNonSolid();
    this.SetDrawingTool((IDrawingTool) DrawingTool);
    this.UpdateDrawingToolBarStatus(sender);
  }

  private void SafeToolBtn_Click(object sender, EventArgs e)
  {
    SafeDrawingTool DrawingTool = new SafeDrawingTool(this.coreApp.GetMapForEdition());
    DrawingTool.SetSafe();
    this.SetDrawingTool((IDrawingTool) DrawingTool);
    this.UpdateDrawingToolBarStatus(sender);
  }

  private void toolStripButton1_Click(object sender, EventArgs e)
  {
    PvpDrawingTool DrawingTool = new PvpDrawingTool(this.coreApp.GetMapForEdition());
    DrawingTool.SetPvp();
    this.SetDrawingTool((IDrawingTool) DrawingTool);
    this.UpdateDrawingToolBarStatus(sender);
  }

  private void UnsafeToolBtn_Click(object sender, EventArgs e)
  {
    SafeDrawingTool DrawingTool = new SafeDrawingTool(this.coreApp.GetMapForEdition());
    DrawingTool.SetUnsafe();
    this.SetDrawingTool((IDrawingTool) DrawingTool);
    this.UpdateDrawingToolBarStatus(sender);
  }

  private void SeaToolBtn_Click(object sender, EventArgs e)
  {
    SeaLandDrawingTool DrawingTool = new SeaLandDrawingTool(this.coreApp.GetMapForEdition());
    DrawingTool.SetSea();
    this.SetDrawingTool((IDrawingTool) DrawingTool);
    this.UpdateDrawingToolBarStatus(sender);
  }

  private void LandToolBtn_Click(object sender, EventArgs e)
  {
    SeaLandDrawingTool DrawingTool = new SeaLandDrawingTool(this.coreApp.GetMapForEdition());
    DrawingTool.SetLand();
    this.SetDrawingTool((IDrawingTool) DrawingTool);
    this.UpdateDrawingToolBarStatus(sender);
  }

  private void BridgeToolBtn_Click(object sender, EventArgs e)
  {
    SeaLandDrawingTool DrawingTool = new SeaLandDrawingTool(this.coreApp.GetMapForEdition());
    DrawingTool.SetBridge();
    this.SetDrawingTool((IDrawingTool) DrawingTool);
    this.UpdateDrawingToolBarStatus(sender);
  }

  private void TextureStripButton_Click(object sender, EventArgs e)
  {
    this.SetDrawingTool((IDrawingTool) new TextureDrawingTool(this.coreApp.GetMapForEdition()));
    this.UpdateDrawingToolBarStatus(sender);
    this.TextureToolStripDropDownButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
    this.TextureLegendColorButton.Visible = false;
    this.TextureLegendColorButton.Text = "Select a Texture";
    this.TextureToolStripDropDownButton.Visible = true;
  }

  private void AreaStripButton_Click(object sender, EventArgs e)
  {
    this.SetDrawingTool((IDrawingTool) new AreaDrawingTool(this.coreApp.GetMapForEdition()));
    this.UpdateDrawingToolBarStatus(sender);
    this.AreaToolStripDropDownButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
    this.AreaLegendColorButton.Visible = false;
    this.AreaToolStripDropDownButton.Text = "Select an Area";
    this.AreaToolStripDropDownButton.Visible = true;
  }

  private void LightToolStripButton_Click(object sender, EventArgs e)
  {
    LightColorDrawingTool DrawingTool = new LightColorDrawingTool(this.coreApp.GetMapForEdition());
    DrawingTool.SetColor(Color.Red);
    this.SetDrawingTool((IDrawingTool) DrawingTool);
    this.UpdateDrawingToolBarStatus(sender);
    this.LightColorPickerToolStripButton.Visible = true;
  }

  private void LightColorPickerToolStripButton_Click(object sender, EventArgs e)
  {
    ToolStripButton toolStripButton = (ToolStripButton) sender;
    using (ColorDialog colorDialog = new ColorDialog())
    {
      colorDialog.Color = toolStripButton.BackColor;
      if (colorDialog.ShowDialog() != DialogResult.OK)
        return;
      toolStripButton.BackColor = colorDialog.Color;
      ((LightColorDrawingTool) this.CurrentDrawingTool).SetColor(colorDialog.Color);
    }
  }

  private void HeightToolStripButton_Click(object sender, EventArgs e)
  {
    HeightSpotDrawingTool DrawingTool = new HeightSpotDrawingTool(this.coreApp.GetMapForEdition());
    DrawingTool.SetHeight((sbyte) 25);
    this.SetDrawingTool((IDrawingTool) DrawingTool);
    this.UpdateDrawingToolBarStatus(sender);
    this.HeightSpotComboBox1.Visible = true;
  }

  private void penSquaredButton_Click(object sender, EventArgs e)
  {
    this.mapRenderer1.DrawTool = MapRenderer.DRAW_SQUARED_PEN;
    for (int index = 0; index < this.drawingToolToolBar.Items.Count; ++index)
    {
      if (this.drawingToolToolBar.Items[index] is ToolStripButton)
        ((ToolStripButton) this.drawingToolToolBar.Items[index]).Checked = sender == this.drawingToolToolBar.Items[index];
    }
  }

  private void penRoundedButton_Click(object sender, EventArgs e)
  {
    this.mapRenderer1.DrawTool = MapRenderer.DRAW_ROUNDED_PEN;
    for (int index = 0; index < this.drawingToolToolBar.Items.Count; ++index)
    {
      if (this.drawingToolToolBar.Items[index] is ToolStripButton)
        ((ToolStripButton) this.drawingToolToolBar.Items[index]).Checked = sender == this.drawingToolToolBar.Items[index];
    }
  }

  private void lineButton_Click(object sender, EventArgs e)
  {
    this.mapRenderer1.DrawTool = MapRenderer.DRAW_LINE;
    for (int index = 0; index < this.drawingToolToolBar.Items.Count; ++index)
    {
      if (this.drawingToolToolBar.Items[index] is ToolStripButton)
        ((ToolStripButton) this.drawingToolToolBar.Items[index]).Checked = sender == this.drawingToolToolBar.Items[index];
    }
  }

  private void rectangleButton_Click(object sender, EventArgs e)
  {
    this.mapRenderer1.DrawTool = MapRenderer.DRAW_RECTANGLE;
    for (int index = 0; index < this.drawingToolToolBar.Items.Count; ++index)
    {
      if (this.drawingToolToolBar.Items[index] is ToolStripButton)
        ((ToolStripButton) this.drawingToolToolBar.Items[index]).Checked = sender == this.drawingToolToolBar.Items[index];
    }
  }

  private void ellipseButton_Click(object sender, EventArgs e)
  {
    this.mapRenderer1.DrawTool = MapRenderer.DRAW_ELLIPSE;
    for (int index = 0; index < this.drawingToolToolBar.Items.Count; ++index)
    {
      if (this.drawingToolToolBar.Items[index] is ToolStripButton)
        ((ToolStripButton) this.drawingToolToolBar.Items[index]).Checked = sender == this.drawingToolToolBar.Items[index];
    }
  }

  private void PenSizeCombo_TextChanged(object sender, EventArgs e)
  {
    ToolStripControlHost stripControlHost = (ToolStripControlHost) sender;
    int num;
    try
    {
      num = int.Parse(stripControlHost.Text);
    }
    catch
    {
      num = 1;
      stripControlHost.Text = "1";
    }
    this.mapRenderer1.PenSize = num;
    this.mapRenderer1.Focus();
  }

  private void TextureToolStripDropDownButton_DropDownItemClicked(
    object sender,
    ToolStripItemClickedEventArgs e)
  {
    TextureToolStripItem clickedItem = (TextureToolStripItem) e.ClickedItem;
    this.TextureToolStripDropDownButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.TextureToolStripDropDownButton.Image = (Image) clickedItem.TextureDef.Texture;
    this.TextureLegendColorButton.BackColor = clickedItem.TextureDef.RenderColor;
    this.TextureLegendColorButton.Visible = true;
    ((TextureDrawingTool) this.CurrentDrawingTool).SetTexture(clickedItem.TextureDef);
  }

  private void TextureToolStripDropDownButton_Click(object sender, EventArgs e)
  {
  }

  private void AreaToolStripDropDownButton_DropDownItemClicked(
    object sender,
    ToolStripItemClickedEventArgs e)
  {
    AreaToolStripItem clickedItem = (AreaToolStripItem) e.ClickedItem;
    this.AreaToolStripDropDownButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
    this.AreaToolStripDropDownButton.Text = clickedItem.AreaDef.Name;
    this.AreaLegendColorButton.BackColor = clickedItem.AreaDef.RenderColor;
    this.AreaLegendColorButton.Visible = true;
    ((AreaDrawingTool) this.CurrentDrawingTool).SetArea(clickedItem.AreaDef);
  }

  private void AreaToolStripDropDownButton_Click(object sender, EventArgs e)
  {
  }

  private void HeightSpotComboBox1_TextChanged(object sender, EventArgs e)
  {
    HeightSpotDrawingTool currentDrawingTool = (HeightSpotDrawingTool) this.CurrentDrawingTool;
    ToolStripControlHost stripControlHost = (ToolStripControlHost) sender;
    sbyte height;
    try
    {
      height = sbyte.Parse(stripControlHost.Text);
    }
    catch
    {
      height = (sbyte) 1;
      stripControlHost.Text = "1";
    }
    currentDrawingTool.SetHeight(height);
    this.mapRenderer1.Focus();
  }

  private void creditsToolStripMenuItem_Click(object sender, EventArgs e)
  {
    int num = (int) MessageBox.Show("I'll do that one day...");
  }

  private void donateToolStripMenuItem_Click(object sender, EventArgs e)
  {
    Process.Start($"{""}https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business={"6AUEVYHPU5SJU"}&item_name={"Donate to Matt"}&cn=Notes&no_note=1&no_shipping=2&bn=PP-DonationsBF:btn_donateCC_LG.gif:NonHosted");
  }

  private void testToolStripMenuItem_Click(object sender, EventArgs e) => this.coreApp.Test();

  private void cropMapToolStripMenuItem_Click(object sender, EventArgs e)
  {
    CropMapInputForm cropMapInputForm = new CropMapInputForm();
    if (cropMapInputForm.ShowDialog() != DialogResult.OK)
      return;
    this.coreApp.CropMap(cropMapInputForm.From_X, cropMapInputForm.From_Y, cropMapInputForm.To_X, cropMapInputForm.To_Y);
  }

  private void importSubMapToolStripMenuItem_Click(object sender, EventArgs e)
  {
    ImportMapPartInputForm mapPartInputForm = new ImportMapPartInputForm();
    if (mapPartInputForm.ShowDialog() != DialogResult.OK)
      return;
    this.coreApp.ImportSubMap(mapPartInputForm.Filename, mapPartInputForm.From_X, mapPartInputForm.From_Y, mapPartInputForm.To_X, mapPartInputForm.To_Y, mapPartInputForm.Target_X, mapPartInputForm.Target_Y);
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (MainForm));
    this.LayerToolBar = new ToolStrip();
    this.PassableToolBtn = new ToolStripButton();
    this.BlockedToolBtn = new ToolStripButton();
    this.toolStripSeparator2 = new ToolStripSeparator();
    this.SafeToolBtn = new ToolStripButton();
    this.UnsafeToolBtn = new ToolStripButton();
    this.toolStripSeparator4 = new ToolStripSeparator();
    this.toolStripButton1 = new ToolStripButton();
    this.toolStripSeparator9 = new ToolStripSeparator();
    this.SeaToolBtn = new ToolStripButton();
    this.LandToolBtn = new ToolStripButton();
    this.BridgeToolBtn = new ToolStripButton();
    this.toolStripSeparator6 = new ToolStripSeparator();
    this.TextureStripButton = new ToolStripButton();
    this.TextureToolStripDropDownButton = new ToolStripDropDownButton();
    this.TextureLegendColorButton = new ToolStripButton();
    this.areaStripSeparator = new ToolStripSeparator();
    this.AreaStripButton = new ToolStripButton();
    this.AreaToolStripDropDownButton = new ToolStripDropDownButton();
    this.AreaLegendColorButton = new ToolStripButton();
    this.toolStripSeparator7 = new ToolStripSeparator();
    this.HeightToolStripButton = new ToolStripButton();
    this.HeightSpotComboBox1 = new ToolStripComboBox();
    this.toolStripSeparator8 = new ToolStripSeparator();
    this.LightToolStripButton = new ToolStripButton();
    this.LightColorPickerToolStripButton = new ToolStripButton();
    this.mainMenu = new MenuStrip();
    this.blablaToolStripMenuItem = new ToolStripMenuItem();
    this.NewProjectMenuItem = new ToolStripMenuItem();
    this.saveProjectToolStripMenuItem = new ToolStripMenuItem();
    this.loadProjectToolStripMenuItem = new ToolStripMenuItem();
    this.toolStripSeparator1 = new ToolStripSeparator();
    this.ImportMapMenuItem = new ToolStripMenuItem();
    this.ExportMapMenuItem1 = new ToolStripMenuItem();
    this.AtrExporttoolStripMenuItem = new ToolStripMenuItem();
    this.BlkExporttoolStripMenuItem = new ToolStripMenuItem();
    this.toolStripSeparator3 = new ToolStripSeparator();
    this.closeToolStripMenuItem = new ToolStripMenuItem();
    this.pluginsToolStripMenuItem = new ToolStripMenuItem();
    this.viewsToolStripMenuItem = new ToolStripMenuItem();
    this.toggleViewsStripMenuItem = new ToolStripMenuItem();
    this.settingsToolStripMenuItem = new ToolStripMenuItem();
    this.refreshTexturesFromGameClientToolStripMenuItem = new ToolStripMenuItem();
    this.refreshAreasFromGameClientToolStripMenuItem = new ToolStripMenuItem();
    this.toolMenuEntry = new ToolStripMenuItem();
    this.cropMapToolStripMenuItem = new ToolStripMenuItem();
    this.importSubMapToolStripMenuItem = new ToolStripMenuItem();
    this.creditsToolStripMenuItem = new ToolStripMenuItem();
    this.donateToolStripMenuItem = new ToolStripMenuItem();
    this.testToolStripMenuItem = new ToolStripMenuItem();
    this.toolStripContainer1 = new ToolStripContainer();
    this.mapRenderer1 = new MapRenderer();
    this.drawingToolToolBar = new ToolStrip();
    this.penSquaredButton = new ToolStripButton();
    this.penRoundedButton = new ToolStripButton();
    this.lineButton = new ToolStripButton();
    this.rectangleButton = new ToolStripButton();
    this.ellipseButton = new ToolStripButton();
    this.toolStripSeparator5 = new ToolStripSeparator();
    this.toolStripLabel1 = new ToolStripLabel();
    this.PenSizeCombo = new ToolStripComboBox();
    this.LayerToolBar.SuspendLayout();
    this.mainMenu.SuspendLayout();
    this.toolStripContainer1.ContentPanel.SuspendLayout();
    this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
    this.toolStripContainer1.SuspendLayout();
    this.drawingToolToolBar.SuspendLayout();
    this.SuspendLayout();
    this.LayerToolBar.Dock = DockStyle.None;
    this.LayerToolBar.Enabled = false;
    this.LayerToolBar.Items.AddRange(new ToolStripItem[25]
    {
      (ToolStripItem) this.PassableToolBtn,
      (ToolStripItem) this.BlockedToolBtn,
      (ToolStripItem) this.toolStripSeparator2,
      (ToolStripItem) this.SafeToolBtn,
      (ToolStripItem) this.UnsafeToolBtn,
      (ToolStripItem) this.toolStripSeparator4,
      (ToolStripItem) this.toolStripButton1,
      (ToolStripItem) this.toolStripSeparator9,
      (ToolStripItem) this.SeaToolBtn,
      (ToolStripItem) this.LandToolBtn,
      (ToolStripItem) this.BridgeToolBtn,
      (ToolStripItem) this.toolStripSeparator6,
      (ToolStripItem) this.TextureStripButton,
      (ToolStripItem) this.TextureToolStripDropDownButton,
      (ToolStripItem) this.TextureLegendColorButton,
      (ToolStripItem) this.areaStripSeparator,
      (ToolStripItem) this.AreaStripButton,
      (ToolStripItem) this.AreaToolStripDropDownButton,
      (ToolStripItem) this.AreaLegendColorButton,
      (ToolStripItem) this.toolStripSeparator7,
      (ToolStripItem) this.HeightToolStripButton,
      (ToolStripItem) this.HeightSpotComboBox1,
      (ToolStripItem) this.toolStripSeparator8,
      (ToolStripItem) this.LightToolStripButton,
      (ToolStripItem) this.LightColorPickerToolStripButton
    });
    this.LayerToolBar.Location = new Point(3, 25);
    this.LayerToolBar.Name = "LayerToolBar";
    this.LayerToolBar.RenderMode = ToolStripRenderMode.Professional;
    this.LayerToolBar.Size = new Size(299, 25);
    this.LayerToolBar.TabIndex = 2;
    this.LayerToolBar.Text = "toolStrip1";
    this.PassableToolBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.PassableToolBtn.Image = (Image) componentResourceManager.GetObject("PassableToolBtn.Image");
    this.PassableToolBtn.ImageTransparentColor = Color.Magenta;
    this.PassableToolBtn.Name = "PassableToolBtn";
    this.PassableToolBtn.Size = new Size(23, 22);
    this.PassableToolBtn.Text = "Define a zone which can be passed through.";
    this.PassableToolBtn.Click += new EventHandler(this.PassableToolBtn_Click);
    this.BlockedToolBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.BlockedToolBtn.Image = (Image) componentResourceManager.GetObject("BlockedToolBtn.Image");
    this.BlockedToolBtn.ImageTransparentColor = Color.Magenta;
    this.BlockedToolBtn.Name = "BlockedToolBtn";
    this.BlockedToolBtn.Size = new Size(23, 22);
    this.BlockedToolBtn.Text = "Define a zone that can't be crossed.";
    this.BlockedToolBtn.Click += new EventHandler(this.BlockedToolBtn_Click);
    this.toolStripSeparator2.Name = "toolStripSeparator2";
    this.toolStripSeparator2.Size = new Size(6, 25);
    this.SafeToolBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.SafeToolBtn.Image = (Image) componentResourceManager.GetObject("SafeToolBtn.Image");
    this.SafeToolBtn.ImageTransparentColor = Color.Magenta;
    this.SafeToolBtn.Name = "SafeToolBtn";
    this.SafeToolBtn.Size = new Size(23, 22);
    this.SafeToolBtn.Text = "toolStripButton3";
    this.SafeToolBtn.ToolTipText = "Define a safe zone.";
    this.SafeToolBtn.Click += new EventHandler(this.SafeToolBtn_Click);
    this.UnsafeToolBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.UnsafeToolBtn.Image = (Image) componentResourceManager.GetObject("UnsafeToolBtn.Image");
    this.UnsafeToolBtn.ImageTransparentColor = Color.Magenta;
    this.UnsafeToolBtn.Name = "UnsafeToolBtn";
    this.UnsafeToolBtn.Size = new Size(23, 22);
    this.UnsafeToolBtn.Text = "toolStripButton4";
    this.UnsafeToolBtn.ToolTipText = "Remove a safe zone.";
    this.UnsafeToolBtn.Click += new EventHandler(this.UnsafeToolBtn_Click);
    this.toolStripSeparator4.Name = "toolStripSeparator4";
    this.toolStripSeparator4.Size = new Size(6, 25);
    this.toolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.toolStripButton1.Image = (Image) Resources.pvp1;
    this.toolStripButton1.ImageTransparentColor = Color.Magenta;
    this.toolStripButton1.Name = "toolStripButton1";
    this.toolStripButton1.Size = new Size(23, 22);
    this.toolStripButton1.Text = "Define PvP Invite area";
    this.toolStripButton1.Click += new EventHandler(this.toolStripButton1_Click);
    this.toolStripSeparator9.Name = "toolStripSeparator9";
    this.toolStripSeparator9.Size = new Size(6, 25);
    this.SeaToolBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.SeaToolBtn.Image = (Image) componentResourceManager.GetObject("SeaToolBtn.Image");
    this.SeaToolBtn.ImageTransparentColor = Color.Magenta;
    this.SeaToolBtn.Name = "SeaToolBtn";
    this.SeaToolBtn.Size = new Size(23, 22);
    this.SeaToolBtn.Text = "toolStripButton5";
    this.SeaToolBtn.ToolTipText = "Define a sea area.";
    this.SeaToolBtn.Click += new EventHandler(this.SeaToolBtn_Click);
    this.LandToolBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.LandToolBtn.Image = (Image) componentResourceManager.GetObject("LandToolBtn.Image");
    this.LandToolBtn.ImageTransparentColor = Color.Magenta;
    this.LandToolBtn.Name = "LandToolBtn";
    this.LandToolBtn.Size = new Size(23, 22);
    this.LandToolBtn.Text = "toolStripButton6";
    this.LandToolBtn.ToolTipText = "Define a land area.";
    this.LandToolBtn.Click += new EventHandler(this.LandToolBtn_Click);
    this.BridgeToolBtn.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.BridgeToolBtn.Image = (Image) componentResourceManager.GetObject("BridgeToolBtn.Image");
    this.BridgeToolBtn.ImageTransparentColor = Color.Magenta;
    this.BridgeToolBtn.Name = "BridgeToolBtn";
    this.BridgeToolBtn.Size = new Size(23, 22);
    this.BridgeToolBtn.Text = "toolStripButton7";
    this.BridgeToolBtn.ToolTipText = "Define an area considered as land and sea (bridges, docks, ...)";
    this.BridgeToolBtn.Click += new EventHandler(this.BridgeToolBtn_Click);
    this.toolStripSeparator6.Name = "toolStripSeparator6";
    this.toolStripSeparator6.Size = new Size(6, 25);
    this.TextureStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.TextureStripButton.Image = (Image) componentResourceManager.GetObject("TextureStripButton.Image");
    this.TextureStripButton.ImageTransparentColor = Color.Magenta;
    this.TextureStripButton.Name = "TextureStripButton";
    this.TextureStripButton.Size = new Size(23, 22);
    this.TextureStripButton.Text = "Texture";
    this.TextureStripButton.Click += new EventHandler(this.TextureStripButton_Click);
    this.TextureToolStripDropDownButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
    this.TextureToolStripDropDownButton.DoubleClickEnabled = true;
    this.TextureToolStripDropDownButton.Image = (Image) componentResourceManager.GetObject("TextureToolStripDropDownButton.Image");
    this.TextureToolStripDropDownButton.ImageAlign = ContentAlignment.MiddleLeft;
    this.TextureToolStripDropDownButton.ImageTransparentColor = Color.Magenta;
    this.TextureToolStripDropDownButton.Name = "TextureToolStripDropDownButton";
    this.TextureToolStripDropDownButton.Size = new Size(97, 17);
    this.TextureToolStripDropDownButton.Text = "Select a texture";
    this.TextureToolStripDropDownButton.Visible = false;
    this.TextureToolStripDropDownButton.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.TextureToolStripDropDownButton_DropDownItemClicked);
    this.TextureToolStripDropDownButton.Click += new EventHandler(this.TextureToolStripDropDownButton_Click);
    this.TextureLegendColorButton.BackColor = SystemColors.ActiveCaption;
    this.TextureLegendColorButton.DisplayStyle = ToolStripItemDisplayStyle.None;
    this.TextureLegendColorButton.Image = (Image) componentResourceManager.GetObject("TextureLegendColorButton.Image");
    this.TextureLegendColorButton.ImageTransparentColor = Color.Magenta;
    this.TextureLegendColorButton.Name = "TextureLegendColorButton";
    this.TextureLegendColorButton.Size = new Size(23, 22);
    this.TextureLegendColorButton.ToolTipText = "Associated render color";
    this.TextureLegendColorButton.Visible = false;
    this.areaStripSeparator.Name = "areaStripSeparator";
    this.areaStripSeparator.Size = new Size(6, 25);
    this.AreaStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
    this.AreaStripButton.Image = (Image) componentResourceManager.GetObject("AreaStripButton.Image");
    this.AreaStripButton.ImageTransparentColor = Color.Magenta;
    this.AreaStripButton.Name = "AreaStripButton";
    this.AreaStripButton.Size = new Size(23, 22);
    this.AreaStripButton.Text = "Area";
    this.AreaStripButton.Click += new EventHandler(this.AreaStripButton_Click);
    this.AreaToolStripDropDownButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
    this.AreaToolStripDropDownButton.DoubleClickEnabled = true;
    this.AreaToolStripDropDownButton.Image = (Image) componentResourceManager.GetObject("AreaToolStripDropDownButton.Image");
    this.AreaToolStripDropDownButton.ImageAlign = ContentAlignment.MiddleLeft;
    this.AreaToolStripDropDownButton.ImageTransparentColor = Color.Magenta;
    this.AreaToolStripDropDownButton.Name = "AreaToolStripDropDownButton";
    this.AreaToolStripDropDownButton.Size = new Size(97, 17);
    this.AreaToolStripDropDownButton.Text = "Select an area";
    this.AreaToolStripDropDownButton.Visible = false;
    this.AreaToolStripDropDownButton.DropDownItemClicked += new ToolStripItemClickedEventHandler(this.AreaToolStripDropDownButton_DropDownItemClicked);
    this.AreaToolStripDropDownButton.Click += new EventHandler(this.AreaToolStripDropDownButton_Click);
    this.AreaLegendColorButton.BackColor = SystemColors.ActiveCaption;
    this.AreaLegendColorButton.DisplayStyle = ToolStripItemDisplayStyle.None;
    this.AreaLegendColorButton.Image = (Image) componentResourceManager.GetObject("AreaLegendColorButton.Image");
    this.AreaLegendColorButton.Name = "AreaLegendColorButton";
    this.AreaLegendColorButton.Size = new Size(23, 22);
    this.AreaLegendColorButton.ToolTipText = "Associated render color";
    this.AreaLegendColorButton.Visible = false;
    this.toolStripSeparator7.Name = "toolStripSeparator7";
    this.toolStripSeparator7.Size = new Size(6, 25);
    this.HeightToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.HeightToolStripButton.Image = (Image) componentResourceManager.GetObject("HeightToolStripButton.Image");
    this.HeightToolStripButton.ImageTransparentColor = Color.Magenta;
    this.HeightToolStripButton.Name = "HeightToolStripButton";
    this.HeightToolStripButton.Size = new Size(23, 22);
    this.HeightToolStripButton.Text = "HeightToolStripButton";
    this.HeightToolStripButton.ToolTipText = "Define height spot points";
    this.HeightToolStripButton.Click += new EventHandler(this.HeightToolStripButton_Click);
    this.HeightSpotComboBox1.DropDownWidth = 25;
    this.HeightSpotComboBox1.Items.AddRange(new object[12]
    {
      (object) "-15",
      (object) "-9",
      (object) "-6",
      (object) "-3",
      (object) "0",
      (object) "5",
      (object) "10",
      (object) "15",
      (object) "25",
      (object) "40",
      (object) "65",
      (object) "105"
    });
    this.HeightSpotComboBox1.Name = "HeightSpotComboBox1";
    this.HeightSpotComboBox1.Size = new Size(121, 25);
    this.HeightSpotComboBox1.Visible = false;
    this.HeightSpotComboBox1.TextChanged += new EventHandler(this.HeightSpotComboBox1_TextChanged);
    this.toolStripSeparator8.Name = "toolStripSeparator8";
    this.toolStripSeparator8.Size = new Size(6, 25);
    this.LightToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.LightToolStripButton.Image = (Image) componentResourceManager.GetObject("LightToolStripButton.Image");
    this.LightToolStripButton.ImageTransparentColor = Color.Magenta;
    this.LightToolStripButton.Name = "LightToolStripButton";
    this.LightToolStripButton.Size = new Size(23, 22);
    this.LightToolStripButton.Text = "Light color";
    this.LightToolStripButton.ToolTipText = "Set area Light color";
    this.LightToolStripButton.Click += new EventHandler(this.LightToolStripButton_Click);
    this.LightColorPickerToolStripButton.BackColor = Color.White;
    this.LightColorPickerToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.None;
    this.LightColorPickerToolStripButton.Image = (Image) componentResourceManager.GetObject("LightColorPickerToolStripButton.Image");
    this.LightColorPickerToolStripButton.ImageTransparentColor = Color.Magenta;
    this.LightColorPickerToolStripButton.Name = "LightColorPickerToolStripButton";
    this.LightColorPickerToolStripButton.Size = new Size(23, 22);
    this.LightColorPickerToolStripButton.Text = "toolStripButton2";
    this.LightColorPickerToolStripButton.ToolTipText = "Chose the light color";
    this.LightColorPickerToolStripButton.Visible = false;
    this.LightColorPickerToolStripButton.Click += new EventHandler(this.LightColorPickerToolStripButton_Click);
    this.mainMenu.Dock = DockStyle.None;
    this.mainMenu.Items.AddRange(new ToolStripItem[8]
    {
      (ToolStripItem) this.blablaToolStripMenuItem,
      (ToolStripItem) this.pluginsToolStripMenuItem,
      (ToolStripItem) this.viewsToolStripMenuItem,
      (ToolStripItem) this.settingsToolStripMenuItem,
      (ToolStripItem) this.toolMenuEntry,
      (ToolStripItem) this.creditsToolStripMenuItem,
      (ToolStripItem) this.donateToolStripMenuItem,
      (ToolStripItem) this.testToolStripMenuItem
    });
    this.mainMenu.Location = new Point(0, 0);
    this.mainMenu.Name = "mainMenu";
    this.mainMenu.Size = new Size(792, 25);
    this.mainMenu.TabIndex = 0;
    this.mainMenu.Text = "menuStrip1";
    this.blablaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[10]
    {
      (ToolStripItem) this.NewProjectMenuItem,
      (ToolStripItem) this.saveProjectToolStripMenuItem,
      (ToolStripItem) this.loadProjectToolStripMenuItem,
      (ToolStripItem) this.toolStripSeparator1,
      (ToolStripItem) this.ImportMapMenuItem,
      (ToolStripItem) this.ExportMapMenuItem1,
      (ToolStripItem) this.AtrExporttoolStripMenuItem,
      (ToolStripItem) this.BlkExporttoolStripMenuItem,
      (ToolStripItem) this.toolStripSeparator3,
      (ToolStripItem) this.closeToolStripMenuItem
    });
    this.blablaToolStripMenuItem.Name = "blablaToolStripMenuItem";
    this.blablaToolStripMenuItem.Size = new Size(35, 21);
    this.blablaToolStripMenuItem.Text = "File";
    this.NewProjectMenuItem.Name = "NewProjectMenuItem";
    this.NewProjectMenuItem.Size = new Size(199, 22);
    this.NewProjectMenuItem.Text = "New Project";
    this.NewProjectMenuItem.Click += new EventHandler(this.NewProjectMenuItem_Click);
    this.saveProjectToolStripMenuItem.Enabled = false;
    this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
    this.saveProjectToolStripMenuItem.Size = new Size(199, 22);
    this.saveProjectToolStripMenuItem.Text = "Save project";
    this.saveProjectToolStripMenuItem.Click += new EventHandler(this.saveProjectToolStripMenuItem_Click);
    this.loadProjectToolStripMenuItem.Name = "loadProjectToolStripMenuItem";
    this.loadProjectToolStripMenuItem.Size = new Size(199, 22);
    this.loadProjectToolStripMenuItem.Text = "Load project";
    this.loadProjectToolStripMenuItem.Click += new EventHandler(this.loadProjectToolStripMenuItem_Click);
    this.toolStripSeparator1.Name = "toolStripSeparator1";
    this.toolStripSeparator1.Size = new Size(196, 6);
    this.ImportMapMenuItem.Name = "ImportMapMenuItem";
    this.ImportMapMenuItem.Size = new Size(199, 22);
    this.ImportMapMenuItem.Text = "Import Top/Pko Map file";
    this.ImportMapMenuItem.Click += new EventHandler(this.ImportMapMenuItem_Click);
    this.ExportMapMenuItem1.Enabled = false;
    this.ExportMapMenuItem1.Name = "ExportMapMenuItem1";
    this.ExportMapMenuItem1.Size = new Size(199, 22);
    this.ExportMapMenuItem1.Text = "Export Top/Pko Map file";
    this.ExportMapMenuItem1.Click += new EventHandler(this.ExportMapMenuItem1_Click);
    this.AtrExporttoolStripMenuItem.Enabled = false;
    this.AtrExporttoolStripMenuItem.Name = "AtrExporttoolStripMenuItem";
    this.AtrExporttoolStripMenuItem.Size = new Size(199, 22);
    this.AtrExporttoolStripMenuItem.Text = "Export Top/Pko Atr file";
    this.AtrExporttoolStripMenuItem.Click += new EventHandler(this.AtrExporttoolStripMenuItem_Click);
    this.BlkExporttoolStripMenuItem.Enabled = false;
    this.BlkExporttoolStripMenuItem.Name = "BlkExporttoolStripMenuItem";
    this.BlkExporttoolStripMenuItem.Size = new Size(199, 22);
    this.BlkExporttoolStripMenuItem.Text = "Export Top/Pko Blk file";
    this.BlkExporttoolStripMenuItem.Click += new EventHandler(this.BlkExporttoolStripMenuItem_Click);
    this.toolStripSeparator3.Name = "toolStripSeparator3";
    this.toolStripSeparator3.Size = new Size(196, 6);
    this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
    this.closeToolStripMenuItem.Size = new Size(199, 22);
    this.closeToolStripMenuItem.Text = "Close";
    this.closeToolStripMenuItem.Click += new EventHandler(this.closeToolStripMenuItem_Click);
    this.pluginsToolStripMenuItem.Name = "pluginsToolStripMenuItem";
    this.pluginsToolStripMenuItem.Size = new Size(52, 21);
    this.pluginsToolStripMenuItem.Text = "Plugins";
    this.viewsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[1]
    {
      (ToolStripItem) this.toggleViewsStripMenuItem
    });
    this.viewsToolStripMenuItem.Name = "viewsToolStripMenuItem";
    this.viewsToolStripMenuItem.Size = new Size(46, 21);
    this.viewsToolStripMenuItem.Text = "Views";
    this.toggleViewsStripMenuItem.Enabled = false;
    this.toggleViewsStripMenuItem.Name = "toggleViewsStripMenuItem";
    this.toggleViewsStripMenuItem.Size = new Size(136, 22);
    this.toggleViewsStripMenuItem.Text = "Hide views";
    this.toggleViewsStripMenuItem.Click += new EventHandler(this.toggleViewsStripMenuItem_Click);
    this.settingsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.refreshTexturesFromGameClientToolStripMenuItem,
      (ToolStripItem) this.refreshAreasFromGameClientToolStripMenuItem
    });
    this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
    this.settingsToolStripMenuItem.Size = new Size(58, 21);
    this.settingsToolStripMenuItem.Text = "Settings";
    this.refreshTexturesFromGameClientToolStripMenuItem.Enabled = false;
    this.refreshTexturesFromGameClientToolStripMenuItem.Name = "refreshTexturesFromGameClientToolStripMenuItem";
    this.refreshTexturesFromGameClientToolStripMenuItem.Size = new Size(249, 22);
    this.refreshTexturesFromGameClientToolStripMenuItem.Text = "Refresh textures from game client";
    this.refreshTexturesFromGameClientToolStripMenuItem.Click += new EventHandler(this.refreshTexturesFromGameClientToolStripMenuItem_Click);
    this.refreshAreasFromGameClientToolStripMenuItem.Enabled = true;
    this.refreshAreasFromGameClientToolStripMenuItem.Name = "refreshAreasFromGameClientToolStripMenuItem";
    this.refreshAreasFromGameClientToolStripMenuItem.Size = new Size(249, 22);
    this.refreshAreasFromGameClientToolStripMenuItem.Text = "Refresh areas from game client";
    this.refreshAreasFromGameClientToolStripMenuItem.Click += new EventHandler(this.refreshAreasFromGameClientToolStripMenuItem_Click);
    this.toolMenuEntry.DropDownItems.AddRange(new ToolStripItem[2]
    {
      (ToolStripItem) this.cropMapToolStripMenuItem,
      (ToolStripItem) this.importSubMapToolStripMenuItem
    });
    this.toolMenuEntry.Enabled = false;
    this.toolMenuEntry.Name = "toolMenuEntry";
    this.toolMenuEntry.Size = new Size(44, 21);
    this.toolMenuEntry.Text = "Tools";
    this.cropMapToolStripMenuItem.Name = "cropMapToolStripMenuItem";
    this.cropMapToolStripMenuItem.Size = new Size(161, 22);
    this.cropMapToolStripMenuItem.Text = "Crop Map";
    this.cropMapToolStripMenuItem.Click += new EventHandler(this.cropMapToolStripMenuItem_Click);
    this.importSubMapToolStripMenuItem.Name = "importSubMapToolStripMenuItem";
    this.importSubMapToolStripMenuItem.Size = new Size(161, 22);
    this.importSubMapToolStripMenuItem.Text = "Import Sub Map";
    this.importSubMapToolStripMenuItem.Click += new EventHandler(this.importSubMapToolStripMenuItem_Click);
    this.creditsToolStripMenuItem.Name = "creditsToolStripMenuItem";
    this.creditsToolStripMenuItem.Size = new Size(53, 21);
    this.creditsToolStripMenuItem.Text = "Credits";
    this.creditsToolStripMenuItem.Click += new EventHandler(this.creditsToolStripMenuItem_Click);
    this.donateToolStripMenuItem.BackColor = Color.FromArgb(192 /*0xC0*/, 192 /*0xC0*/, (int) byte.MaxValue);
    this.donateToolStripMenuItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
    this.donateToolStripMenuItem.Font = new Font("OCRB", 12f, FontStyle.Bold | FontStyle.Underline, GraphicsUnit.Point, (byte) 0);
    this.donateToolStripMenuItem.ForeColor = Color.FromArgb(192 /*0xC0*/, 0, 0);
    this.donateToolStripMenuItem.Name = "donateToolStripMenuItem";
    this.donateToolStripMenuItem.Size = new Size(98, 21);
    this.donateToolStripMenuItem.Text = "Donate";
    this.donateToolStripMenuItem.Click += new EventHandler(this.donateToolStripMenuItem_Click);
    this.testToolStripMenuItem.Name = "testToolStripMenuItem";
    this.testToolStripMenuItem.Size = new Size(38, 21);
    this.testToolStripMenuItem.Text = "test";
    this.testToolStripMenuItem.Visible = false;
    this.testToolStripMenuItem.Click += new EventHandler(this.testToolStripMenuItem_Click);
    this.toolStripContainer1.ContentPanel.AutoScroll = true;
    this.toolStripContainer1.ContentPanel.Controls.Add((Control) this.mapRenderer1);
    this.toolStripContainer1.ContentPanel.Size = new Size(792, 498);
    this.toolStripContainer1.Dock = DockStyle.Fill;
    this.toolStripContainer1.Location = new Point(0, 0);
    this.toolStripContainer1.Name = "toolStripContainer1";
    this.toolStripContainer1.Size = new Size(792, 573);
    this.toolStripContainer1.TabIndex = 3;
    this.toolStripContainer1.Text = "toolStripContainer1";
    this.toolStripContainer1.TopToolStripPanel.Controls.Add((Control) this.mainMenu);
    this.toolStripContainer1.TopToolStripPanel.Controls.Add((Control) this.LayerToolBar);
    this.toolStripContainer1.TopToolStripPanel.Controls.Add((Control) this.drawingToolToolBar);
    this.mapRenderer1.BorderStyle = BorderStyle.FixedSingle;
    this.mapRenderer1.Dock = DockStyle.Fill;
    this.mapRenderer1.Location = new Point(0, 0);
    this.mapRenderer1.Name = "mapRenderer1";
    this.mapRenderer1.Size = new Size(792, 498);
    this.mapRenderer1.TabIndex = 1;
    this.drawingToolToolBar.Dock = DockStyle.None;
    this.drawingToolToolBar.Items.AddRange(new ToolStripItem[8]
    {
      (ToolStripItem) this.penSquaredButton,
      (ToolStripItem) this.penRoundedButton,
      (ToolStripItem) this.lineButton,
      (ToolStripItem) this.rectangleButton,
      (ToolStripItem) this.ellipseButton,
      (ToolStripItem) this.toolStripSeparator5,
      (ToolStripItem) this.toolStripLabel1,
      (ToolStripItem) this.PenSizeCombo
    });
    this.drawingToolToolBar.Location = new Point(3, 50);
    this.drawingToolToolBar.Name = "drawingToolToolBar";
    this.drawingToolToolBar.Size = new Size((int) byte.MaxValue, 25);
    this.drawingToolToolBar.TabIndex = 1;
    this.penSquaredButton.Checked = true;
    this.penSquaredButton.CheckState = CheckState.Checked;
    this.penSquaredButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.penSquaredButton.Image = (Image) componentResourceManager.GetObject("penSquaredButton.Image");
    this.penSquaredButton.ImageTransparentColor = Color.Magenta;
    this.penSquaredButton.Name = "penSquaredButton";
    this.penSquaredButton.Size = new Size(23, 22);
    this.penSquaredButton.Text = "toolStripButton1";
    this.penSquaredButton.ToolTipText = "Free hand  drawing tool with a squard shape.";
    this.penSquaredButton.Click += new EventHandler(this.penSquaredButton_Click);
    this.penRoundedButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.penRoundedButton.Image = (Image) componentResourceManager.GetObject("penRoundedButton.Image");
    this.penRoundedButton.ImageTransparentColor = Color.Magenta;
    this.penRoundedButton.Name = "penRoundedButton";
    this.penRoundedButton.Size = new Size(23, 22);
    this.penRoundedButton.Text = "toolStripButton2";
    this.penRoundedButton.ToolTipText = "Free hand  drawing tool with a rounded shape.";
    this.penRoundedButton.Click += new EventHandler(this.penRoundedButton_Click);
    this.lineButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.lineButton.Image = (Image) componentResourceManager.GetObject("lineButton.Image");
    this.lineButton.ImageTransparentColor = Color.Magenta;
    this.lineButton.Name = "lineButton";
    this.lineButton.Size = new Size(23, 22);
    this.lineButton.Text = "toolStripButton3";
    this.lineButton.ToolTipText = "Line drawing tool.";
    this.lineButton.Click += new EventHandler(this.lineButton_Click);
    this.rectangleButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.rectangleButton.Image = (Image) componentResourceManager.GetObject("rectangleButton.Image");
    this.rectangleButton.ImageTransparentColor = Color.Magenta;
    this.rectangleButton.Name = "rectangleButton";
    this.rectangleButton.Size = new Size(23, 22);
    this.rectangleButton.Text = "toolStripButton4";
    this.rectangleButton.ToolTipText = "Filled rectangle drawing tool.";
    this.rectangleButton.Click += new EventHandler(this.rectangleButton_Click);
    this.ellipseButton.DisplayStyle = ToolStripItemDisplayStyle.Image;
    this.ellipseButton.Image = (Image) componentResourceManager.GetObject("ellipseButton.Image");
    this.ellipseButton.ImageTransparentColor = Color.Magenta;
    this.ellipseButton.Name = "ellipseButton";
    this.ellipseButton.Size = new Size(23, 22);
    this.ellipseButton.Text = "toolStripButton5";
    this.ellipseButton.ToolTipText = "Filled Ellipse drawing tool.";
    this.ellipseButton.Click += new EventHandler(this.ellipseButton_Click);
    this.toolStripSeparator5.Name = "toolStripSeparator5";
    this.toolStripSeparator5.Size = new Size(6, 25);
    this.toolStripLabel1.Name = "toolStripLabel1";
    this.toolStripLabel1.Size = new Size(47, 22);
    this.toolStripLabel1.Text = "Pen Size";
    this.PenSizeCombo.DropDownWidth = 30;
    this.PenSizeCombo.Items.AddRange(new object[20]
    {
      (object) "1",
      (object) "2",
      (object) "3",
      (object) "4",
      (object) "5",
      (object) "6",
      (object) "7",
      (object) "8",
      (object) "9",
      (object) "10",
      (object) "11",
      (object) "12",
      (object) "13",
      (object) "14",
      (object) "15",
      (object) "16",
      (object) "17",
      (object) "18",
      (object) "19",
      (object) "20"
    });
    this.PenSizeCombo.Name = "PenSizeCombo";
    this.PenSizeCombo.Size = new Size(75, 25);
    this.PenSizeCombo.Text = "1";
    this.PenSizeCombo.ToolTipText = "Size of the Drawing Pen";
    this.PenSizeCombo.TextChanged += new EventHandler(this.PenSizeCombo_TextChanged);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(792, 573);
    this.Controls.Add((Control) this.toolStripContainer1);
    this.MainMenuStrip = this.mainMenu;
    this.Name = nameof (MainForm);
    this.Text = "YAMMI - Yet Another Map Manager Innovation By Matt";
    this.WindowState = FormWindowState.Maximized;
    this.Load += new EventHandler(this.MainForm_Load);
    this.LayerToolBar.ResumeLayout(false);
    this.LayerToolBar.PerformLayout();
    this.mainMenu.ResumeLayout(false);
    this.mainMenu.PerformLayout();
    this.toolStripContainer1.ContentPanel.ResumeLayout(false);
    this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
    this.toolStripContainer1.TopToolStripPanel.PerformLayout();
    this.toolStripContainer1.ResumeLayout(false);
    this.toolStripContainer1.PerformLayout();
    this.drawingToolToolBar.ResumeLayout(false);
    this.drawingToolToolBar.PerformLayout();
    this.ResumeLayout(false);
  }
}
