using System.Collections.Generic;
using System.Drawing;
using System.Linq;

#nullable disable
namespace YetAnotherMapManager.Plugins.Native.Renderer;

public class CachedAreaRenderer : AreaRenderer, ICachedRenderer
{
  private Dictionary<Rectangle, Bitmap> cache;

  public CachedAreaRenderer() => this.cache = new Dictionary<Rectangle, Bitmap>();

  public void InvalidateCache() => this.cache.Clear();

  public void InvalidateCache(Rectangle rectangle) => this.cache.Remove(rectangle);

  public override Bitmap GetRenderedArea(int originX, int originY, int width, int height)
  {
    Rectangle key = new Rectangle(originX, originY, width, height);
    Bitmap renderedArea;
    if (!this.cache.Keys.Contains<Rectangle>(key))
    {
      renderedArea = base.GetRenderedArea(originX, originY, width, height);
      this.cache[key] = renderedArea;
    }
    else
      renderedArea = this.cache[key];
    return renderedArea;
  }
}
