using FServ.Global;

namespace FServ;

public class Worker : BackgroundService
{
  private KeyValuePair<string, string> ReadPath = Settings.FindProperty("path");

  private KeyValuePair<string, string> UpdateRate = Settings.FindProperty("update-rate");

  private readonly ILogger<Worker> Logger;

  public Worker(ILogger<Worker> logger)
  {
    Logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    Logger.LogInformation("FServ up and running...");

    while (!stoppingToken.IsCancellationRequested)
    {
      Logger.LogInformation("Updating directory...");

      await Task.Delay(int.Parse(UpdateRate.Value), stoppingToken);
    }
  }
}
