using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using YetAnotherMapManager.Plugins;

#nullable disable
namespace YetAnotherMapManager.MapData;

[Serializable]
public class MapInfo
{
  public static int DATA_FORMAT_VERSION = 1;
  private int width;
  private int height;
  private Dictionary<string, IMapData> mapData;
  [OptionalField]
  private int nativeSectionWidth;
  [OptionalField]
  private int nativeSectionHeight;
  [OptionalField]
  private bool[] nativeSectionPresence;
  [OptionalField]
  private int[] nativeSectionOffsets;
  [OptionalField]
  private long nativeFileSize;

  public int Width => this.width;

  public int Height => this.height;

  public int NativeSectionWidth => this.nativeSectionWidth;

  public int NativeSectionHeight => this.nativeSectionHeight;

  public long NativeFileSize => this.nativeFileSize;

  public void AddCustomData(string key, IMapData data) => this.mapData.Add(key, data);

  public IMapData GetCustomData(string key)
  {
    return this.mapData.Keys.Contains<string>(key) ? this.mapData[key] : (IMapData) null;
  }

  public string[] CustomDataKeys => this.mapData.Keys.ToArray<string>();

  public void SetNativeSectionLayout(int sectionWidth, int sectionHeight, bool[] sectionPresence)
  {
    this.nativeSectionWidth = sectionWidth;
    this.nativeSectionHeight = sectionHeight;
    this.nativeSectionPresence = sectionPresence == null ? (bool[]) null : (bool[]) sectionPresence.Clone();
  }

  public void SetNativeSectionLayout(
    int sectionWidth,
    int sectionHeight,
    int[] sectionOffsets,
    long nativeFileSize)
  {
    this.nativeSectionWidth = sectionWidth;
    this.nativeSectionHeight = sectionHeight;
    this.nativeSectionOffsets = sectionOffsets == null ? (int[]) null : (int[]) sectionOffsets.Clone();
    this.nativeSectionPresence = sectionOffsets == null ? (bool[]) null : Array.ConvertAll<int, bool>(sectionOffsets, (Converter<int, bool>) (offset => offset != 0));
    this.nativeFileSize = nativeFileSize;
  }

  public bool TryGetNativeSectionPresence(
    int sectionWidth,
    int sectionHeight,
    int sectionCount,
    out bool[] sectionPresence)
  {
    if (this.nativeSectionPresence != null && this.nativeSectionWidth == sectionWidth && this.nativeSectionHeight == sectionHeight && this.nativeSectionPresence.Length == sectionCount)
    {
      sectionPresence = (bool[]) this.nativeSectionPresence.Clone();
      return true;
    }
    sectionPresence = (bool[]) null;
    return false;
  }

  public bool TryGetNativeSectionOffsets(
    int sectionWidth,
    int sectionHeight,
    int sectionCount,
    out int[] sectionOffsets)
  {
    if (this.nativeSectionOffsets != null && this.nativeSectionWidth == sectionWidth && this.nativeSectionHeight == sectionHeight && this.nativeSectionOffsets.Length == sectionCount)
    {
      sectionOffsets = (int[]) this.nativeSectionOffsets.Clone();
      return true;
    }
    sectionOffsets = (int[]) null;
    return false;
  }

  public MapInfo(int width, int height)
  {
    this.width = width;
    this.height = height;
    this.mapData = new Dictionary<string, IMapData>(5);
  }
}
