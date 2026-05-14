using System;

#nullable disable
namespace YetAnotherMapManager.Debug;

internal class Recorder
{
  private DateTime startTime;
  private DateTime stopTime;
  private int count;
  private int totalElapsed;
  private bool running;

  public void Start()
  {
    this.startTime = DateTime.Now;
    this.running = true;
  }

  public void Stop()
  {
    this.stopTime = DateTime.Now;
    ++this.count;
    this.totalElapsed += (int) (this.stopTime - this.startTime).TotalMilliseconds;
    this.running = false;
  }

  public int GetCount() => this.count;

  public int GetTotalTime() => this.totalElapsed;

  public double GetElapsedTime()
  {
    return (!this.running ? this.stopTime - this.startTime : DateTime.Now - this.startTime).TotalMilliseconds;
  }

  public double GetElapsedTimeSecs()
  {
    return (!this.running ? this.stopTime - this.startTime : DateTime.Now - this.startTime).TotalSeconds;
  }
}
