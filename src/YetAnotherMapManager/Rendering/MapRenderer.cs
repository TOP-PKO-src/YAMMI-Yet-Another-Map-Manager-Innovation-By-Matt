using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Windows.Forms;
using YetAnotherMapManager.Debug;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins;

#nullable disable
namespace YetAnotherMapManager.Rendering;

public class MapRenderer : UserControl
{
  private MapInfo map;
  public Dictionary<string, IRenderer> RendererList;
  private Bitmap[] layerSolid;
  private Bitmap[] layerSeaLand;
  private int MapSplit = 256 /*0x0100*/;
  private int renderingWidth;
  private int renderingHeight;
  private float zoomRatio = 1f;
  private int clippingWidth;
  private int clippingHeight;
  private int hOffset;
  private int vOffset;
  private int hMapSplitCount;
  private int vMapSplitCount;
  private int dragReferenceXPosition = -1;
  private int dragReferenceYPosition = -1;
  private bool dragOccured;
  private Cursor currentCursor = Cursor.Current;
  public static short DRAW_SQUARED_PEN = 1;
  public static short DRAW_ROUNDED_PEN = 2;
  public static short DRAW_LINE = 3;
  public static short DRAW_ELLIPSE = 4;
  public static short DRAW_RECTANGLE = 5;
  public static short DRAW_FILLED_AREA = 6;
  private bool editModeOn;
  private bool drawingOngoing;
  private short drawingMode = MapRenderer.DRAW_SQUARED_PEN;
  private Bitmap drawingBitmap;
  private Point startPoint = Point.Empty;
  private int penSize = 1;
  private IContainer components = new Container();
  private HScrollBar hScrollBar;
  private VScrollBar vScrollBar;
  private DoubleBufferedPanel renderingPanel;
  private DoubleBufferedPanel DrawingOverlayPanel;

  public MapInfo Map
  {
    set
    {
      this.map = value;
      this.AdjustSize();
      this.Reset();
      this.renderingPanel.Invalidate();
    }
  }

  public void Reset()
  {
    this.layerSolid = new Bitmap[this.hMapSplitCount * this.vMapSplitCount];
    this.layerSeaLand = new Bitmap[this.hMapSplitCount * this.vMapSplitCount];
    this.zoomRatio = 1f;
    this.SetMapOffset(0, 0);
  }

  public MapRenderer()
  {
    this.InitializeComponent();
    this.MouseWheel += new MouseEventHandler(this.MapRenderer_MouseWheel);
  }

  private void MapRenderer_MouseWheel(object sender, MouseEventArgs e)
  {
    if (e.X < 0 || e.Y < 0 || e.X > this.renderingPanel.Width || e.Y > this.renderingPanel.Height)
      return;
    Point coordinatesFromXy = this.GetMapCoordinatesFromXY(this.renderingPanel.Width / 2, this.renderingPanel.Height / 2);
    if (e.Delta < 0)
      this.SetZoomLevel(this.zoomRatio * 0.5f);
    else
      this.SetZoomLevel(this.zoomRatio / 0.5f);
    this.ZoomMapOnCoordinates(coordinatesFromXy.X, coordinatesFromXy.Y);
    this.renderingPanel.Invalidate();
  }

  private void vScrollBar_Scroll(object sender, ScrollEventArgs e)
  {
    this.SetMapOffset(this.hScrollBar.Value, this.vScrollBar.Value);
    this.renderingPanel.Invalidate();
  }

  private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
  {
    this.SetMapOffset(this.hScrollBar.Value, this.vScrollBar.Value);
    this.renderingPanel.Invalidate();
  }

  public event MapRenderer.MouseMoveOnMapHandler MouseMovedOnMap;

  protected virtual void OnMouseMovedOnMap(MouseOnMapEventArgs args)
  {
    if (this.MouseMovedOnMap == null)
      return;
    this.MouseMovedOnMap((object) this, args);
  }

  public event MapRenderer.MouseDownOnMapHandler MouseDownOnMap;

  protected virtual void OnMouseDownOnMap(MouseOnMapEventArgs args)
  {
    if (this.MouseDownOnMap == null)
      return;
    this.MouseDownOnMap((object) this, args);
  }

