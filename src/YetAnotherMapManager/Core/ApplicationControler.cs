using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.MapData.PkoNative;
using YetAnotherMapManager.Plugins;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Plugins.Native.Renderer;
using YetAnotherMapManager.Rendering;
using YetAnotherMapManager.UI;

#nullable disable
namespace YetAnotherMapManager.Core;

public class ApplicationControler
{
  private MapInfo currentMap;
  private Settings settings;
  private Dictionary<string, System.Type> MapCustomDataList;
  private Dictionary<string, IRenderer> RendererList;
  private bool allDataSaved = true;
  private Dictionary<string, EventHandler> entryPoints;
  private Dictionary<string, UserControl> views;
  private MapRenderer renderer;
  private static string LAYER_SELECTOR = "Layers";
  private static string TEXTURE_SETTINGS = "Textures";
  private static string AREA_SETTINGS = "Areas";
  private static string SETTINGS_FILE = "settings.xml";
  private static string CACHE_STORE = "cache/";
  private static string TERRAIN_TEXTURE_CACHE_STORE = ApplicationControler.CACHE_STORE + "textures/terrains/";

  public Settings Settings => this.settings;

  public List<TextureDef> TextureList
  {
    get => this.settings == null ? (List<TextureDef>) null : this.settings.Textures;
  }

  public List<AreaDef> AreaList
  {
    get => this.settings == null ? (List<AreaDef>) null : this.settings.Areas;
  }

  public bool AllDataSaved => this.allDataSaved;

  public Dictionary<string, EventHandler> EntryPoints => this.entryPoints;

  public Dictionary<string, UserControl> Views => this.views;

  public ApplicationControler(MapRenderer renderer)
  {
    this.renderer = renderer;
    this.MapCustomDataList = new Dictionary<string, System.Type>();
    this.RendererList = new Dictionary<string, IRenderer>();
    this.renderer.RendererList = this.RendererList;
    this.entryPoints = new Dictionary<string, EventHandler>();
    this.views = new Dictionary<string, UserControl>();
  }

  public event ApplicationControler.MapLoadedHandler MapLoaded;

  protected virtual void OnMapLoaded(EventArgs args)
  {
    if (this.MapLoaded == null)
      return;
    this.MapLoaded((object) this, args);
  }

  public bool Init()
  {
    bool flag = this.LoadApplicationSettings();
    if (!flag)
      return flag;
    this.SaveSettings();
    this.LoadNativeDataHandlers();
    this.LoadNativeViews();
    this.LoadNativeRenderers();
    foreach (IPlugin plugin in this.GetPlugins())
      plugin.Init(this);
    return true;
  }

  public bool LoadApplicationSettings()
  {
    if (File.Exists(ApplicationControler.SETTINGS_FILE))
    {
      this.settings = SettingsSerializer.LoadFromXML(ApplicationControler.SETTINGS_FILE);
      foreach (TextureDef texture in this.settings.Textures)
      {
        try
        {
          texture.Texture = new Bitmap(texture.CachedBitmapPath);
        }
        catch (Exception)
        {
        }
      }
    }
    if (this.settings == null)
    {
      if (MessageBox.Show("Map manager cannot find your settings.\nDo you want to extract settings from your game client now ?", "Warning", MessageBoxButtons.YesNo) != DialogResult.Yes)
        return false;
      FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
      folderBrowserDialog.Description = "Select your game client folder";
      if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
        return false;
      this.LoadApplicationSettings(folderBrowserDialog.SelectedPath);
      folderBrowserDialog.Dispose();
    }
    return true;
  }

  public void LoadApplicationSettings(string gameClientPath)
  {
    Console.Out.WriteLine($"Extracting settings from client [{gameClientPath}]");
    this.settings = new Settings();
    this.settings.GameClient = gameClientPath;
    this.RefreshTexturesFromGameClient();
    this.RefreshAreaListFromGameClient();
    this.SaveSettings();
  }

