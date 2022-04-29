using System.Net;
using System.Web;
using System.Text;
using Ganss.XSS;
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

      response.ContentLength64 = buffer.Length;

      Stream output = response.OutputStream;

      output.Write(buffer, 0, buffer.Length);
      output.Close();
    }

    Listener.Stop();

    HttpServer.Host(URI);
  }
}

public class HtmlDocument
{
  private string Content = "<html> <head></head> <body>";

  public void AddElement(string element, string innerContent)
  {
    string contentInsideTag = HttpUtility.HtmlDecode(innerContent);

    Content += $"<{element}>{contentInsideTag}</{element}>";
  }

  public byte[] GetBuffer()
  {
    Content += $"</body></html>";

    string sanitizedContent = new HtmlSanitizer().Sanitize(Content);

    byte[] buffer = Encoding.UTF8.GetBytes(sanitizedContent);

    return buffer;
  }
}
