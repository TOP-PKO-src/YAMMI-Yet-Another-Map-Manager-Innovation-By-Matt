using System;
using System.Windows.Forms;
using YetAnotherMapManager.Core;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins;
using YetAnotherMapManager.Plugins.Native.Data;

#nullable disable
namespace AutoTextureBlending;

public class AutoTextureBlending : IPlugin
{
  private ApplicationControler appCore;

  public void Init(ApplicationControler appCore)
  {
    Console.Out.WriteLine(">> Initialisation of [Auto Texture Blending] Plugin.");
    this.appCore = appCore;
    appCore.RegisterEntryPoint("Apply texture blending", new EventHandler(this.ApplyTextureBlending));
  }

  public void Unload()
  {
    Console.Out.WriteLine(">> Unloading [Auto Texture Blending] Plugin.");
    this.appCore = (ApplicationControler) null;
  }

  public void ApplyTextureBlending(object sender, EventArgs args)
  {
    if (this.appCore == null)
      return;
    MapInfo mapForEdition = this.appCore.GetMapForEdition();
    if (mapForEdition == null)
      return;
    TextureBlendingMapData data1 = (TextureBlendingMapData) mapForEdition.GetCustomData(TextureBlendingMapData.KEY);
    if (data1 == null)
    {
      data1 = new TextureBlendingMapData();
      data1.InitializeData(mapForEdition);
      mapForEdition.AddCustomData(TextureBlendingMapData.KEY, (IMapData) data1);
    }
    TextureMapData data2 = (TextureMapData) mapForEdition.GetCustomData(TextureMapData.KEY);
    if (data2 == null)
    {
      data2 = new TextureMapData();
      data2.InitializeData(mapForEdition);
      mapForEdition.AddCustomData(TextureMapData.KEY, (IMapData) data2);
    }
    for (int y = 0; y < mapForEdition.Height; ++y)
    {
      for (int x = 0; x < mapForEdition.Width; ++x)
      {
        byte num1 = data2[x, y];
        if (num1 >= (byte) 0)
        {
          byte num2 = 0;
          byte num3 = 0;
          byte num4 = 0;
          if (x - 1 < mapForEdition.Width && y - 1 < mapForEdition.Height)
          {
            if (x - 1 >= 0 && y - 1 >= 0)
              num3 = data2[x - 1, y - 1];
            else if (x - 1 >= 0 || y - 1 >= 0)
            {
              if (x - 1 < 0)
                num3 = data2[x, y - 1];
              else if (y - 1 < 0)
                num3 = data2[x - 1, y];
            }
          }
          if (x < mapForEdition.Width && y - 1 < mapForEdition.Height && y - 1 >= 0)
            num2 = data2[x, y - 1];
          if (x - 1 < mapForEdition.Width && y < mapForEdition.Height && x - 1 >= 0)
            num4 = data2[x - 1, y];
          uint num5 = 0;
          for (int index1 = 2; index1 >= 0; --index1)
          {
            uint num6 = 0;
            uint num7 = 0;
            if (num2 > (byte) 0 && (int) num2 != (int) num1)
            {
              if (num6 == 0U)
              {
                num6 = (uint) num2;
                num7 |= 4U;
                num2 = (byte) 0;
              }
              else if ((int) num6 == (int) num2)
              {
                num7 |= 4U;
                num2 = (byte) 0;
              }
            }
            if (num3 > (byte) 0 && (int) num3 != (int) num1)
            {
              if (num6 == 0U)
              {
                num6 = (uint) num3;
                num7 |= 8U;
                num3 = (byte) 0;
              }
              else if ((int) num6 == (int) num3)
              {
                num7 |= 8U;
                num3 = (byte) 0;
              }
            }
            if (num4 > (byte) 0 && (int) num4 != (int) num1)
            {
              if (num6 == 0U)
              {
                num6 = (uint) num4;
                num7 |= 2U;
                num4 = (byte) 0;
              }
              else if ((int) num6 == (int) num4)
              {
                num7 |= 2U;
                num4 = (byte) 0;
              }
            }
            if (num6 != 0U && num7 != 0U)
            {
              uint num8 = (num6 << 4 | num7) << 2;
              for (int index2 = 0; index2 < index1; ++index2)
                num8 <<= 10;
              num5 |= num8;
            }
            else
              break;
          }
          data1[x, y] = num5;
        }
      }
    }
    int num = (int) MessageBox.Show("Blending applied !");
  }
}
