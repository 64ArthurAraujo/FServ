using System.Runtime.InteropServices;

namespace FServ.Parser;

public static class FServFileParser
{
  private static bool IsRunningOnWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

  private static string FolderPath = IsRunningOnWindows ? @"C:\AppData\fserv\" : "/home/arthur/Projects/FServ/";

  private static string[] FServFolder = Directory.GetFiles(FolderPath);

  private static List<KeyValuePair<string, string>> Settings = new List<KeyValuePair<string, string>>();

  private static string FServFile = FolderPath + ".fserv";

  public static List<KeyValuePair<string, string>> Parse()
  {
    bool FServFolderExists = Directory.Exists(FolderPath);

    try
    {
      if (!FServFolderExists)
        Directory.CreateDirectory(FolderPath);

      string? dotFServFile = Array.Find<string>(FServFolder, file => file.Equals(FServFile));

      if (dotFServFile == null)
        CreateFServFile();

      ParseFServFile();
    }
    catch (Exception e)
    {
      // lacks admin privileges
      Console.WriteLine(e);
    }

    return Settings;
  }

  private static void ParseFServFile()
  {
    using (StreamReader sr = File.OpenText(FServFile))
    {
      string? line = String.Empty;

      while ((line = sr.ReadLine()) != "!!!end!!!")
      {
        if (line == null || line.StartsWith("#")) continue;

        KeyValuePair<string, string> ParsedLine =
          new KeyValuePair<string, string>(line.Split("=")[0], line.Split("=")[1]);

        Settings.Add(ParsedLine);
      }
    }
  }

  public static void CreateFServFile()
  {
    using (StreamWriter sw = File.CreateText(FServFile))
    {
      if (IsRunningOnWindows)
        sw.WriteLine(@"path=C:\AppData\fserv\files");
      else
        sw.WriteLine(@"path=/home/");
    }
  }
}
