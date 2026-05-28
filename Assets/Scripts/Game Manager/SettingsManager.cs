using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public bool movementMode = true;
    public static SettingsManager instance;

     void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}