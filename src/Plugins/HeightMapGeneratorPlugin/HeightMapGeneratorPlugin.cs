using System;
using System.Drawing;
using System.Windows.Forms;
using YetAnotherMapManager.Common;
using YetAnotherMapManager.Core;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins;
using YetAnotherMapManager.Plugins.Native.Data;

#nullable disable
namespace HeightMapGeneratorPlugin;

public class HeightMapGeneratorPlugin : IPlugin
{
  private ApplicationControler appCore;

  public void Init(ApplicationControler appCore)
  {
    Console.Out.WriteLine(">> Initialisation of [Height Map Generator] Plugin.");
    this.appCore = appCore;
    appCore.RegisterEntryPoint("Height Map Generator", new EventHandler(this.GenerateHeightMap));
  }

  public void Unload()
  {
    Console.Out.WriteLine(">> Unloading [Height Map Generator] Plugin.");
    this.appCore = (ApplicationControler) null;
  }

  public void GenerateHeightMap(object sender, EventArgs args)
  {
    sbyte hightValue = 60;
    sbyte lowValue = -30;
    sbyte variability = 10;
    try
    {
      Cursor current = Cursor.Current;
      Cursor.Current = Cursors.WaitCursor;
      this.GenerateHeightMap(hightValue, lowValue, (int) variability);
      Cursor.Current = current;
    }
    catch (Exception ex)
    {
      int num = (int) MessageBox.Show($"{ex.Message}\n{ex.StackTrace}");
    }
  }

  private void GenerateHeightMap(sbyte hightValue, sbyte lowValue, int variability)
  {
    bool flag = MessageBox.Show("Do you want to apply height automatic softening ?", "Height map generation", MessageBoxButtons.YesNo) == DialogResult.Yes;
    MapInfo mapForEdition = this.appCore.GetMapForEdition();
    if (mapForEdition == null)
      return;
    SeaLandMapData customData1 = (SeaLandMapData) mapForEdition.GetCustomData(SeaLandMapData.KEY);
    HeightMapSpotData customData2 = (HeightMapSpotData) mapForEdition.GetCustomData(HeightMapSpotData.KEY);
    if (customData1 == null)
      return;
    SByteArray_2D sbyteArray2D = new SByteArray_2D(mapForEdition.Width, mapForEdition.Height, sbyte.MinValue);
    this.GenerateSeaLandHeight(mapForEdition.Width, mapForEdition.Height, customData1, sbyteArray2D);
    if (customData2 != null)
      this.MergeHeightSpots(customData2, sbyteArray2D);
    float[,] matrix = new float[5, 5]
    {
      {
        1f,
        1f,
        1f,
        1f,
        1f
      },
      {
        1f,
        1f,
        1f,
        1f,
        1f
      },
      {
        1f,
        1f,
        1f,
        1f,
        1f
      },
      {
        1f,
        1f,
        1f,
        1f,
        1f
      },
      {
        1f,
        1f,
        1f,
        1f,
        1f
      }
    };
    if (flag)
      sbyteArray2D = sbyteArray2D.applyConvolution(matrix);
    ((HeightMapData) mapForEdition.GetCustomData(HeightMapData.KEY)).OverwriteData(sbyteArray2D);
    this.appCore.ReleaseMap();
  }

  public void MergeHeightSpots(HeightMapSpotData heightSpotData, SByteArray_2D tempHeightMap)
  {
    foreach (Point key in heightSpotData.HeightSpotList.Keys)
      tempHeightMap[key.X, key.Y] = heightSpotData.HeightSpotList[key];
  }

  public void GenerateSeaLandHeight(
    int width,
    int height,
    SeaLandMapData seaLandData,
    SByteArray_2D heightMap)
  {
    for (int x = 0; x < width; ++x)
    {
      for (int y = 0; y < height; ++y)
        heightMap[x, y] = (int) seaLandData[x, y] != (int) SeaLandMapData.TYPE_LAND ? (sbyte) -10 : (sbyte) 5;
    }
  }

