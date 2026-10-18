using UnityEngine;

public static class CustomDebug
{
    [System.Diagnostics.Conditional("UNITY_EDITOR")]
    public static void LogMessage(string LogMessage)
    {
        Debug.Log(LogMessage);
    }
}

