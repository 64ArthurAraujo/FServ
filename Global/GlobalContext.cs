using System.Runtime.InteropServices;
using FServ.Parser;

namespace FServ.Global;

public static class ServerSys
{
  public static bool IsRunningOnWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

  public static bool IsRunningOnLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

  public static bool IsRunningOnBSD = RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);

  public static bool IsRunningOnOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
}

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

  public static bool FileExistsInDirectory(string fileToFind, string[]? folderToSearch = null)
  {
    string filename = fileToFind.Substring(1);

    foreach (string file in folderToSearch ?? RootDirectoryFiles)
    {
      if (file == ReadPath + filename) return true;
    }

    return false;
  }

  public static bool FolderExistsInDirectory(string folderToFind, string[]? folderToSearch = null)
  {
    string filename = folderToFind.Substring(1);

    foreach (string file in folderToSearch ?? RootDirectoryFolders)
    {
      if (file == ReadPath + filename) return true;
    }

    return false;
  }

  public static string[] GetDirectoryFiles(string directoryPath)
  {
    string folderPath = ReadPath + directoryPath.Substring(1);

    return Directory.GetFiles(folderPath);
  }

  public static void Update()
  {
    RootDirectoryFiles = Directory.GetFiles(ReadPath);
    RootDirectoryFolders = Directory.GetDirectories(ReadPath);
  }
}