  public void LoadNativeDataHandlers()
  {
    this.RegisterMapData(SeaLandMapData.KEY, typeof (SeaLandMapData));
    this.RegisterMapData(SolidMapData.KEY, typeof (SolidMapData));
    this.RegisterMapData(HeightMapData.KEY, typeof (HeightMapData));
    this.RegisterMapData(TextureMapData.KEY, typeof (TextureMapData));
    this.RegisterMapData(AreaMapData.KEY, typeof (AreaMapData));
    this.RegisterMapData(PvpMapData.KEY, typeof (PvpMapData));
    this.RegisterMapData(SafeZoneMapData.KEY, typeof (SafeZoneMapData));
    this.RegisterMapData(HeightMapSpotData.KEY, typeof (HeightMapSpotData));
    this.RegisterMapData(ColorMapData.KEY, typeof (ColorMapData));
  }

  public void LoadNativeRenderers()
  {
    this.RegisterRenderer(SeaLandRenderer.KEY, true, 1f, (IRenderer) new CachedSeaLandRenderer());
    this.RegisterRenderer(HeightRenderer.KEY, false, 1f, (IRenderer) new CachedHeightRenderer());
    this.RegisterRenderer(HeightSpotRenderer.KEY, false, 1f, (IRenderer) new HeightSpotRenderer());
    this.RegisterRenderer(SolidRenderer.KEY, true, 1f, (IRenderer) new CachedSolidRenderer());
    TextureRenderer textureRenderer = (TextureRenderer) new CachedTextureRenderer();
    textureRenderer.SetColorMap(this.settings);
    this.RegisterRenderer(TextureRenderer.KEY, false, 0.5f, (IRenderer) textureRenderer);
    AreaRenderer areaRenderer = (AreaRenderer) new CachedAreaRenderer();
    areaRenderer.SetColorMap(this.settings);
    this.RegisterRenderer(AreaRenderer.KEY, false, 1f, (IRenderer) areaRenderer);
    this.RegisterRenderer(PvpRenderer.KEY, false, 0.5f, (IRenderer) new CachedPvpRenderer());
    this.RegisterRenderer(SafeZoneRenderer.KEY, false, 0.5f, (IRenderer) new CachedSafeZoneRenderer());
    this.RegisterRenderer(ColorRenderer.KEY, false, 0.5f, (IRenderer) new ColorRenderer());
  }

  public void LoadNativeDrawingTools()
  {
  }

  public void LoadNativeViews()
  {
    LayerSelector layerSelector = new LayerSelector();
    this.views.Add(ApplicationControler.LAYER_SELECTOR, (UserControl) layerSelector);
    layerSelector.SettingsChanged += new LayerSelector.SettingsChangedHandler(this.layerSelector_SettingsChanged);
    TextureSettings textureSettings = new TextureSettings();
    foreach (TextureDef texture in this.settings.Textures)
      textureSettings.AddTexture(texture);
    this.views.Add(ApplicationControler.TEXTURE_SETTINGS, (UserControl) textureSettings);
    textureSettings.TextureSettingsChanged += new TextureSettings.TextureSettingsChangedHandler(this.OnTextureSettingsChanged);
    AreaSettings areaSettings = new AreaSettings();
    for (int count = this.settings.Areas.Count; count > 0; --count)
      areaSettings.AddArea(this.settings.Areas[count - 1]);
    this.views.Add(ApplicationControler.AREA_SETTINGS, (UserControl) areaSettings);
    areaSettings.AreaSettingsChanged += new AreaSettings.AreaSettingsChangedHandler(this.OnAreaSettingsChanged);
  }

  private void layerSelector_SettingsChanged(object sender, SettingsChangedEventArgs args)
  {
    IRenderer renderer = this.RendererList[args.Name];
    renderer.SetAlphaLevel(args.Alpha);
    renderer.SetEnabled(args.Displayed);
    this.renderer.Refresh();
  }

