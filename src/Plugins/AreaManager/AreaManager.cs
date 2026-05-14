using System;
using YetAnotherMapManager.Core;
using YetAnotherMapManager.Plugins;

#nullable disable
namespace AreaManager;

public class AreaManager : IPlugin
{
  private ApplicationControler appCore;

  public void Init(ApplicationControler appCore)
  {
    Console.Out.WriteLine(">> Initialisation of [Area Manager] Plugin.");
    this.appCore = appCore;
    appCore.RegisterEntryPoint("Area Manager", new EventHandler(this.OpenAreaManagerEntryPoint));
  }

  public void Unload()
  {
    Console.Out.WriteLine(">> Unloading [Area Manager] Plugin.");
    this.appCore = (ApplicationControler) null;
  }

  public void OpenAreaManagerEntryPoint(object sender, EventArgs args)
  {
    using (AreaManagerForm areaManagerForm = new AreaManagerForm())
    {
      int num = (int) areaManagerForm.ShowDialog();
    }
  }
}
