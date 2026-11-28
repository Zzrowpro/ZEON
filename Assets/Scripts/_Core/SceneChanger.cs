using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void ChangeScene(string sceneName)
    {
        ResetRun();
        SceneManager.LoadScene(sceneName);
    }

    public void RestartScene()
    {
        ResetRun();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ResetRun()
    {
        Time.timeScale = 1f;
        DataManager.instance.points = 0;
        DataManager.instance.isGameOver = false;  
    }

    public void Quit()
        {
            Application.Quit();

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
        }
}