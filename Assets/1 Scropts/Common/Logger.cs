using System.Diagnostics;

/// <summary>
/// 커스텀 로거 <br/>
/// Default, Warning, Error 3단계로 나눈 간단한 로거 <br/>
/// Conditional로 출시용 빌드시 로그 제거(Error 제외)
/// </summary>
public static class Logger
{
    [Conditional("Dev")]
    public static void Log(string message)
    {
        UnityEngine.Debug.Log($"[{TimeString}] {message}]");
    }

    [Conditional("Dev")]
    public static void LogWarning(string message)
    {
        UnityEngine.Debug.LogWarning($"[{TimeString}] {message}]");
    }

    public static void LogError(string message)
    {
        UnityEngine.Debug.LogError($"[{TimeString}] {message}]");
    }

    private static string TimeString => $"{System.DateTime.Now:MM/dd/yyyy hh:mm:ss.fff}";
}
