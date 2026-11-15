using UnityEngine;
using UnityEngine.Events;

public class AbilityLoadout : MonoBehaviour
{
    public int slotCount = 3;
    [SerializeField] public Ability[] slots;
    private UnityEvent useSlot;
    private UnityEvent isEquiped;

    void Awake()
    {
        if (slots == null || slots.Length != slotCount)
        System.Array.Resize(ref slots, slotCount);
    }
    public void UseSlot(int i)
    {
        if (i < 0 || i >= slots.Length) return;
        if (slots[i] != null) slots[i].TryUse();
    }

    public void Equip(Ability a, int slot)
    {
        if (slot < 0 || slot >= slots.Length) return;
        slots[slot] = a;
    }
    
}