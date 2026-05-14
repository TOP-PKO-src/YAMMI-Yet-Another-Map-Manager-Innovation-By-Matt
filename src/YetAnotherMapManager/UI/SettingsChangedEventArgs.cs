using System;

#nullable disable
namespace YetAnotherMapManager.UI;

public class SettingsChangedEventArgs : EventArgs
{
  public string Name;
  public bool Displayed;
  public float Alpha;
}
