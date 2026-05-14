using System.Collections.Generic;

#nullable disable
namespace YetAnotherMapManager.MapData.PkoNative;

internal struct _mapProperties
{
  public int magic;
  public int mapWidth;
  public int mapHeight;
  public int widthBreakdownSize;
  public int heightBreakdownSize;
  public int xDivisionCount;
  public int yDivisionCount;
  public List<int> offsetList;
}
