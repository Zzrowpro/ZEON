
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public bool IsGameOver { get; private set; }

    void Awake() => Instance = this;

    public void TriggerGameOver()
    {
        if (IsGameOver) return;   // guard: only fire once
        IsGameOver = true;
        Time.timeScale = 0f;      // freeze, then show your game over UI
    }
}