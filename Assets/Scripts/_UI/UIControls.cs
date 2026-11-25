using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIControls : MonoBehaviour
{
    public ObjDisplayAnim objDisplayAnim;
    public InputActionAsset InputActions;


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
