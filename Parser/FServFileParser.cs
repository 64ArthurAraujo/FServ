using FServ.Log;
using FServ.Global;

namespace FServ.Parser;

public static class FServFileParser
{
  private static string FolderPath =
    ServerSys.IsRunningOnWindows ? @"C:\AppData\fserv\" : "/etc/fserv/";

  private static string[] FServFolder = Directory.GetFiles(FolderPath);

  private static List<KeyValuePair<string, string>> Settings = new List<KeyValuePair<string, string>>();

  private static string FServFile = FolderPath + ".fserv";

  public static List<KeyValuePair<string, string>> Parse()
  {
    try
    {
      if (!Directory.Exists(FolderPath))
      {
        Directory.CreateDirectory(FolderPath);
        CreateFServFile();
      }
      else
      {
        string? dotFServFile = Array.Find<string>(FServFolder, file => file.Equals(FServFile));

        if (dotFServFile == null)
          CreateFServFile();
      }

      ParseFServFile();
    }
    catch (IndexOutOfRangeException e)
    {
      Logger.Issue($"Parsing of the .fserv file failed! This is probably is probably due to blank lines in the file, please remove those lines and try again.", e);
    }
    catch (Exception e)
    {
      // Probably a user privilege exception.
      Logger.Issue($"Failed to parse .fserv file!", e);
    }

    return Settings;
  }

  private static void ParseFServFile()
  {
    Logger.Log("Parsing .fserv file...");

    using (StreamReader sr = File.OpenText(FServFile))
    {
      string? line = String.Empty;

      while ((line = sr.ReadLine()) != null)
      {
        if (line.StartsWith("#")) continue;

        KeyValuePair<string, string> ParsedLine =
          new KeyValuePair<string, string>(line.Split("=")[0], line.Split("=")[1]);

        Settings.Add(ParsedLine);
      }
    }
  }

  public static void CreateFServFile()
  {
    Logger.Log("A .fserv file was not found, creating a new one...");

    using (StreamWriter file = File.CreateText(FServFile))
    {
      file.WriteLine("# Path to read the files from");
      if (ServerSys.IsRunningOnWindows)
      {
        file.WriteLine(@"path=C:\AppData\fserv\files");
      }
      else if (ServerSys.IsRunningOnLinux || ServerSys.IsRunningOnBSD)
      {
        file.WriteLine(@"path=/etc/fserv/repo-files/");
      }

      file.WriteLine("# Speed in ms which the FServ will fetch new files");
      file.WriteLine(@"update-rate=1000");

      file.WriteLine("# The name of the file repository");
      file.WriteLine(@"repo-name=FServ File Repository");
    }
  }
}
