using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public InputActionAsset InputActions;
    [Header("Menu Objects")]
    [SerializeField] private GameObject _mainMenuCanvasGO;
    [SerializeField] private GameObject _settingsMenuCanvasGO;

    [Header("First Selected Options")]
    [SerializeField] private GameObject _mainMenuFirst;
    [SerializeField] private GameObject _settingsMenuFirst;

    private InputAction pause;
    private bool isPaused = false;

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
            Debug.Log("ASPODPÄDADA");
            if(isPaused == false)
            {
                Pause();
            }
            else if(isPaused == true)
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

        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }

    private void OpenSettingsMenu()
    {
        _settingsMenuCanvasGO.SetActive(true);
        _mainMenuCanvasGO.SetActive(false);
        EventSystem.current.SetSelectedGameObject(_settingsMenuFirst);
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

    public void OnSettingsPressed()
    {
        OpenSettingsMenu();
    }

    public void OnResumePressed()
    {
        UnPause();
    }

    public void OnSettingsBackPress()
    {
        OpenPauseMenu();
    }
}