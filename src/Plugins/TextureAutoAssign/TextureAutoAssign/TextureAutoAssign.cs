using System;
using YetAnotherMapManager.Core;
using YetAnotherMapManager.Plugins;

#nullable disable
namespace TextureAutoAssign;

public class TextureAutoAssign : IPlugin
{
  public void Init(ApplicationControler appCore)
  {
    Console.Out.WriteLine(">> Initialisation of [Texture Auto Assign] Plugin.");
  }

  public void Unload()
  {
    Console.Out.WriteLine(">> Unloading [Texture Auto Assign] Plugin.");
  }

  public void TextureAutoAssignEntryPoint(object sender, EventArgs args)
  {
  }
}