  public List<IPlugin> GetPlugins()
  {
    FileInfo[] files = new DirectoryInfo(Path.Combine(Environment.CurrentDirectory, "Plugins")).GetFiles("*.dll");
    List<Assembly> assemblyList = new List<Assembly>();
    if (files != null)
    {
      foreach (FileInfo fileInfo in files)
        assemblyList.Add(Assembly.LoadFile(fileInfo.FullName));
    }
    List<System.Type> typeList = new List<System.Type>();
    foreach (Assembly assembly in assemblyList)
    {
      try
      {
        typeList.AddRange((IEnumerable<System.Type>) assembly.GetTypes());
      }
      catch (ReflectionTypeLoadException ex)
      {
        foreach (Exception loaderException in ex.LoaderExceptions)
        {
          int num = (int) MessageBox.Show($"{loaderException.Message}\n{loaderException.StackTrace}");
        }
      }
    }
    return typeList.FindAll((Predicate<System.Type>) (t => new List<System.Type>((IEnumerable<System.Type>) t.GetInterfaces()).Contains(typeof (IPlugin)))).ConvertAll<IPlugin>((Converter<System.Type, IPlugin>) (t => Activator.CreateInstance(t) as IPlugin));
  }

  private void SetMap(MapInfo map)
  {
    this.currentMap = map;
    this.renderer.Map = map;
    foreach (IRenderer renderer in this.RendererList.Values)
    {
      if (new List<System.Type>((IEnumerable<System.Type>) renderer.GetType().GetInterfaces()).Contains(typeof (ICachedRenderer)))
        ((ICachedRenderer) renderer).InvalidateCache();
      renderer.SetMap(map);
    }
    this.renderer.MouseMovedOnMap += new MapRenderer.MouseMoveOnMapHandler(this.renderer_MouseMovedOnMap);
    this.OnMapLoaded(new EventArgs());
  }

  public void ApplyTileMapDrawing(IDrawingTool drawingTool, List<Point> drawnTileList)
  {
    try
    {
      drawingTool.DrawTileChanges(drawnTileList);
      foreach (string rendererForcedUpdate in drawingTool.GetRendererForcedUpdateList())
      {
        IRenderer renderer = this.RendererList[rendererForcedUpdate];
        if (new List<System.Type>((IEnumerable<System.Type>) renderer.GetType().GetInterfaces()).Contains(typeof (ICachedRenderer)))
          ((ICachedRenderer) renderer).InvalidateCache();
      }
      this.renderer.Refresh();
    }
    catch (Exception)
    {
      int num = (int) MessageBox.Show("Wow !! Something nasty happened. Try to save your work if you still can !! Built-in bug by Matt !!");
    }
  }

  private void renderer_MouseMovedOnMap(object sender, MouseOnMapEventArgs args)
  {
  }

  public void RefreshTexturesFromGameClient()
  {
    this.settings.ClearTextures();
    if (!Directory.Exists(ApplicationControler.TERRAIN_TEXTURE_CACHE_STORE))
      Directory.CreateDirectory(ApplicationControler.TERRAIN_TEXTURE_CACHE_STORE);
    foreach (TextureDef loadTexture in BinFileConvertor.LoadTextures(this.settings.GameClient, ApplicationControler.TERRAIN_TEXTURE_CACHE_STORE))
      this.settings.AddTextureDef(loadTexture);
    this.SaveSettings();
  }

  public void RefreshAreaListFromGameClient()
  {
    this.settings.ClearAreas();
    foreach (AreaDef loadArea in BinFileConvertor.LoadAreas(this.settings.GameClient))
      this.settings.AddAreaDef(loadArea);
    this.SaveSettings();
  }

  private void OnTextureSettingsChanged(object sender, TextureSettingsChangedEventArgs args)
  {
    this.SaveSettings();
    ((TextureRenderer) this.RendererList[TextureRenderer.KEY]).SetColorMap(this.settings);
    this.renderer.Refresh();
  }

