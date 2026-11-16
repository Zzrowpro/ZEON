
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
    private List<Sprite> sprites = new List<Sprite>();
    private List<Sprite> deactivatedSprites = new List<Sprite>();

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
        AssignActionsToInputActions();
        AddSpritesToList();
        AddDeactivatedSpritesToList();
        AddSpritesToImages();
        SetHotkeyText();
    } 




    private void AssignActionsToInputActions()
    {
        foreach(InputAction action in abilitiesMap.actions)
        {
            inputActions.Add(action);
        }
    }

    private void AddSpritesToImages()
    {
        int count = Mathf.Min(images.Count, sprites.Count); //When sequencing 2 arrays or lists in a for loop use matf.min big bro-
        for (int i = 0; i < count; i++)
        {
            if (images[i] == null) continue;
            images[i].sprite = sprites[i];
        }
    }

    private void AddSpritesToList()
    {
        if(abilityLoadout != null)
        {
            int count = Mathf.Min(images.Count, abilityLoadout.slots.Length);
            for (int i = 0; i < count; i++) sprites.Add(abilityLoadout.slots[i] != null ? abilityLoadout.slots[i].sprite : null);
        }
    }

    public void AddDeactivatedSpritesToImages()
    {
        int count = Mathf.Min(images.Count, deactivatedSprites.Count);
        for (int i = 0; i < count; i++)
        {
            if(abilityLoadout.slots[i].TryUse() == false  && images[i] == null)
            {
                images[i].sprite = deactivatedSprites[i];
            }
        }
    }

    private void AddDeactivatedSpritesToList()
    {
        int count = Mathf.Min(images.Count, abilityLoadout.slots.Length);
        for (int i = 0; i < count; i++)deactivatedSprites.Add(abilityLoadout.slots[i] != null ? abilityLoadout.slots[i].DeactivatedSprite : null);
    }

    private void SetHotkeyText()
    {
        int count = Mathf.Min(hotKeyDisplay.Count, inputActions.Count);
        for (int i = 0; i < count; i++)
        {
            if (hotKeyDisplay[i] == null) continue;
            hotKeyDisplay[i].text =  hotKeyDisplay[i].text = inputActions[i].GetBindingDisplayString(0);
        }

    }

    private void CheckForSlotsOnCooldown()
    {
        foreach(Ability ability in abilityLoadout.slots)
        {
            if(ability.TryUse() == false)
            {
                
            }
        }
    }
    
}