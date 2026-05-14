#nullable disable
namespace IsleGenerator.dll;

public struct Point(int p1, int p2)
{
  public int x = p1;
  public int y = p2;

  public override bool Equals(object obj)
  {
    switch (obj)
    {
      case Point _:
      case null:
        Point point = (Point) obj;
        return this.x == point.x && this.y == point.y;
      default:
        return false;
    }
  }

  public override int GetHashCode() => this.x;
}
