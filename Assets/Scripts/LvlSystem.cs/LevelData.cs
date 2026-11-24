using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Levels/Level")]
public class LevelData : ScriptableObject
{
    public string levelName;
    public List<Objective> objectives;
}