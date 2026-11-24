using System;

public static class GameEvents
{
    public static event Action<string> OnReport;
    public static void Report(string id) => OnReport?.Invoke(id);
    public static event System.Action ObjectivesCompleted;
    public static void RaiseObjectivesCompleted() => ObjectivesCompleted?.Invoke();
}