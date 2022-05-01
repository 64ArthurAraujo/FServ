#pragma warning disable CS8604

using System.Net;
using FServ.Global;

namespace FServ.Server;

public static class HttpServer
{
  private static HttpListener Listener = new HttpListener();

  public static void Host(string URI)
  {
    Listener.Prefixes.Add("http://*:8080/");
    Listener.Start();

    HttpListenerContext context = Listener.GetContext();
    HttpListenerResponse response = context.Response;

    if (context.Request.RawUrl == "/")
    {
      HtmlDocument html = new HtmlDocument();

      foreach (string file in ServerContext.ReadDirectory)
        html.AddElement("h4", file.Split("/").Last());

      byte[] buffer = html.GetBuffer();

      Http.Respond(response, buffer);
    }
    else if (ServerContext.ExistsInDirectory(context.Request.RawUrl))
    {
      Console.WriteLine("tru");
      // todo automatically remove the last / in the .fserv file
      byte[] requestedFileBuffer =
        File.ReadAllBytes(ServerContext.ReadPath + context.Request.RawUrl?.Replace("/", ""));

      Http.RespondWithFile(response, requestedFileBuffer);
    }

    Listener.Stop();

    HttpServer.Host(URI);
  }
}

public static class Http
{
  public static void Respond(HttpListenerResponse response, byte[] buffer)
  {
    response.ContentLength64 = buffer.Length;

    Stream output = response.OutputStream;

    output.Write(buffer, 0, buffer.Length);

    output.Close();
  }

  public static void RespondWithFile(HttpListenerResponse response, byte[] buffer)
  {
    response.AddHeader("Content-Type", "application/octet-stream");

    Http.Respond(response, buffer);
  }
}