  public void SinkLand(SByteArray_2D heightMap, int loopCount)
  {
    SByteArray_2D sbyteArray2D = new SByteArray_2D(heightMap.Width, heightMap.Height, (sbyte) 0);
    for (int index1 = 0; index1 < loopCount; ++index1)
    {
      for (int x1 = 0; x1 < heightMap.Width; ++x1)
      {
        for (int y1 = 0; y1 < heightMap.Height; ++y1)
        {
          int num1 = 0;
          int num2 = 0;
          for (int index2 = -1; index2 <= 1; ++index2)
          {
            for (int index3 = -1; index3 <= 1; ++index3)
            {
              int x2 = x1 + index2;
              int y2 = y1 + index3;
              if (x2 >= 0 && x2 < heightMap.Width && y2 >= 0 && y2 < heightMap.Height)
              {
                num1 += (int) heightMap[x2, y2];
                ++num2;
              }
            }
          }
          if (num2 > 0)
          {
            float d = (float) num1 / (float) num2;
            if ((double) d < 0.0 && (double) d < (double) heightMap[x1, y1])
              heightMap[x1, y1] = (sbyte) Math.Floor((double) d);
          }
        }
      }
    }
  }

  public void GenerateSeaBordersHeight(
    int width,
    int height,
    SeaLandMapData seaLandData,
    SByteArray_2D heightMap)
  {
    for (int x1 = 0; x1 < width; ++x1)
    {
      for (int y1 = 0; y1 < height; ++y1)
      {
        int num1 = 0;
        int num2 = 0;
        for (int index1 = -1; index1 <= 1; ++index1)
        {
          for (int index2 = -1; index2 <= 1; ++index2)
          {
            int x2 = x1 + index1;
            int y2 = y1 + index2;
            if (x2 >= 0 && x2 < width && y2 >= 0 && y2 < height)
            {
              if ((int) seaLandData[x2, y2] == (int) SeaLandMapData.TYPE_LAND)
                ++num1;
              else
                ++num2;
            }
          }
        }
        if (num1 > 0 && num2 > 0)
          heightMap[x1, y1] = (int) seaLandData[x1, y1] != (int) SeaLandMapData.TYPE_LAND ? (sbyte) -1 : (sbyte) 0;
      }
    }
  }

  public int GenerateHeight(
    SByteArray_2D heightMap,
    sbyte hightValue,
    sbyte lowValue,
    int variability)
  {
    int height = 0;
    Random random = new Random();
    for (int x1 = 0; x1 < heightMap.Width; ++x1)
    {
      for (int y1 = 0; y1 < heightMap.Height; ++y1)
      {
        int num1 = 0;
        int num2 = 0;
        if (heightMap[x1, y1] == sbyte.MinValue)
        {
          for (int index1 = -1; index1 <= 1; ++index1)
          {
            for (int index2 = -1; index2 <= 1; ++index2)
            {
              int x2 = x1 + index1;
              int y2 = y1 + index2;
              if (x2 >= 0 && x2 < heightMap.Width && y2 >= 0 && y2 < heightMap.Height && heightMap[x2, y2] != sbyte.MinValue)
              {
                ++num1;
                num2 += (int) heightMap[x2, y2];
              }
            }
          }
          if (num1 > 0)
          {
            sbyte num3 = (sbyte) (num2 / num1);
            sbyte num4;
            if ((int) num3 > (int) hightValue + variability)
              num4 = hightValue;
            else if ((int) num3 < (int) lowValue - variability)
            {
              num4 = hightValue;
            }
            else
            {
              int num5 = random.Next(-1, 1);
              num4 = (sbyte) ((double) num3 + (double) num5 * ((double) variability * random.NextDouble()));
            }
            heightMap[x1, y1] = num4;
          }
          else
            ++height;
        }
      }
    }
    return height;
  }
}