  public event MapRenderer.MapDrawnAtTileLevelHandler MapDrawnAtTileLevel;

  public event MapRenderer.MapDrawnAtSubTileLevelHandler MapDrawnAtSubTileLevel;

  protected virtual void OnMapDrawn(Bitmap bitmap)
  {
    if (this.MapDrawnAtTileLevel != null)
    {
      List<Point> list = new List<Point>();
      FastBitmap fastBitmap = new FastBitmap(this.drawingBitmap);
      fastBitmap.LockBitmap();
      int num1 = (int) ((double) this.drawingBitmap.Width / (double) this.zoomRatio);
      int num2 = (int) ((double) this.drawingBitmap.Height / (double) this.zoomRatio);
      int num3 = 1;
      for (int index1 = 0; index1 < num1; ++index1)
      {
        for (int index2 = 0; index2 < num2; ++index2)
        {
          int num4 = 0;
          for (int index3 = 0; (double) index3 < (double) this.zoomRatio; ++index3)
          {
            for (int index4 = 0; (double) index4 < (double) this.zoomRatio; ++index4)
            {
              int x = (int) ((double) index1 * (double) this.zoomRatio + (double) index3);
              int y = (int) ((double) index2 * (double) this.zoomRatio + (double) index4);
              if (fastBitmap.GetPixel(x, y).Alpha != (byte) 0)
                ++num4;
            }
          }
          if (num4 > num3)
            list.Add(new Point(index1 + this.hOffset, index2 + this.vOffset));
        }
      }
      fastBitmap.UnlockBitmap();
      if (list.Count > 0)
        this.MapDrawnAtTileLevel(list);
    }
    if (this.MapDrawnAtSubTileLevel == null)
      return;
    List<SubTilePoint> list1 = new List<SubTilePoint>();
    FastBitmap fastBitmap1 = new FastBitmap(this.drawingBitmap);
    fastBitmap1.LockBitmap();
    int num5 = (int) ((double) this.drawingBitmap.Width / (double) this.zoomRatio);
    int num6 = (int) ((double) this.drawingBitmap.Height / (double) this.zoomRatio);
    int num7 = (int) ((double) this.zoomRatio * (double) this.zoomRatio / 8.0);
    for (int index5 = 0; index5 < num5; ++index5)
    {
      for (int index6 = 0; index6 < num6; ++index6)
      {
        for (int SubX = 0; SubX < 2; ++SubX)
        {
          for (int SubY = 0; SubY < 2; ++SubY)
          {
            int num8 = 0;
            for (int index7 = 0; (double) index7 < (double) this.zoomRatio; ++index7)
            {
              for (int index8 = 0; (double) index8 < (double) this.zoomRatio; ++index8)
              {
                int x = (int) ((double) index5 * (double) this.zoomRatio + (double) SubX * (double) this.zoomRatio / 2.0 + (double) index7);
                int y = (int) ((double) index6 * (double) this.zoomRatio + (double) SubY * (double) this.zoomRatio / 2.0 + (double) index8);
                if (fastBitmap1.GetPixel(x, y).Alpha != (byte) 0)
                  ++num8;
              }
            }
            if (num8 > num7)
              list1.Add(new SubTilePoint(index5 + this.hOffset, index6 + this.vOffset, SubX, SubY));
          }
        }
      }
    }
    fastBitmap1.UnlockBitmap();
    if (list1.Count <= 0)
      return;
    this.MapDrawnAtSubTileLevel(list1);
  }

  private void AdjustSize()
  {
    if (this.map == null || this.renderingPanel.Width == 0 || this.renderingPanel.Height == 0)
      return;
    this.renderingWidth = this.renderingPanel.Width;
    this.renderingHeight = this.renderingPanel.Height;
    this.clippingWidth = Math.Min(this.map.Width, (int) Math.Floor((double) this.renderingPanel.Width / (double) this.zoomRatio));
    this.clippingHeight = Math.Min(this.map.Height, (int) Math.Floor((double) this.renderingPanel.Height / (double) this.zoomRatio));
    this.hMapSplitCount = (int) Math.Ceiling((double) this.map.Width / (double) this.MapSplit);
    this.vMapSplitCount = (int) Math.Ceiling((double) this.map.Height / (double) this.MapSplit);
    this.hScrollBar.Minimum = 0;
    this.hScrollBar.Maximum = this.map.Width - this.clippingWidth;
    this.vScrollBar.Minimum = 0;
    this.vScrollBar.Maximum = this.map.Height - this.clippingHeight;
  }

