using System.Web;
using System.Text;
using Ganss.XSS;

namespace FServ.Server;

public class HtmlDocument
{
  private static string StyleURL = "https://raw.githack.com/64ArthurAraujo/FServ/main/style.css";

  private static string StyleImport = $"<link rel=\"stylesheet\" href=\"{StyleURL}\">";

  private string Content = $"<html> <head>{StyleImport}</head> <body>";

  public void AddElement(string element, string innerContent)
  {
    string contentInTag = HttpUtility.HtmlDecode(innerContent);

    Content += $"<{element}>{contentInTag}</{element}>";
  }

  public byte[] GetBuffer()
  {
    Content += $"</body></html>";

    HtmlSanitizer sanitizer = new HtmlSanitizer();
    sanitizer.AllowedTags.Add("link");

    string sanitizedContent = sanitizer.Sanitize(Content);

    byte[] buffer = Encoding.UTF8.GetBytes(sanitizedContent);

    return buffer;
  }
}
