using UnityEngine;

[CreateAssetMenu(menuName = "Objectives/Points")]
public class PointsObjective : Objective
{
    public const string PointsId = "points";

    public override bool Matches(string id) => id == PointsId;
    public override bool IsComplete(ObjectiveProgress p) => p.count >= amount;
}