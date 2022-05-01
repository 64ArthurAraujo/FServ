using System.Web;
using System.Text;
using Ganss.XSS;

namespace FServ.Server;

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