  private void SetMapOffset(int _x, int _y)
  {
    if (this.map == null)
      return;
    _x = Math.Min(_x, this.map.Width - this.clippingWidth - 1);
    _y = Math.Min(_y, this.map.Height - this.clippingHeight - 1);
    _x = Math.Max(0, _x);
    _y = Math.Max(0, _y);
    this.hOffset = _x;
    this.vOffset = _y;
    this.hScrollBar.Value = this.hOffset;
    this.vScrollBar.Value = this.vOffset;
  }

  private void SetZoomLevel(float zoomLevel)
  {
    if (this.drawingOngoing)
      return;
    zoomLevel = Math.Max(zoomLevel, 0.125f);
    zoomLevel = Math.Min(zoomLevel, 16f);
    if ((double) zoomLevel == (double) this.zoomRatio)
      return;
    this.zoomRatio = zoomLevel;
    this.AdjustSize();
  }

  private void CustomPaint(Graphics g)
  {
    PerfTimer.Instance.Start("MapRenderer.Repaint");
    Rectangle rectangle1 = new Rectangle(0, 0, this.renderingPanel.Width, this.renderingPanel.Height);
    Rectangle rectangle2 = new Rectangle(0, 0, this.map.Width, this.map.Height);
    Rectangle rectangle3 = new Rectangle(this.hOffset, this.vOffset, (int) ((double) this.renderingPanel.Width / (double) this.zoomRatio), (int) ((double) this.renderingPanel.Height / (double) this.zoomRatio));
    ImageAttributes imageAttr = new ImageAttributes();
    imageAttr.SetWrapMode(WrapMode.TileFlipXY);
    int width;
    for (int hOffset = this.hOffset; hOffset - this.hOffset < rectangle3.Width; hOffset += width)
    {
      int num1 = hOffset / this.MapSplit;
      int x1 = hOffset % this.MapSplit;
      width = Math.Min(this.MapSplit - x1, rectangle3.Width - (hOffset - this.hOffset));
      int height;
      for (int vOffset = this.vOffset; vOffset - this.vOffset < rectangle3.Height; vOffset += height)
      {
        int num2 = vOffset / this.MapSplit;
        int y1 = vOffset % this.MapSplit;
        height = Math.Min(this.MapSplit - y1, rectangle3.Height - (vOffset - this.vOffset));
        Rectangle rectangle4 = new Rectangle(x1, y1, width, height);
        if (this.RendererList != null)
        {
          foreach (IRenderer renderer in this.RendererList.Values)
          {
            if (renderer.IsEnabled())
            {
              int scale = renderer.GetScale();
              int x2 = hOffset / this.MapSplit * this.MapSplit;
              int y2 = vOffset / this.MapSplit * this.MapSplit;
              if (x2 < this.map.Width && y2 < this.map.Height)
              {
                float alphaLevel = renderer.GetAlphaLevel();
                if ((double) alphaLevel != 1.0)
                {
                  ColorMatrix newColorMatrix = new ColorMatrix(new float[5][]
                  {
                    new float[5]{ 1f, 0.0f, 0.0f, 0.0f, 0.0f },
                    new float[5]{ 0.0f, 1f, 0.0f, 0.0f, 0.0f },
                    new float[5]{ 0.0f, 0.0f, 1f, 0.0f, 0.0f },
                    new float[5]{ 0.0f, 0.0f, 0.0f, alphaLevel, 0.0f },
                    new float[5]{ 0.0f, 0.0f, 0.0f, 0.0f, 1f }
                  });
                  imageAttr.SetColorMatrix(newColorMatrix);
                }
                Bitmap renderedArea = renderer.GetRenderedArea(x2, y2, this.MapSplit, this.MapSplit);
                Rectangle destRect = new Rectangle((int) ((double) (hOffset - this.hOffset) * (double) this.zoomRatio), (int) ((double) (vOffset - this.vOffset) * (double) this.zoomRatio), (int) ((double) rectangle4.Width * (double) this.zoomRatio), (int) ((double) rectangle4.Height * (double) this.zoomRatio));
                if (renderedArea != null)
                {
                  Rectangle rectangle5 = new Rectangle(x1 * scale, y1 * scale, rectangle4.Width * scale, rectangle4.Height * scale);
                  g.DrawImage((Image) renderedArea, destRect, rectangle5.X, rectangle5.Y, rectangle5.Width, rectangle5.Height, GraphicsUnit.Pixel, imageAttr);
                }
                imageAttr.ClearColorMatrix();
              }
            }
          }
        }
      }
    }
    imageAttr.Dispose();
    PerfTimer.Instance.Stop("MapRenderer.Repaint");
    g.DrawString($"Zoom={(object) (int) ((double) this.zoomRatio * 100.0)}%", new Font(FontFamily.GenericMonospace, 12f), Brushes.Yellow, (PointF) new Point(15, 15));
  }