  private void OnAreaSettingsChanged(object sender, AreaSettingsChangedEventArgs args)
  {
    this.SaveSettings();
    ((AreaRenderer) this.RendererList[AreaRenderer.KEY]).SetColorMap(this.settings);
    this.renderer.Refresh();
  }

  private void SaveSettings()
  {
    SettingsSerializer.SaveAsXML(this.settings, ApplicationControler.SETTINGS_FILE);
  }

  public void RegisterEntryPoint(string name, EventHandler eventHandler)
  {
    this.entryPoints.Add(name, eventHandler);
  }

  public void RegisterView(string name, UserControl control) => this.views.Add(name, control);

  public void RegisterRenderer(
    string name,
    bool selectedByDefault,
    float defaultAlphaLevel,
    IRenderer renderer)
  {
    ((LayerSelector) this.views[ApplicationControler.LAYER_SELECTOR]).AddLayer(name, selectedByDefault, defaultAlphaLevel);
    renderer.SetAlphaLevel(defaultAlphaLevel);
    renderer.SetEnabled(selectedByDefault);
    this.RendererList.Add(name, renderer);
  }

  public void RegisterMapData(string key, System.Type type)
  {
    if (!new List<System.Type>((IEnumerable<System.Type>) type.GetInterfaces()).Contains(typeof (IMapData)))
      throw new Exception("Map custom data must be of type [IMapData]");
    this.MapCustomDataList.Add(key, type);
  }

  public void RegisterMouseMovedOnMapEventHandler(MapRenderer.MouseMoveOnMapHandler handler)
  {
    this.renderer.MouseMovedOnMap += new MapRenderer.MouseMoveOnMapHandler(handler.Invoke);
  }

  public void ImportNativeMapFile(string filename)
  {
    MapInfo map = PkoMapConvertor.LoadMap(filename);
    if (map != null)
    {
      this.SetMap(map);
    }
    else
    {
      int num = (int) MessageBox.Show($"Failed to load map file [{filename}]");
    }
  }

  public void ExportNativeMapFile(string filename)
  {
    PkoMapConvertor.WriteMap(this.settings.L, filename, this.currentMap);
  }

  public void ExportNativeAtrFile(string filename)
  {
    PkoMapConvertor.WriteATR(filename, this.currentMap);
  }

  public void ExportNativeBlkFile(string filename)
  {
    PkoMapConvertor.WriteBLK(filename, this.currentMap);
  }

  public void LoadProjectFromDisk(string filename)
  {
    if (!this.CloseCurrentProject())
      return;
    MapInfo map = MapInfoSerializer.Unserialize(filename);
    if (map == null)
      return;
    this.SetMap(map);
    this.allDataSaved = true;
    this.renderer.Refresh();
  }

  public void SaveProjectToDisk(string filename)
  {
    if (this.currentMap == null)
      return;
    try
    {
      MapInfoSerializer.Serialize(this.currentMap, filename);
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show($"Failed to save current map !\n{ex.Message}\n{ex.StackTrace}");
    }
    this.allDataSaved = true;
  }

  public bool CloseCurrentProject()
  {
    if (this.currentMap != null)
    {
      if (!this.AllDataSaved && MessageBox.Show("Current map will be lost.", "Warning", MessageBoxButtons.OKCancel) != DialogResult.OK)
        return false;
      this.currentMap = (MapInfo) null;
    }
    return true;
  }

  public bool CreateMap(int width, int height)
  {
    if (!this.CloseCurrentProject())
      return false;
    width += width % 8;
    height += height % 8;
    this.SetMap(new MapInfo(width, height));
    foreach (string key in this.MapCustomDataList.Keys)
    {
      IMapData instance = (IMapData) Activator.CreateInstance(this.MapCustomDataList[key]);
      instance.InitializeData(this.currentMap);
      this.currentMap.AddCustomData(key, instance);
    }
    this.renderer.Map = this.currentMap;
    this.allDataSaved = false;
    return true;
  }

  public MapInfo GetMapForEdition() => this.currentMap;

  public MapInfo GetMapForReading() => this.currentMap;

