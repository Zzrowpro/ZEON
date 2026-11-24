using UnityEngine;

public abstract class Objective : ScriptableObject
{
    public abstract bool IsComplete(ObjectiveProgress p);
    public virtual bool Matches(string id) => false;
}   
