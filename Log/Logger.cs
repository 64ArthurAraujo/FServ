namespace FServ.Log;

public static class Logger
{
  public static void Log(string contentToLog)
  {
    string currentTime = DateTime.Now.ToString("HH:mm:ss");

    Console.WriteLine($"[{LogType.INFO}]   ({currentTime})      {contentToLog}");
  }

  public static void Warn(string contentToLog)
  {
    string currentTime = DateTime.Now.ToString("HH:mm:ss");

    Console.WriteLine($"[{LogType.WARNING}] ({currentTime})     {contentToLog}");
  }

  public static void Issue(string contentToLog, Exception err)
  {
    string currentTime = DateTime.Now.ToString("HH:mm:ss");

    Console.WriteLine($"[{LogType.ISSUE}]  ({currentTime})      {contentToLog} \n\n {err} \n\n");
  }

  public enum LogType
  {
    INFO,
    WARNING,
    ISSUE,
  }
}