  private void renderingPanel_Paint(object sender, PaintEventArgs e)
  {
    if (this.map == null)
      return;
    try
    {
      this.CustomPaint(e.Graphics);
    }
    catch (Exception)
    {
      int num = (int) MessageBox.Show("Whatever happened its bad. Things will prolly notbe rendered anymore, but you can try to save still... Good luck and have fun. ~Matt");
    }
  }

  private void MapRenderer_Resize(object sender, EventArgs e)
  {
    if (this.map == null)
      return;
    this.AdjustSize();
    this.renderingPanel.Invalidate();
  }

  private Point GetMapCoordinatesFromXY(int x, int y)
  {
    return new Point((int) (((double) this.hOffset + (double) x / (double) this.zoomRatio) * 100.0), (int) (((double) this.vOffset + (double) y / (double) this.zoomRatio) * 100.0));
  }

  private void ZoomMapOnCoordinates(int x, int y)
  {
    x /= 100;
    y /= 100;
    this.SetMapOffset((int) Math.Max(0.0f, (float) x - (float) this.renderingPanel.Width / (2f * this.zoomRatio)), (int) Math.Max(0.0f, (float) y - (float) this.renderingPanel.Height / (2f * this.zoomRatio)));
  }

  private void renderingPanel_MouseMove(object sender, MouseEventArgs e)
  {
    if (this.map == null)
      return;
    if (e.Button == MouseButtons.Right)
    {
      this.picture_Drag(sender, e);
    }
    else
    {
      MouseOnMapEventArgs mouseOnMapEventArgs = this.BuildMouseOnMapEventargs(e);
      if (mouseOnMapEventArgs.InMap && this.editModeOn)
        this.Draw(mouseOnMapEventArgs);
      this.OnMouseMovedOnMap(mouseOnMapEventArgs);
    }
  }

  private MouseOnMapEventArgs BuildMouseOnMapEventargs(MouseEventArgs e)
  {
    MouseOnMapEventArgs mouseOnMapEventArgs = new MouseOnMapEventArgs();
    mouseOnMapEventArgs.Button = e.Button;
    mouseOnMapEventArgs.UiCoords = new Point(e.X, e.Y);
    mouseOnMapEventArgs.Location = this.GetMapCoordinatesFromXY(e.X, e.Y);
    if (mouseOnMapEventArgs.Location.X >= 0 && mouseOnMapEventArgs.Location.X < this.map.Width * 100 && mouseOnMapEventArgs.Location.Y >= 0 && mouseOnMapEventArgs.Location.Y < this.map.Height * 100)
    {
      mouseOnMapEventArgs.InMap = true;
      mouseOnMapEventArgs.Coords = new Point(mouseOnMapEventArgs.Location.X / 100, mouseOnMapEventArgs.Location.Y / 100);
    }
    else
      mouseOnMapEventArgs.InMap = false;
    return mouseOnMapEventArgs;
  }

