using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        if(Time.timeScale == 0) Time.timeScale = 1;
        SceneManager.LoadScene(sceneName);
    }
    
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }


    public void Quit()
    {
        Application.Quit();

        // Stops Play Mode when testing inside the Unity Editor
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}