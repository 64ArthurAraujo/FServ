using FServ.Global;

namespace FServ;

public class Worker : BackgroundService
{
  private static string ReadPath = Settings.FindProperty("path");

  private static string UpdateRate = Settings.FindProperty("update-rate");

  private string[] ReadDirectory = Directory.GetFiles(ReadPath);

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
      ReadDirectory = Directory.GetFiles(ReadPath);

      await Task.Delay(int.Parse(UpdateRate), stoppingToken);
    }
  }
}