  private void renderingPanel_MouseClick(object sender, MouseEventArgs e)
  {
    if (this.map == null)
      return;
    MouseOnMapEventArgs e1 = this.BuildMouseOnMapEventargs(e);
    if (e.Button == MouseButtons.Right)
    {
      if (this.dragOccured)
      {
        this.dragOccured = false;
      }
      else
      {
        if (!e1.InMap || this.drawingOngoing)
          return;
        this.ZoomMapOnCoordinates(e1.Location.X, e1.Location.Y);
      }
    }
    else
    {
      if (e.Button != MouseButtons.Left)
        return;
      this.InitDrawing(e1);
    }
  }

  private void renderingPanel_MouseUp(object sender, MouseEventArgs e)
  {
    if (this.map == null)
      return;
    if (e.Button == MouseButtons.Right)
    {
      this.picture_DragLeave(sender, (EventArgs) e);
    }
    else
    {
      if (e.Button != MouseButtons.Left)
        return;
      this.FinalizeDrawing();
    }
  }

  private void renderingPanel_MouseLeave(object sender, EventArgs e)
  {
    if (this.map == null)
      return;
    this.picture_DragLeave(sender, e);
  }

  private void renderingPanel_MouseDown(object sender, MouseEventArgs e)
  {
    if (this.map == null)
      return;
    MouseOnMapEventArgs e1 = this.BuildMouseOnMapEventargs(e);
    if (e.Button == MouseButtons.Right)
    {
      if (!e1.InMap)
        return;
      this.picture_DragEnter(sender, e);
    }
    else
    {
      if (e.Button != MouseButtons.Left)
        return;
      this.InitDrawing(e1);
    }
  }

  private void picture_DragEnter(object sender, MouseEventArgs e)
  {
    if (this.drawingOngoing)
      return;
    this.dragReferenceXPosition = e.X;
    this.dragReferenceYPosition = e.Y;
    this.currentCursor = Cursor.Current;
    Cursor.Current = Cursors.SizeAll;
  }

  private void picture_DragLeave(object sender, EventArgs e)
  {
    if (!this.dragOccured)
      return;
    this.dragReferenceXPosition = -1;
    this.dragReferenceYPosition = -1;
    Cursor.Current = this.currentCursor;
  }

  private void picture_Drag(object sender, MouseEventArgs e)
  {
    if (this.dragReferenceXPosition == -1 || this.dragReferenceYPosition == -1)
      return;
    this.dragOccured = true;
    int num1 = e.X - this.dragReferenceXPosition;
    int num2 = e.Y - this.dragReferenceYPosition;
    int _x = this.hOffset - (int) ((double) num1 / (double) this.zoomRatio);
    int _y = this.vOffset - (int) ((double) num2 / (double) this.zoomRatio);
    if (_x < 0)
      _x = 0;
    if (_x >= this.map.Width)
      _x = this.map.Width - 1;
    if (_y < 0)
      _y = 0;
    if (_y >= this.map.Height)
      _y = this.map.Height - 1;
    this.dragReferenceXPosition = e.X;
    this.dragReferenceYPosition = e.Y;
    this.SetMapOffset(_x, _y);
    this.renderingPanel.Invalidate();
  }

  public short DrawTool
  {
    set => this.drawingMode = value;
  }

  public int PenSize
  {
    set => this.penSize = value;
  }

  public void StartEditMode()
  {
    if (this.editModeOn)
      return;
    this.editModeOn = true;
    this.DrawingOverlayPanel.Visible = true;
    this.drawingBitmap = new Bitmap(this.renderingWidth, this.renderingHeight);
  }

  public void EndEditMode()
  {
    if (!this.editModeOn)
      return;
    this.editModeOn = false;
    this.DrawingOverlayPanel.Visible = false;
    this.drawingBitmap.Dispose();
    this.drawingBitmap = (Bitmap) null;
    if (!this.drawingOngoing)
      return;
    this.FinalizeDrawing();
  }

  private void InitDrawing(MouseOnMapEventArgs e)
  {
    if (!e.InMap || (double) this.zoomRatio < 1.0 || !this.editModeOn)
      return;
    this.drawingOngoing = true;
    Graphics graphics = Graphics.FromImage((Image) this.drawingBitmap);
    graphics.Clear(Color.Transparent);
    graphics.Dispose();
    this.startPoint = e.UiCoords;
    this.Draw(e);
  }

