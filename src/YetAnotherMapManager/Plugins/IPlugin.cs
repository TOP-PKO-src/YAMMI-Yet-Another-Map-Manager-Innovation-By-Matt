using YetAnotherMapManager.Core;

#nullable disable
namespace YetAnotherMapManager.Plugins;

public interface IPlugin
{
  void Init(ApplicationControler appCore);

  void Unload();
}
