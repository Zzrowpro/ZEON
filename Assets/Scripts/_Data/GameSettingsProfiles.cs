using UnityEngine;

public class GameSettingsProfiles : ScriptableObject
{
    [Header("ControlType")]

    [Range(0, 100)]
    public int Volume
    {
        set
        {
            Mathf.Clamp(value, 0, 100);
        }
    }
}