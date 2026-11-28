using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/Collect")]
public class CollectObjective : Objective
{
    public string itemId;    // empty = any item counts

    public override bool Matches(string id) =>
        string.IsNullOrEmpty(itemId) || itemId == id;

    public override bool IsComplete(ObjectiveProgress p) => p.count >= amount;
}