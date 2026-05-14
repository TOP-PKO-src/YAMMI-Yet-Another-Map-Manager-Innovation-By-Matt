using System;
using System.Collections.Generic;

#nullable disable
namespace YetAnotherMapManager.Debug;

public class PerfTimer
{
  private bool muted = true;
  private static PerfTimer instance;
  private Dictionary<string, Recorder> recorderList;

  private PerfTimer()
  {
    this.recorderList = new Dictionary<string, Recorder>();
    PerfTimer.instance = this;
  }

  public static PerfTimer Instance
  {
    get
    {
      if (PerfTimer.instance == null)
        PerfTimer.instance = new PerfTimer();
      return PerfTimer.instance;
    }
  }

  public void Start(string name)
  {
    Recorder recorder;
    if (this.recorderList.ContainsKey(name))
    {
      recorder = this.recorderList[name];
    }
    else
    {
      recorder = new Recorder();
      this.recorderList.Add(name, recorder);
    }
    recorder.Start();
  }

  public void Stop(string name)
  {
    if (!this.recorderList.ContainsKey(name))
      throw new Exception($"Timer named {{{name}}} does not exist");
    this.recorderList[name].Stop();
    if (this.muted)
      return;
    Console.WriteLine($"Timer> {{{name}}} lasted {(object) this.recorderList[name].GetElapsedTimeSecs()}s. [{(object) this.recorderList[name].GetCount()}][{(object) this.recorderList[name].GetTotalTime()}]");
  }

  public void PrintStats()
  {
    Console.WriteLine("-----------------------------------------------------");
    foreach (string key in this.recorderList.Keys)
      Console.WriteLine($"Timer> {{{key}}} lasted {(object) this.recorderList[key].GetElapsedTimeSecs()}s. [{(object) this.recorderList[key].GetCount()}][{(object) this.recorderList[key].GetTotalTime()}]");
    Console.WriteLine("-----------------------------------------------------");
  }
}
