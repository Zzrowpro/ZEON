using UnityEngine;

public abstract class Objective : ScriptableObject
{
    public string description;   // "Kill Pyros"
    public int amount = 5;
    public abstract bool IsComplete(ObjectiveProgress p);
    public virtual bool Matches(string id) => false;
}