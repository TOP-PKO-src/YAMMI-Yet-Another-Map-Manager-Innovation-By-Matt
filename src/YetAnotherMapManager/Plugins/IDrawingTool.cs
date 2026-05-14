using System.Collections.Generic;
using System.Drawing;
using YetAnotherMapManager.Rendering;

#nullable disable
namespace YetAnotherMapManager.Plugins;

public interface IDrawingTool
{
  bool IsTileLevelTool();

  bool IsSubTileLevelTool();

  Bitmap GetIconPicture();

  void DrawTileChanges(List<Point> list);

  void DrawSubTileChanges(List<SubTilePoint> list);

  List<string> GetRendererForcedUpdateList();
}
