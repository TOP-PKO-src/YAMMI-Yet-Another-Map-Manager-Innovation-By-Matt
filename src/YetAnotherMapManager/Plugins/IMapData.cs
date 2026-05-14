using YetAnotherMapManager.MapData;

#nullable disable
namespace YetAnotherMapManager.Plugins;

public interface IMapData
{
  void InitializeData(MapInfo map);

  void InitializeData(int width, int height);

  void CopyArea(
    IMapData toMapData,
    int toX,
    int toY,
    int fromX,
    int fromY,
    int width,
    int height);

  IMapData CloneArea(int fromX, int fromY, int width, int height);
}
