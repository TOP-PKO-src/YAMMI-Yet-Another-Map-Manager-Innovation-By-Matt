#nullable disable
namespace YetAnotherMapManager.MapData.PkoNative;

internal class Tile
{
  public byte btIsland;
  public byte btTileInfo;
  public sbyte cHeight;
  public uint dwTileInfo;
  public ushort sColor;
  public sbyte[,] squares;
  public short sRegion;

  public bool isBridge() => ((int) this.sRegion & 8) == 8;

  public bool isLand() => ((int) this.sRegion & 1) == 1;

  public bool isMiningZone() => ((int) this.sRegion & 32 /*0x20*/) == 32 /*0x20*/;

  public bool isNoMonsterZone() => ((int) this.sRegion & 16 /*0x10*/) == 16 /*0x10*/;

  public bool isNonPk() => ((int) this.sRegion & 4) == 4;

  public bool isPvPInviteZone() => ((int) this.sRegion & 64 /*0x40*/) == 64 /*0x40*/;

  public bool isSafeZone() => ((int) this.sRegion & 2) == 2;

  public void setLand(bool isLand)
  {
    if (isLand)
      this.sRegion |= (short) 1;
    else
      this.sRegion &= (short) 254;
  }

  public void setSafeZone(bool isSafe)
  {
    if (isSafe)
      this.sRegion |= (short) 2;
    else
      this.sRegion &= (short) 253;
  }

  public void setPvPInvite(bool isPvPInvite)
  {
    if (isPvPInvite)
      this.sRegion |= (short) 64 /*0x40*/;
    else
      this.sRegion &= (short) 191;
  }

  public void setNoMonster(bool isNoMonster)
  {
    if (isNoMonster)
      this.sRegion |= (short) 16 /*0x10*/;
    else
      this.sRegion &= (short) 239;
  }

  public void setNonPk(bool isNonPk)
  {
    if (isNonPk)
      this.sRegion |= (short) 4;
    else
      this.sRegion &= (short) 251;
  }
}
