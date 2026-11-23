using System.Collections;
using DG.Tweening;
using UnityEngine;

public class SlideIn : MonoBehaviour
{
    [SerializeField] float duration = 1f;
    [SerializeField] Vector2 startOffset = new Vector2(0, -954);

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
        tween = rect.DOAnchorPos(targetPos, duration).SetEase(Ease.OutBack).SetUpdate(true);
        tween.Play();
    }

    public IEnumerator DisableGameObject()
    {
        tween = rect.DOAnchorPos(startOffset, duration).SetEase(Ease.OutBack).SetUpdate(true);
        tween.Play();
        yield return new WaitForSeconds(duration);
        gameObject.SetActive(false);
    }

    void OnDisable()
    {
        tween?.Kill();                              
    }
}