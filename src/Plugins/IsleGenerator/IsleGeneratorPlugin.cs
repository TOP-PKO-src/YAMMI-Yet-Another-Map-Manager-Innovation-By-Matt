using System;
using System.Collections.Generic;
using System.Windows.Forms;
using YetAnotherMapManager.Common;
using YetAnotherMapManager.Core;
using YetAnotherMapManager.MapData;
using YetAnotherMapManager.Plugins;
using YetAnotherMapManager.Plugins.Native.Data;

#nullable disable
namespace IsleGenerator.dll;

public class IsleGeneratorPlugin : IPlugin
{
  private ApplicationControler appCore;

  public void Init(ApplicationControler appCore)
  {
    Console.Out.WriteLine(">> Initialisation of [Isle Generator] Plugin.");
    this.appCore = appCore;
    appCore.RegisterEntryPoint("Isle Generator", new EventHandler(this.GenerateIsleEntryPoint));
  }

  public void Unload()
  {
    Console.Out.WriteLine(">> Unloading [Isle Generator] Plugin.");
    this.appCore = (ApplicationControler) null;
  }

  public void GenerateIsleEntryPoint(object sender, EventArgs args)
  {
    IsleGeneratorSettingsForm generatorSettingsForm = new IsleGeneratorSettingsForm();
    int num1 = (int) generatorSettingsForm.ShowDialog();
    while (generatorSettingsForm.DialogResult == DialogResult.OK)
    {
      try
      {
        Cursor current = Cursor.Current;
        Cursor.Current = Cursors.WaitCursor;
        this.GenerateMap(generatorSettingsForm.MapWidth, generatorSettingsForm.MapHeight, generatorSettingsForm.StartPoints, generatorSettingsForm.LandCoverage, generatorSettingsForm.DensityFactor, generatorSettingsForm.DistanceToEdge, generatorSettingsForm.SmoothingRounds);
        Cursor.Current = current;
      }
      catch (Exception ex)
      {
        int num2 = (int) MessageBox.Show($"{ex.Message}\n{ex.StackTrace}");
      }
      int num3 = (int) generatorSettingsForm.ShowDialog();
    }
  }

  public void GenerateMap(
    int width,
    int height,
    int startPointCount,
    float landSeed,
    int densityFactor,
    float distanceToEdges,
    int smoothingRounds)
  {
    if (!this.appCore.CreateMap(width, height))
      return;
    int num1 = width * height;
    distanceToEdges /= 2f;
    TwoBitArray_2D twoBitArray2D = new TwoBitArray_2D(width, height);
    BitArray_2D bitArray2D = new BitArray_2D(width, height, false);
    Random random = new Random();
    Queue<Point> pointQueue = new Queue<Point>();
    Point[] pointArray1 = new Point[startPointCount];
    for (int index = 0; index < startPointCount; ++index)
    {
      pointArray1[index].x = random.Next((int) ((double) distanceToEdges * (double) width), (int) ((double) width - (double) distanceToEdges * (double) width - 1.0));
      pointArray1[index].y = random.Next((int) ((double) distanceToEdges * (double) height), (int) ((double) height - (double) distanceToEdges * (double) height - 1.0));
      pointQueue.Enqueue(pointArray1[index]);
      bitArray2D[pointArray1[index].x, pointArray1[index].y] = true;
    }
    int num2 = 0;
    while (pointQueue.Count > 0 && (double) num2 / (double) num1 < (double) landSeed)
    {
      Point point1 = pointQueue.Dequeue();
      twoBitArray2D[point1.x, point1.y] = SeaLandMapData.TYPE_LAND;
      int num3 = random.Next(0, 3);
      Point[] pointArray2 = new Point[4];
      pointArray2[num3 % 4] = new Point(point1.x - 1, point1.y);
      pointArray2[(1 + num3) % 4] = new Point(point1.x, point1.y - 1);
      pointArray2[(2 + num3) % 4] = new Point(point1.x + 1, point1.y);
      pointArray2[(3 + num3) % 4] = new Point(point1.x, point1.y + 1);
      foreach (Point point2 in pointArray2)
      {
        if (point2.x >= 0 && point2.x < width && point2.y >= 0 && point2.y < height && !bitArray2D[point2.x, point2.y])
        {
          bitArray2D[point2.x, point2.y] = true;
          if (densityFactor + (int) ((1.0 - (double) num2 / (double) num1 / (double) landSeed) * 100.0) > random.Next(0, 100))
          {
            pointQueue.Enqueue(point2);
            ++num2;
          }
        }
      }
    }
    for (int index = 0; index < smoothingRounds; ++index)
      twoBitArray2D = this.mapSmoothing(twoBitArray2D);
    SByteArray_2D sbyteArray2D = new SByteArray_2D(width, height, sbyte.MinValue);
    this.GenerateSeaLandHeight(width, height, twoBitArray2D, sbyteArray2D);
    MapInfo mapForEdition = this.appCore.GetMapForEdition();
    ((SeaLandMapData) mapForEdition.GetCustomData(SeaLandMapData.KEY)).OverwriteData(twoBitArray2D);
    ((HeightMapData) mapForEdition.GetCustomData(HeightMapData.KEY)).OverwriteData(sbyteArray2D);
    this.appCore.ReleaseMap();
  }

  public TwoBitArray_2D mapSmoothing(TwoBitArray_2D map)
  {
    TwoBitArray_2D twoBitArray2D = new TwoBitArray_2D(map.Width, map.Height);
    for (int x1 = 0; x1 < map.Width; ++x1)
    {
      for (int y1 = 0; y1 < map.Height; ++y1)
      {
        int num = 0;
        for (int index1 = -1; index1 <= 1; ++index1)
        {
          for (int index2 = -1; index2 <= 1; ++index2)
          {
            int x2 = x1 + index1;
            int y2 = y1 + index2;
            if (x2 >= 0 && x2 < map.Width && y2 >= 0 && y2 < map.Height && (int) map[x2, y2] == (int) SeaLandMapData.TYPE_LAND)
              ++num;
          }
        }
        if (num >= 5)
          twoBitArray2D[x1, y1] = SeaLandMapData.TYPE_LAND;
      }
    }
    return twoBitArray2D;
  }

  public void GenerateSeaLandHeight(
    int width,
    int height,
    TwoBitArray_2D seaLandData,
    SByteArray_2D heightMap)
  {
    for (int x = 0; x < width; ++x)
    {
      for (int y = 0; y < height; ++y)
        heightMap[x, y] = (int) seaLandData[x, y] != (int) SeaLandMapData.TYPE_LAND ? (sbyte) -10 : (sbyte) 5;
    }
  }
}
