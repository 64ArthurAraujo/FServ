using FServ.Parser;

namespace FServ.Global;

public static class Settings
{
  private static List<KeyValuePair<string, string>> SettingsFile = FServFileParser.Parse();

  public static KeyValuePair<string, string> FindProperty(string Property)
  {
    foreach (var property in SettingsFile)
    {
      if (property.Key == Property) return SettingsFile[SettingsFile.IndexOf(property)];
    }

    throw new KeyNotFoundException($"Property named '{Property}' was not found!");
  }
}
