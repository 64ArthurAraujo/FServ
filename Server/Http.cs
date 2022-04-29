using System.Net;
using FServ.Global;

namespace FServ.Server;

public static class HttpServer
{
  private static HttpListener Listener = new HttpListener();




  public static void Host(string URI)
  {
    if (!HttpListener.IsSupported)
    {
      Console.WriteLine("HttpListener is not supported on this host!");
      return;
    }

    Listener.Prefixes.Add("http://*:8080/");
    Listener.Start();

    HttpListenerContext context = Listener.GetContext();

    HttpListenerRequest request = context.Request;

    if (context.Request.RawUrl == "/")
    {
      HttpListenerResponse response = context.Response;

      string responseString = $"<html><body>{ServerContext.ReadDirectory.First().Split("/").Last()}</body></html>";

      byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);

      response.ContentLength64 = buffer.Length;

      Stream output = response.OutputStream;

      output.Write(buffer, 0, buffer.Length);
      output.Close();
    }

    Listener.Stop();

    HttpServer.Host(URI);
  }
}