  private void FinalizeDrawing()
  {
    if (!this.editModeOn || !this.drawingOngoing)
      return;
    this.drawingOngoing = false;
    this.OnMapDrawn(this.drawingBitmap);
    Graphics graphics = Graphics.FromImage((Image) this.drawingBitmap);
    graphics.Clear(Color.Transparent);
    graphics.Dispose();
    this.DrawingOverlayPanel.Invalidate();
    this.startPoint = Point.Empty;
  }

  private void Draw(MouseOnMapEventArgs e)
  {
    if (!this.editModeOn)
      return;
    Graphics graphics = Graphics.FromImage((Image) this.drawingBitmap);
    if (this.drawingOngoing)
    {
      if ((int) this.drawingMode == (int) MapRenderer.DRAW_SQUARED_PEN)
      {
        int num1 = (int) this.zoomRatio * this.penSize;
        if (num1 >= 1)
        {
          int num2 = num1 / 2;
          graphics.FillRectangle(Brushes.Pink, e.UiCoords.X - num2, e.UiCoords.Y - num2, num1, num1);
        }
      }
      else if ((int) this.drawingMode == (int) MapRenderer.DRAW_ROUNDED_PEN)
      {
        int num3 = (int) this.zoomRatio * this.penSize;
        if (num3 >= 1)
        {
          int num4 = num3 / 2;
          graphics.FillEllipse(Brushes.Pink, e.UiCoords.X - num4, e.UiCoords.Y - num4, num3, num3);
        }
      }
      else if ((int) this.drawingMode == (int) MapRenderer.DRAW_LINE)
      {
        Pen pen = new Pen(Color.Pink, this.zoomRatio * (float) this.penSize);
        graphics.Clear(Color.Transparent);
        graphics.DrawLine(pen, this.startPoint, e.UiCoords);
      }
      else if ((int) this.drawingMode == (int) MapRenderer.DRAW_RECTANGLE)
      {
        graphics.Clear(Color.Transparent);
        graphics.FillRectangle(Brushes.Pink, Math.Min(this.startPoint.X, e.UiCoords.X), Math.Min(this.startPoint.Y, e.UiCoords.Y), Math.Abs(e.UiCoords.X - this.startPoint.X), Math.Abs(e.UiCoords.Y - this.startPoint.Y));
      }
      else if ((int) this.drawingMode == (int) MapRenderer.DRAW_ELLIPSE)
      {
        graphics.Clear(Color.Transparent);
        graphics.FillEllipse(Brushes.Pink, this.startPoint.X, this.startPoint.Y, e.UiCoords.X - this.startPoint.X, e.UiCoords.Y - this.startPoint.Y);
      }
    }
    else
    {
      graphics.Clear(Color.Transparent);
      if ((int) this.drawingMode == (int) MapRenderer.DRAW_SQUARED_PEN)
      {
        int num5 = (int) this.zoomRatio * this.penSize;
        if (num5 >= 1)
        {
          int num6 = num5 / 2;
          graphics.DrawRectangle(Pens.Red, e.UiCoords.X - num6, e.UiCoords.Y - num6, num5, num5);
        }
      }
      else if ((int) this.drawingMode == (int) MapRenderer.DRAW_ROUNDED_PEN)
      {
        int num7 = (int) this.zoomRatio * this.penSize;
        if (num7 >= 1)
        {
          int num8 = num7 / 2;
          graphics.DrawEllipse(Pens.Red, e.UiCoords.X - num8, e.UiCoords.Y - num8, num7, num7);
        }
      }
    }
    graphics.Dispose();
    this.DrawingOverlayPanel.Invalidate();
  }

  private void DrawingOverlayPanel_Paint(object sender, PaintEventArgs e)
  {
    if (!this.editModeOn)
      return;
    e.Graphics.DrawImageUnscaled((Image) this.drawingBitmap, new Point(0, 0));
  }

  protected override void Dispose(bool disposing)
  {
    if (disposing && this.components != null)
      this.components.Dispose();
    base.Dispose(disposing);
  }

