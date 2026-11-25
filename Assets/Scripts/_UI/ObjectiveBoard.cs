using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveBoard : MonoBehaviour
{
    [SerializeField] ObjectiveManager manager;
    [SerializeField] TMP_Text rowPrefab;
    [SerializeField] Transform container;   // has a Vertical Layout Group
    [SerializeField] string bullet = "- ";

    readonly Dictionary<Objective, TMP_Text> rows = new();

    void OnEnable()  => GameEvents.ProgressChanged += Refresh;
    void OnDisable() => GameEvents.ProgressChanged -= Refresh;

    void Start()
    {
        foreach (var o in manager.level.objectives)
            rows[o] = Instantiate(rowPrefab, container);

        Refresh();   // show 0/5 straight away
    }

    void Refresh()
    {
        foreach (var kv in rows)
        {
            Objective o = kv.Key;
            int shown = Mathf.Min(manager.GetCount(o), o.amount);
            string line = $"{bullet}{o.description} {shown}/{o.amount}";

            kv.Value.text = manager.IsObjectiveDone(o)
                ? $"<color=#6f6><s>{line}</s></color>"
                : line;
        }
    }
}