  public void ReleaseMap()
  {
    this.renderer.Refresh();
    this.SetMap(this.currentMap);
  }

  public void CropMap(int fromX, int fromY, int toX, int toY)
  {
    MapInfo mapForEdition = this.GetMapForEdition();
    if (fromX < 0)
      fromX = 0;
    if (fromY < 0)
      fromY = 0;
    if (toX > mapForEdition.Width - 1)
      toX = mapForEdition.Width - 1;
    if (toY > mapForEdition.Height - 1)
      toY = mapForEdition.Height - 1;
    int num1 = toX - fromX + 1;
    int num2 = toY - fromY + 1;
    int num3 = num1 + 7;
    int num4 = num2 + 7;
    int width = num3 - num3 % 8;
    int height = num4 - num4 % 8;
    if (width < 8 || height < 8)
    {
      int num5 = (int) MessageBox.Show($"Crop size too small, operation aborted.\nFrom [{(object) fromX},{(object) fromY}] to [{(object) toX},{(object) toY}]");
    }
    else
    {
      MapInfo map = new MapInfo(width, height);
      foreach (string customDataKey in mapForEdition.CustomDataKeys)
      {
        IMapData customData = mapForEdition.GetCustomData(customDataKey);
        map.AddCustomData(customDataKey, customData.CloneArea(fromX, fromY, width, height));
      }
      this.SetMap(map);
      this.ReleaseMap();
    }
  }

  public void ImportSubMap(
    string filename,
    int fromX,
    int fromY,
    int toX,
    int toY,
    int targetX,
    int targetY)
  {
    MapInfo mapForEdition = this.GetMapForEdition();
    if (targetX < 0 || targetX >= mapForEdition.Width - 1 || targetY < 0 || targetY >= mapForEdition.Height - 1)
    {
      int num1 = (int) MessageBox.Show("Target Location out of bounds.");
    }
    else
    {
      MapInfo mapInfo;
      int val1_1;
      int val1_2;
      try
      {
        mapInfo = PkoMapConvertor.LoadMap(filename);
        if (toX >= mapInfo.Width)
          toX = mapInfo.Width - 1;
        if (toY >= mapInfo.Height)
          toY = mapInfo.Height - 1;
        val1_1 = toX - fromX + 1;
        val1_2 = toY - fromY + 1;
        if (fromX < mapInfo.Width && fromY < mapInfo.Height && val1_1 > 0)
        {
          if (val1_2 > 0)
            goto label_11;
        }
        int num2 = (int) MessageBox.Show("Nothing to import");
        return;
      }
      catch
      {
        int num3 = (int) MessageBox.Show("Failed to import from selected map file : " + filename);
        return;
      }
label_11:
      int width = Math.Min(val1_1, mapForEdition.Width - targetX - 1);
      int height = Math.Min(val1_2, mapForEdition.Height - targetY - 1);
      foreach (string customDataKey in mapInfo.CustomDataKeys)
      {
        IMapData customData = mapInfo.GetCustomData(customDataKey);
        IMapData mapData = mapForEdition.GetCustomData(customDataKey);
        if (mapData == null)
        {
          mapData = (IMapData) Activator.CreateInstance(customData.GetType());
          mapData.InitializeData(mapForEdition);
          mapForEdition.AddCustomData(customDataKey, mapData);
        }
        customData.CopyArea(mapData, targetX, targetY, fromX, fromY, width, height);
      }
      this.ReleaseMap();
    }
  }

  public void Test()
  {
    TextureMapData customData = (TextureMapData) this.currentMap.GetCustomData(TextureMapData.KEY);
    int num = 0;
    for (int x = 0; x < this.currentMap.Width; ++x)
    {
      for (int y = 0; y < this.currentMap.Height; ++y)
      {
        customData[x, y] = (byte) this.settings.Textures[num % this.settings.Textures.Count].Id;
        ++num;
      }
    }
  }

  public delegate void MapLoadedHandler(object sender, EventArgs args);
}
