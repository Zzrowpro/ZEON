using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/Kill")]
public class KillObjective : Objective
{
    public string enemyId;   // empty = any enemy counts
    public int amount = 5;

    public override bool Matches(string id) =>
        string.IsNullOrEmpty(enemyId) || enemyId == id;

    public override bool IsComplete(ObjectiveProgress p) => p.count >= amount;
}