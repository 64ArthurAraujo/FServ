using FServ.Global;
using FServ.Server;

namespace FServ;

public class Worker : BackgroundService
{
  private static string UpdateRate = Settings.FindProperty("update-rate");

  private readonly ILogger<Worker> Logger;

  public Worker(ILogger<Worker> logger)
  {
    Logger = logger;
  }

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    Logger.LogInformation("FServ up and running...");

    new Thread(() => HttpServer.Host("/")).Start();

    while (!stoppingToken.IsCancellationRequested)
    {
      Logger.LogInformation("Updating directory...");

      ServerFolder.Update();

      await Task.Delay(int.Parse(UpdateRate), stoppingToken);
    }
  }
}
