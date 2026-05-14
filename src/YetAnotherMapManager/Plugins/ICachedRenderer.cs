using System.Drawing;

#nullable disable
namespace YetAnotherMapManager.Plugins;

public interface ICachedRenderer
{
  void InvalidateCache();

  void InvalidateCache(Rectangle rectangle);
}
