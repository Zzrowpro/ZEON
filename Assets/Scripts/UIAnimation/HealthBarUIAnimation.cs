using System;
using DG.Tweening;
using Unity.Mathematics;
using UnityEngine;

public class HealthBarUIAnimation : MonoBehaviour
{
    [SerializeField] private bool scale = true;

    [Header("Shake")]
    [SerializeField] private float shakeDuration = 1f;
    [SerializeField] private float shakeStrength = 3f;
    [SerializeField] private int shakeVibrato = 10;
    [SerializeField] private float randomness = 90;

    [Header("Scale")]
    [SerializeField] Vector3 targetScale = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private float scaleDuration = 2f;
    [SerializeField] private int scaleVibrato = 10;
    [SerializeField] private float elasticity = 1;

    public PlayerHealth playerHealth;
    public  RocketProfiles playerProfile;

    private int dangerThreshold;

    RectTransform rect;
    Tween tween;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        dangerThreshold = (int)math.round(0.3 * playerProfile.maxHealth);
    }

    void OnEnable()
    {
        playerHealth.TookDamage += ShakeAnimation;
    }
    void OnDisable()
    {
        playerHealth.TookDamage -= ShakeAnimation;
    }

    void Update()
    {
        
    }

    private void ShakeAnimation()
    {
        CustomDebug.LogMessage("RIFIFIIFIF");
        if(scale == false)
        tween = rect.DOShakeAnchorPos(shakeDuration, shakeStrength, shakeVibrato, randomness).SetUpdate(true);
        if(scale == true)
        tween = rect.DOPunchScale(targetScale, scaleDuration, scaleVibrato, elasticity);

        if(playerHealth.Health <= dangerThreshold && playerHealth.Health >= 0)
        {
            tween = rect.DOShakeAnchorPos(1, shakeStrength, shakeVibrato, randomness).SetUpdate(true).SetLoops(-1);  
        }
    }
}
