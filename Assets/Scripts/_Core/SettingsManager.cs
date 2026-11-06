using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [Header("Controls")]
    public bool shotType = true;
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