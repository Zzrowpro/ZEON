using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIControls : MonoBehaviour
{
    public ObjDisplayAnim objDisplayAnim;
    public InputActionAsset InputActions;
    public AudioClip buttonClick;


    private InputAction toggleObjPanel;

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
        var map = InputActions.FindActionMap("UI");
        toggleObjPanel = map.FindAction("ToggleObjPanel");
    }

    public void TogglePanel(GameObject gameObject)
    {
        if(gameObject.activeInHierarchy)
        gameObject.SetActive(false);
        else if(!gameObject.activeInHierarchy)
        gameObject.SetActive(true); 
    }

    public void PlaySFX()
    {
        AudioManager.instance.PlaySoundFXClip(buttonClick,transform, 1f);
    } 

    void Update()
    {
        if (toggleObjPanel.WasPressedThisFrame())
        {
            if (objDisplayAnim)
            {
                objDisplayAnim.ToggleAnimation();
            }
        }
    }
}
