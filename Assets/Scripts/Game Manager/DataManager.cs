
using UnityEngine;

[System.Serializable]
public class GameData
{
    public float points;
}

public class DataManager : MonoBehaviour
{
    // The original variables
    public float points = 0;
    public static DataManager instance;

    // NEW: DEFAULT DATA STORAGE
    private GameData defaultData;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            // Save default when game starts
            SaveDefault();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //CREATE A COPY OF CURRENT DATA
    GameData GetCurrentData()
    {
        return new GameData
        {
            points = points
        };
    }

    //APPLY DATA BACK INTO MANAGER
    void ApplyData(GameData data)
    {
        points = data.points;
        Debug.Log("Data Reset Applied");

        // Optional: notify other systems
        OnDataReset?.Invoke(); //Could have use if we plan on continuing the project later on
    }

    // SAVE DEFAULT STATE
    public void SaveDefault()
    {
        defaultData = GetCurrentData();
        Debug.Log("Default Data Saved");
    }

    // RESET TO DEFAULT 
    public void ResetToDefault()
    {
        if (defaultData == null)
        {
            Debug.LogWarning("No default data saved!");
            return;
        }

        ApplyData(defaultData);
    }

    // OPTIONAL: RESET EVENT SYSTEM (was gonna be used but forgot)
    public static System.Action OnDataReset;
}