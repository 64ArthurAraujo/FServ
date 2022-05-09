#pragma warning disable CS8604

using System.Net;
using FServ.Global;

namespace FServ.Server;

public static class HttpServer
{
  private static HttpListener Listener = new HttpListener();

  public static void Host(string name)
  {
    Listener.Prefixes.Add("http://*:8080/");
    Listener.Start();

    HttpListenerContext context = Listener.GetContext();
    HttpListenerResponse response = context.Response;

    if (context.Request.RawUrl == "/")
    {
      RenderDirectory(response);
    }
    else if (ServerFolder.FolderExistsInDirectory(context.Request.RawUrl))
    {
      string dirPath = ServerFolder.ReadPath + context.Request.RawUrl.Substring(1);

      RenderDirectory(response, dirPath);
    }
    else
    {
      // todo automatically remove the last / in the .fserv file
      string requestFilePath = ServerFolder.ReadPath + context.Request.RawUrl?.Substring(1);

      DownloadFile(requestFilePath, response);
    }

    Listener.Stop();

    HttpServer.Host(name);
  }

  private static void DownloadFile(string requestFilePath, HttpListenerResponse response)
  {
    if (File.Exists(requestFilePath))
      SendFile(response, requestFilePath);
    else
      SendPageNotFound(response);
  }

  private static void SendFile(HttpListenerResponse response, string requestFilePath)
  {
    byte[] requestedFileBuffer = File.ReadAllBytes(requestFilePath);

    Http.RespondWithFile(response, requestedFileBuffer);
  }

  private static void SendPageNotFound(HttpListenerResponse response)
  {
    HtmlDocument html404 = new HtmlDocument();

    html404.AddElement("h2 align=\"center\"", "Error 404", false);
    html404.AddElement("h4 align=\"center\"", "This File Moved or never existed!", false);

    byte[] buffer404page = html404.GetBuffer();

    Http.Respond(response, buffer404page, 404);
  }

  private static void RenderDirectory(HttpListenerResponse response, string? dirPath = null)
  {
    string GetFilename(string filePath) { return filePath.Split("/").Last(); }

    dirPath = dirPath ?? ServerFolder.ReadPath;

    HtmlDocument html = new HtmlDocument();

    foreach (string folder in Directory.GetDirectories(dirPath))
      html.AddElement("h3", GetFilename(folder));

    foreach (string file in Directory.GetFiles(dirPath))
      html.AddElement("h4", GetFilename(file));

    Http.Respond(response, html.GetBuffer());
  }
}

public static class Http
{
  public static void Respond(HttpListenerResponse response, byte[] buffer, int statusCode = 200)
  {
    response.ContentLength64 = buffer.Length;

    response.StatusCode = statusCode;

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
