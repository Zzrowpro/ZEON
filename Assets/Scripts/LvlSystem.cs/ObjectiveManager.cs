using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public LevelData level;
    public GameObject levelPanel;

    bool done;
    public bool Done => done;   // others can read it, only this class can set it

    readonly Dictionary<Objective, ObjectiveProgress> progress = new();

    void OnEnable()  => GameEvents.OnReport += HandleReport;
    void OnDisable() => GameEvents.OnReport -= HandleReport;

    void Start()
    {
        foreach (var o in level.objectives)
            progress[o] = new ObjectiveProgress();
    }

    void HandleReport(string id)
    {
        if (done || progress.Count == 0) return;

        foreach (var kv in progress)
            if (kv.Key.Matches(id)) kv.Value.count++;

        if (progress.All(kv => kv.Key.IsComplete(kv.Value)))
        {
            done = true;
            GameEvents.RaiseObjectivesCompleted();
        }
    }
}