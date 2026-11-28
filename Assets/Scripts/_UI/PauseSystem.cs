using UnityEngine;
using UnityEngine.InputSystem;

public class PauseSystem : MonoBehaviour
{
    public InputActionAsset InputActions;
    [SerializeField] private GameObject _mainMenuCanvasGO;
    [SerializeField] private GameObject _settingsMenuCanvasGO;

    private InputAction pause;
    private bool isPaused;

    void Start()
    {
        _mainMenuCanvasGO.SetActive(false);
        _settingsMenuCanvasGO.SetActive(false);
    }

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
            if(isPaused == true)
            {
                Pause();
            }
            if(isPaused == false)
            {
                UnPause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        OpenPauseMenu();
        
    }

    public void UnPause()
    {
        isPaused = false;
        Time.timeScale = 1;
        CloseAllMenus();
    }

    private void OpenPauseMenu()
    {
        _mainMenuCanvasGO.SetActive(true);
        _settingsMenuCanvasGO.SetActive(false);
    }

    private void CloseAllMenus()
    {
        _mainMenuCanvasGO.SetActive(false);
        _settingsMenuCanvasGO.SetActive(false);
    }
    public bool GetIsPaused()
    {
        return isPaused;
    }
}