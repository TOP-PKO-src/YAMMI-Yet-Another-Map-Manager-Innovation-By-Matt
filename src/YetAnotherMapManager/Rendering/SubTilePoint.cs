#nullable disable
namespace YetAnotherMapManager.Rendering;

public class SubTilePoint
{
  private readonly int x;
  private readonly int y;

  public int X => this.x / 2;

  public int Y => this.y / 2;

  public int SubX => this.x % 2;

  public int SubY => this.y % 2;

  public SubTilePoint(int X, int Y, int SubX, int SubY)
  {
    this.x = X * 2 + SubX;
    this.y = Y * 2 + SubY;
  }

  public override bool Equals(object obj)
  {
    return obj != null && obj is SubTilePoint subTilePoint && this.x == subTilePoint.x && this.y == subTilePoint.y;
  }

  public bool Equals(SubTilePoint p) => p != null && this.x == p.x && this.y == p.y;

  public override int GetHashCode() => this.x ^ this.y;
}
