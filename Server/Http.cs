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
      RespondWithRootDirectory(response);
    }
    else if (ServerFolder.ExistsInDirectory(context.Request.RawUrl))
    {
      RespondWithFile(context, response);
    }

    Listener.Stop();

    HttpServer.Host(URI);
  }

  private static void RespondWithFile(HttpListenerContext context, HttpListenerResponse response)
  {
    // todo automatically remove the last / in the .fserv file
    byte[] requestedFileBuffer =
      File.ReadAllBytes(ServerFolder.ReadPath + context.Request.RawUrl?.Replace("/", ""));

    Http.RespondWithFile(response, requestedFileBuffer);
  }

  private static void RespondWithRootDirectory(HttpListenerResponse response)
  {
    string GetFilename(string filePath) { return filePath.Split("/").Last(); }


    HtmlDocument html = new HtmlDocument();

    foreach (string folder in ServerFolder.RootDirectoryFolders)
      html.AddElement("h3", GetFilename(folder));

    foreach (string file in ServerFolder.RootDirectoryFiles)
      html.AddElement("h4", GetFilename(file));

    Http.Respond(response, html.GetBuffer());
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
