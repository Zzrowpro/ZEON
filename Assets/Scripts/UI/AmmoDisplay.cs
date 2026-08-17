using TMPro;
using UnityEngine;

public class AmmoDisplay : MonoBehaviour
{
    [SerializeField] private PlasmaCanon PlasmaCanon;
    [SerializeField]private TextMeshProUGUI ammoDisplay;

    void Update()
    {
        if (ammoDisplay)
        {
            ammoDisplay.text = $"X {PlasmaCanon.Ammo}";
        }
    }
}
