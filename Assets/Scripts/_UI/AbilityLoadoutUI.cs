using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AbilityLoadoutUI : MonoBehaviour
{
    public AbilityLoadout abilityLoadout;
    public List<TextMeshProUGUI> textMeshProUGUIs;
    private List<Sprite> sprites;

    private int slotCount;
    private void Awake()
    {
        slotCount = abilityLoadout.slotCount;
        if(abilityLoadout != null)
        {
            for (int i = 0; i < slotCount; i++)
            {
                if(abilityLoadout.slots[i] != null) continue;
                sprites.Add(abilityLoadout.slots[i].sprite);
            }
        }
    } 
}