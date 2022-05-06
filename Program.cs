using FServ.Global;
using FServ.Log;
using FServ;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
      if (ServerSys.IsRunningOnOSX)
      {
        Logger.Issue("Unsupported Platform");
      }
      else
      {
        services.AddHostedService<Worker>();
      }
    })
    .Build();

await host.RunAsync();
