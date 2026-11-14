using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(AbilityLoadout))]
public class AbilityInput : MonoBehaviour
{
    public InputActionAsset InputActions;

    private InputAction useSlot1;
    private InputAction useSlot2;
    private InputAction useSlot3;

    private AbilityLoadout abilityLoadout;

    void Awake()
    {
        var map = InputActions.FindActionMap("Abilities");
        useSlot1 = map.FindAction("UseSlot1");
        useSlot2 = map.FindAction("UseSlot2");
        useSlot3 = map.FindAction("UseSlot3");

        abilityLoadout = GetComponent<AbilityLoadout>();
    }

    void OnEnable()
    {
        InputActions.FindActionMap("Abilities").Enable();
    }
    void OnDisable()
    {
        InputActions.FindActionMap("Abilities").Disable();
    }


    void Update()
    {
        if (useSlot1.WasPressedThisFrame())
        {
            abilityLoadout.UseSlot(0);
        }
        else if (useSlot2.WasPressedThisFrame())
        {
            abilityLoadout.UseSlot(1);
        }
        else if (useSlot3.WasPressedThisFrame())
        {
            abilityLoadout.UseSlot(2);
        }
    } 
}