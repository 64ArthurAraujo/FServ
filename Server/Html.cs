using FServ.Global;
using System.Web;
using System.Text;
using Ganss.XSS;

namespace FServ.Server;

public class HtmlDocument
{
  private static string StyleURL = "https://rawcdn.githack.com/64ArthurAraujo/FServ/31e9b113d07119f9a5210a8c8b32f7522fe219b8/style.css";

  private static string StyleImport = $"<link rel=\"stylesheet\" href=\"{StyleURL}\">";

  private static string FolderImageURL = "https://rawcdn.githack.com/64ArthurAraujo/FServ/84235fa18afe283fd414fd5828fe82dfa4ab1d82/icons/folders.png";

  private static string RepositoryName = Settings.FindProperty("repo-name");

  private string Content =
    $"<html> <head>{StyleImport}</head> <body> <img src=\"{FolderImageURL}\"><h1>{RepositoryName}</h1>";

  public static void SetRepositoryName(string name)
  {
    name = HttpUtility.HtmlDecode(name);

    RepositoryName = name;
  }

  public void AddElement(string element, string innerContent, bool isLink = true)
  {
    string contentInTag = HttpUtility.HtmlDecode(innerContent);

    if (isLink)
      Content += $"<a href=\"/{contentInTag}\"><{element}>{contentInTag}</{element}></a>";
    else
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
