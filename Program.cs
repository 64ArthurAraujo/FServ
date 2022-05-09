using FServ.Global;
using FServ.Log;
using FServ;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
      if (ServerSys.IsRunningOnOSX)
      {
        Logger.Issue("Unsupported Platform", new PlatformNotSupportedException());
      }
      else
      {
        services.AddHostedService<Worker>();
      }
    })
    .Build();

await host.RunAsync();
