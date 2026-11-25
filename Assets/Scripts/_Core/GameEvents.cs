

public static class GameEvents
{
    public static event System.Action<string, int> OnReport;
    public static void Report(string id, int amount = 1) => OnReport?.Invoke(id, amount);
    public static event System.Action ObjectivesCompleted;
    public static void RaiseObjectivesCompleted() => ObjectivesCompleted?.Invoke();
    public static event System.Action ProgressChanged;
    public static void RaiseProgressChanged() => ProgressChanged?.Invoke();
}   