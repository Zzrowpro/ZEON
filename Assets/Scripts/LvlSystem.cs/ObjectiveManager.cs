using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public LevelData level;
    public GameObject levelPanel;

    bool done;
    public bool Done => done;

    readonly Dictionary<Objective, ObjectiveProgress> progress = new();

    void OnEnable()  => GameEvents.OnReport += HandleReport;
    void OnDisable() => GameEvents.OnReport -= HandleReport;


    void Start()
    {
        foreach (var o in level.objectives)
            progress[o] = new ObjectiveProgress();
    }

    public int GetCount(Objective o) => progress.TryGetValue(o, out var p) ? p.count : 0;

    public bool IsObjectiveDone(Objective o) => progress.TryGetValue(o, out var p) && o.IsComplete(p);

    void HandleReport(string id, int amount)
    {
        if (done || progress.Count == 0) return;

        bool changed = false;
        foreach (var kv in progress)
        {
            if (!kv.Key.Matches(id)) continue;
            kv.Value.count += amount;
            changed = true;
        }

        if (changed) GameEvents.RaiseProgressChanged();

        if (progress.All(kv => kv.Key.IsComplete(kv.Value)))
        {
            done = true;
            GameEvents.RaiseObjectivesCompleted();
        }
    }
}