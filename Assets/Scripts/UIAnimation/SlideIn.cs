using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SlideIn : MonoBehaviour
{
    [SerializeField] float duration = 1f;
    [SerializeField] Vector2 startOffset = new Vector2(0, -954);
    [SerializeField] Ease ease = Ease.OutBack;

    public static event Action OnDone;
    

    RectTransform rect;
    Vector2 targetPos;
    Tween tween;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        targetPos = rect.anchoredPosition;  
    }

    void OnEnable()
    {
        rect.anchoredPosition = targetPos + startOffset;
        tween = rect.DOAnchorPos(targetPos, duration).SetEase(ease).SetUpdate(true);
        tween.OnComplete(CallMyEventRetard);
        tween.Play();
    }

    public void CallMyEventRetard()
    {
        OnDone?.Invoke();
    }

    public IEnumerator DisableGameObject()
    {
        tween = rect.DOAnchorPos(startOffset, duration).SetEase(ease).SetUpdate(true);
        tween.Play();
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        tween?.Kill();                              
    }
}