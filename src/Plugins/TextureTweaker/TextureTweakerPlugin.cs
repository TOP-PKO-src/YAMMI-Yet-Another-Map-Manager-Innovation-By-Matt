using System;
using System.Windows.Forms;
using YetAnotherMapManager.Core;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins;
using YetAnotherMapManager.Plugins.Native.Data;

#nullable disable
namespace TextureTweaker;

public class TextureTweakerPlugin : IPlugin
{
  private ApplicationControler appCore;

  public void Init(ApplicationControler appCore)
  {
    Console.Out.WriteLine(">> Initialisation of [Texture Tweaker] Plugin.");
    this.appCore = appCore;
    appCore.RegisterEntryPoint("Texture Tweaker", new EventHandler(this.TextureTweakerEntryPoint));
  }

  public void Unload()
  {
    Console.Out.WriteLine(">> Unloading [Texture Tweaker] Plugin.");
    this.appCore = (ApplicationControler) null;
  }

  public void TextureTweakerEntryPoint(object sender, EventArgs args)
  {
    MapInfo mapForEdition = this.appCore.GetMapForEdition();
    if (mapForEdition == null)
    {
      int num1 = (int) MessageBox.Show("No map loaded !");
    }
    else
    {
      int num2 = (int) new TextureTweakerForm(this.appCore.Settings.Textures, (TextureMapData) mapForEdition.GetCustomData(TextureMapData.KEY), (TextureBlendingMapData) mapForEdition.GetCustomData(TextureBlendingMapData.KEY), (SeaLandMapData) mapForEdition.GetCustomData(SeaLandMapData.KEY)).ShowDialog();
    }
  }
}
