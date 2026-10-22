using UnityEngine;
using UnityEngine.InputSystem;

public class PauseSystem : MonoBehaviour
{
    public InputActionAsset InputActions;
    public GameObject pauseMenu = null;

    private InputAction pause;
    private bool isPaused;

    void OnEnable()
    {
        InputActions.FindActionMap("UI").Enable();
    }

    void OnDisable()
    {
        InputActions.FindActionMap("UI").Disable();
    }

    void Awake()
    {
        var map = InputActions.FindActionMap("UI", true);
        pause = map.FindAction("pause", true);
    }

    void Update()
    {
        
        if (pause.WasPressedThisFrame())
        {
            Pause();
        }
    }

    public void Pause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pauseMenu.SetActive(isPaused);
    }

    public bool GetIsPaused()
    {
        return isPaused;
    }
}