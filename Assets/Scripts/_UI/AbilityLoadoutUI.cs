using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.UI;

[System.Serializable]
public class ControlIcon
{
    public string control;   // e.g. buttonSouth, buttonEast, buttonWest, buttonNorth, leftShoulder, rightTrigger
    public Sprite sprite;
}

public class AbilityLoadoutUI : MonoBehaviour
{
    public AbilityLoadout abilityLoadout;
    public List<TextMeshProUGUI> hotKeyDisplay;
    public List<Image> hotKeyIcons;          // one Image per slot, for controller icons
    public List<Image> images;
    public List<Image> cooldownOverlays;
    public List<ControlIcon> gamepadIcons;

    public InputActionAsset InputActions;

    [SerializeField] private string keyboardScheme = "Keyboard & Mouse";
    [SerializeField] private string gamepadScheme = "Gamepad";

    private InputActionMap abilitiesMap;
    private readonly List<InputAction> inputActions = new List<InputAction>();
    private readonly Dictionary<string, Sprite> iconLookup = new Dictionary<string, Sprite>();
    private System.IDisposable pressListener;
    private string currentGroup;
    private bool setupDone;

    private void OnEnable()  => pressListener = InputSystem.onAnyButtonPress.Call(OnAnyButtonPress);
    private void OnDisable() => pressListener?.Dispose();

    private void Start()
    {
        currentGroup = keyboardScheme;

        if (InputActions == null) { Debug.LogError("InputActions not assigned", this); return; }

        abilitiesMap = InputActions.FindActionMap("Abilities");
        if (abilitiesMap == null) { Debug.LogError("Action map 'Abilities' not found", this); return; }
        if (abilityLoadout == null) { Debug.LogError("abilityLoadout not assigned", this); return; }

        foreach (InputAction action in abilitiesMap.actions)
            inputActions.Add(action);

        foreach (ControlIcon icon in gamepadIcons)
            if (icon.sprite != null) iconLookup[icon.control] = icon.sprite;

        setupDone = true;
        RefreshHotkeys();
    }

    private void Update()
    {
        if (!setupDone) return;
        RefreshIcons();
    }

    private void OnAnyButtonPress(InputControl control)
    {
        if (!setupDone) return;

        InputDevice device = control.device;
        string group = (device is Keyboard || device is Mouse) ? keyboardScheme : gamepadScheme;
        if (group == currentGroup) return;

        currentGroup = group;
        RefreshHotkeys();
    }

    private void RefreshHotkeys()
    {
        for (int i = 0; i < inputActions.Count && i < hotKeyDisplay.Count; i++)
        {
            GetBinding(inputActions[i], currentGroup, out string text, out Sprite icon);

            hotKeyDisplay[i].text = icon != null ? "" : text;

            if (hotKeyIcons != null && i < hotKeyIcons.Count && hotKeyIcons[i] != null)
            {
                hotKeyIcons[i].sprite = icon;
                hotKeyIcons[i].enabled = icon != null;
            }
        }
    }

    private void GetBinding(InputAction action, string group, out string text, out Sprite icon)
    {
        text = "";
        icon = null;

        for (int b = 0; b < action.bindings.Count; b++)
        {
            InputBinding binding = action.bindings[b];
            if (binding.isComposite) continue;
            if (!BelongsToGroup(binding, group)) continue;

            if (text == "") text = action.GetBindingDisplayString(b);   // fallback text

            // "<Gamepad>/buttonSouth" -> "buttonSouth"
            string path = binding.effectivePath;
            string controlName = path.Substring(path.LastIndexOf('/') + 1);

            if (iconLookup.TryGetValue(controlName, out Sprite found))
            {
                icon = found;
                return;
            }
        }
    }

    private bool BelongsToGroup(InputBinding binding, string group)
    {
        foreach (string g in binding.groups.Split(InputBinding.Separator))
            if (g == group) return true;
        return false;
    }

    private void RefreshIcons()
    {
        int count = Mathf.Min(images.Count, abilityLoadout.slots.Length);
        for (int i = 0; i < count; i++)
        {
            if (images[i] == null) continue;
            Ability a = abilityLoadout.slots[i];
            if (a == null) { images[i].sprite = null; continue; }
            images[i].sprite = a.IsReady ? a.sprite : a.DeactivatedSprite;
            if (i < cooldownOverlays.Count && cooldownOverlays[i] != null)
                cooldownOverlays[i].fillAmount = a.CooldownNormalized;
        }
    }
}