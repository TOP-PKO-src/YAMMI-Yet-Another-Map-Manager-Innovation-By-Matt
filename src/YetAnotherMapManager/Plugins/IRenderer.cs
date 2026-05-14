using System.Drawing;
using YetAnotherMapManager.MapData;

#nullable disable
namespace YetAnotherMapManager.Plugins;

public interface IRenderer
{
  int GetScale();

  void SetMap(MapInfo map);

  bool IsEnabled();

  void SetEnabled(bool status);

  float GetAlphaLevel();

  void SetAlphaLevel(float value);

  Bitmap GetRenderedArea(int x, int y, int width, int height);
}
