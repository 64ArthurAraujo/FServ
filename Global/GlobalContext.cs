using FServ.Parser;

namespace FServ.Global;

public static class Settings
{
  private static List<KeyValuePair<string, string>> SettingsFile = FServFileParser.Parse();

  public static string FindProperty(string Property)
  {
    foreach (var property in SettingsFile)
    {
      if (property.Key == Property) return SettingsFile[SettingsFile.IndexOf(property)].Value;
    }

    throw new KeyNotFoundException($"Property named '{Property}' was not found!");
  }
}

public static class ServerFolder
{
  public static string ReadPath = Settings.FindProperty("path");

  public static string[] RootDirectoryFiles = Directory.GetFiles(ReadPath);

  public static string[] RootDirectoryFolders = Directory.GetDirectories(ReadPath);

  public static bool ExistsInDirectory(string fileToFind)
  {
    string filename = fileToFind.Replace("/", "");

    foreach (string file in RootDirectoryFiles)
      if (file == ReadPath + filename) return true;

    return false;
  }

  public static void Update()
  {
    RootDirectoryFiles = Directory.GetFiles(ReadPath);
    RootDirectoryFolders = Directory.GetDirectories(ReadPath);
  }
}