  private void InitializeComponent()
  {
    this.hScrollBar = new HScrollBar();
    this.vScrollBar = new VScrollBar();
    this.renderingPanel = new DoubleBufferedPanel();
    this.DrawingOverlayPanel = new DoubleBufferedPanel();
    this.renderingPanel.SuspendLayout();
    this.SuspendLayout();
    this.hScrollBar.Dock = DockStyle.Bottom;
    this.hScrollBar.Location = new Point(0, 264);
    this.hScrollBar.Name = "hScrollBar";
    this.hScrollBar.Size = new Size(289, 16 /*0x10*/);
    this.hScrollBar.TabIndex = 0;
    this.hScrollBar.Scroll += new ScrollEventHandler(this.hScrollBar_Scroll);
    this.vScrollBar.Dock = DockStyle.Right;
    this.vScrollBar.Location = new Point(273, 0);
    this.vScrollBar.Name = "vScrollBar";
    this.vScrollBar.Size = new Size(16 /*0x10*/, 264);
    this.vScrollBar.TabIndex = 1;
    this.vScrollBar.Scroll += new ScrollEventHandler(this.vScrollBar_Scroll);
    this.renderingPanel.BackColor = Color.Black;
    this.renderingPanel.Controls.Add((Control) this.DrawingOverlayPanel);
    this.renderingPanel.Dock = DockStyle.Fill;
    this.renderingPanel.Location = new Point(0, 0);
    this.renderingPanel.Name = "renderingPanel";
    this.renderingPanel.Size = new Size(273, 264);
    this.renderingPanel.TabIndex = 2;
    this.renderingPanel.MouseLeave += new EventHandler(this.renderingPanel_MouseLeave);
    this.renderingPanel.Paint += new PaintEventHandler(this.renderingPanel_Paint);
    this.renderingPanel.MouseMove += new MouseEventHandler(this.renderingPanel_MouseMove);
    this.renderingPanel.MouseClick += new MouseEventHandler(this.renderingPanel_MouseClick);
    this.renderingPanel.MouseDown += new MouseEventHandler(this.renderingPanel_MouseDown);
    this.renderingPanel.MouseUp += new MouseEventHandler(this.renderingPanel_MouseUp);
    this.DrawingOverlayPanel.BackColor = Color.Transparent;
    this.DrawingOverlayPanel.Dock = DockStyle.Fill;
    this.DrawingOverlayPanel.Location = new Point(0, 0);
    this.DrawingOverlayPanel.Name = "DrawingOverlayPanel";
    this.DrawingOverlayPanel.Size = new Size(273, 264);
    this.DrawingOverlayPanel.TabIndex = 0;
    this.DrawingOverlayPanel.Visible = false;
    this.DrawingOverlayPanel.MouseLeave += new EventHandler(this.renderingPanel_MouseLeave);
    this.DrawingOverlayPanel.Paint += new PaintEventHandler(this.DrawingOverlayPanel_Paint);
    this.DrawingOverlayPanel.MouseMove += new MouseEventHandler(this.renderingPanel_MouseMove);
    this.DrawingOverlayPanel.MouseDown += new MouseEventHandler(this.renderingPanel_MouseDown);
    this.DrawingOverlayPanel.MouseUp += new MouseEventHandler(this.renderingPanel_MouseUp);
    this.AutoScaleDimensions = new SizeF(6f, 13f);
    this.AutoScaleMode = AutoScaleMode.Font;
    this.BorderStyle = BorderStyle.FixedSingle;
    this.Controls.Add((Control) this.renderingPanel);
    this.Controls.Add((Control) this.vScrollBar);
    this.Controls.Add((Control) this.hScrollBar);
    this.Name = nameof (MapRenderer);
    this.Size = new Size(289, 280);
    this.Resize += new EventHandler(this.MapRenderer_Resize);
    this.renderingPanel.ResumeLayout(false);
    this.ResumeLayout(false);
  }

  public delegate void MouseMoveOnMapHandler(object sender, MouseOnMapEventArgs args);

  public delegate void MouseDownOnMapHandler(object sender, MouseOnMapEventArgs args);

  public delegate void MapDrawnAtTileLevelHandler(List<Point> list);

  public delegate void MapDrawnAtSubTileLevelHandler(List<SubTilePoint> list);
}
