using FServ.Global;
using FServ.Server;
using FServ.Log;

namespace FServ;

public class Worker : BackgroundService
{
  private static string UpdateRate = Settings.FindProperty("update-rate");

  protected override async Task ExecuteAsync(CancellationToken stoppingToken)
  {
    Logger.Log("Starting FServ worker...");

    new Thread(() => HttpServer.Host("/")).Start();

    while (!stoppingToken.IsCancellationRequested)
    {
      Logger.Log("Updating root directory...");

      ServerFolder.Update();

      await Task.Delay(int.Parse(UpdateRate), stoppingToken);
    }
  }
}
