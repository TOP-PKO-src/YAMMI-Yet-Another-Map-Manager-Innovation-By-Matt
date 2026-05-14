using System.Windows.Forms;
using YetAnotherMapManager.Core;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins;
using YetAnotherMapManager.Plugins.Native.Data;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace TileInfoView;

public class TileInfoView : IPlugin
{
  private ApplicationControler appCore;
  private InfoViewForm view;

  public void Init(ApplicationControler applicationCore)
  {
    this.appCore = applicationCore;
    this.view = new InfoViewForm();
    this.appCore.RegisterView("Map Info", (UserControl) this.view);
    this.appCore.RegisterMouseMovedOnMapEventHandler(new MapRenderer.MouseMoveOnMapHandler(this.mouseMovedOnMap));
  }

  public void Unload()
  {
  }

  private void mouseMovedOnMap(object sender, MouseOnMapEventArgs args)
  {
    if (!args.InMap)
    {
      this.view.MapLocation = "~ Not in map ~";
      this.view.Area = "~ Not in map ~";
    }
    else
    {
      this.view.MapLocation = $"{(object) args.Location.X},{(object) args.Location.Y} [{(object) args.Coords.X},{(object) args.Coords.Y}]";
      MapInfo mapForReading = this.appCore.GetMapForReading();
      SeaLandMapData customData1 = (SeaLandMapData) mapForReading.GetCustomData(SeaLandMapData.KEY);
      HeightMapData customData2 = (HeightMapData) mapForReading.GetCustomData(HeightMapData.KEY);
      SolidMapData customData3 = (SolidMapData) mapForReading.GetCustomData(SolidMapData.KEY);
      SolidHeightMapData customData4 = (SolidHeightMapData) mapForReading.GetCustomData(SolidHeightMapData.KEY);
      TextureMapData customData5 = (TextureMapData) mapForReading.GetCustomData(TextureMapData.KEY);
      TextureBlendingMapData customData6 = (TextureBlendingMapData) mapForReading.GetCustomData(TextureBlendingMapData.KEY);
      AreaMapData customData7 = (AreaMapData) mapForReading.GetCustomData(AreaMapData.KEY);
      short tileType = (short) customData1.GetTileType(args.Coords.X, args.Coords.Y);
      this.view.TileType = (int) tileType != (int) SeaLandMapData.TYPE_SEA ? ((int) tileType != (int) SeaLandMapData.TYPE_LAND ? "Bridge" : "Land") : "Sea";
      sbyte num1 = customData2[args.Coords.X, args.Coords.Y];
      this.view.TileHeight = $"{(num1 != (sbyte) 0 || (int) tileType != (int) SeaLandMapData.TYPE_SEA ? num1.ToString() : "Offshore")} [Min={customData2.MinHeight.ToString()};Max={customData2.MaxHeight.ToString()}]";
      int num2 = (int) customData7[args.Coords.X, args.Coords.Y];
      if (num2 == 0)
      {
        this.view.Area = "No area assigned";
      }
      else
      {
        AreaDef areaDef = (AreaDef) null;
        if (this.appCore.AreaList != null)
        {
          foreach (AreaDef area in this.appCore.AreaList)
          {
            if (area.Id == num2)
              areaDef = area;
          }
        }
        if (areaDef != null)
          this.view.Area = $"[{(object) num2}] {areaDef.Name}";
        else
          this.view.Area = $"[{(object) num2}] ## Undefined ##";
      }
    }
  }
}
