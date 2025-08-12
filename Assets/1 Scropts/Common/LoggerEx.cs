using System.Diagnostics;

/// <summary>
/// 커스텀 로거 <br/>
/// Default, Warning, Error 3단계로 나눈 간단한 로거 <br/>
/// Conditional로 출시용 빌드시 로그 제거(Error 제외)
/// </summary>
public static class LoggerEx
{
    [Conditional("Dev")]
    public static void Log(string message)
    {
        UnityEngine.Debug.Log($"[{TimeString}] {message}");
    }

    [Conditional("Dev")]
    public static void LogWarning(string message)
    {
        UnityEngine.Debug.LogWarning($"[{TimeString}] {message}");
    }

    public static void LogError(string message)
    {
        UnityEngine.Debug.LogError($"[{TimeString}] {message}");
    }

    private static string _timeString;
    private static System.DateTime _now;
    private static readonly System.TimeSpan UpdateInterval = System.TimeSpan.FromMilliseconds(100);

    private static string TimeString
    {
        get
        {
            var now = System.DateTime.Now;
            if (_timeString == null || now - _now > UpdateInterval)
            {
                _now = now;
                _timeString = _now.ToString("MM/dd/yyyy hh:mm:ss.fff");
            }
            return _timeString;
        }
    }
}
