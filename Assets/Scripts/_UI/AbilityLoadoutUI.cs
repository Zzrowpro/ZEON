
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AbilityLoadoutUI : MonoBehaviour
{
    public AbilityLoadout abilityLoadout;
    public List<TextMeshProUGUI> hotKeyDisplay;
    public List<Image> images;
    public List<Image> cooldownOverlays;

    public InputActionAsset InputActions;
    private InputActionMap abilitiesMap;
    private List<InputAction> inputActions = new List<InputAction>();


    private void Start()
    {
        if (InputActions == null)
        {
            Debug.LogError("InputActions not assigned in Inspector", this);
            return;
        }
        abilitiesMap = InputActions.FindActionMap("Abilities");
        if (abilitiesMap == null)
        {
            Debug.LogError("Action map 'Abilities' not found. Check the name for typos.", this);
            return;
        }
        if(abilityLoadout == null)
        {
            return;
        }
        AssignActionsToInputActions();

    }

    void Update()
    {
        RefreshIcons();
    }




    private void AssignActionsToInputActions()
    {
        foreach(InputAction action in abilitiesMap.actions)
        {
            inputActions.Add(action);
        }
    }

    private void RefreshIcons()
    {
        int count = Mathf.Min(images.Count, abilityLoadout.slots.Length);
        for (int i = 0; i < count; i++)
        {
            hotKeyDisplay[i].text =  inputActions[i].GetBindingDisplayString(0);
            if (images[i] == null) continue;
            Ability a = abilityLoadout.slots[i];
            if (a == null) { images[i].sprite = null; continue; }
            images[i].sprite = a.IsReady ? a.sprite : a.DeactivatedSprite;
            if (i < cooldownOverlays.Count && cooldownOverlays[i] != null)
            cooldownOverlays[i].fillAmount = a.CooldownNormalized;
        }
    }
    
}