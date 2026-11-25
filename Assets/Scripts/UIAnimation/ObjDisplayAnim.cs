using System.Collections;
using DG.Tweening;
using UnityEngine;

public class ObjDisplayAnim : MonoBehaviour
{
    [SerializeField] float duration = 1f;
    [SerializeField] float startPause = 4f;
    [SerializeField] float toggleDuration = 0.5f;
    [SerializeField] Vector2 offset = new Vector2(1229, 0);
    [SerializeField] Ease ease = Ease.OutQuad;
    [SerializeField] bool stagnant = false;

    private bool toggled = false;

    RectTransform rect;
    Vector2 targetPos;
    Tween tween;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        targetPos = rect.anchoredPosition;
    }

    void Start()
    {
        if(stagnant == false)
        StartCoroutine(StartAnimation()); 
    }

    private IEnumerator StartAnimation()
    {
        Vector2 offsetPos = targetPos  + offset;
        rect.anchoredPosition = offsetPos;
        tween = rect.DOAnchorPos(targetPos, duration).SetEase(ease).SetUpdate(true);
        yield return new WaitForSecondsRealtime(4f);
        tween = rect.DOAnchorPos(offsetPos, duration).SetEase(ease).SetUpdate(true);
    }

    public void ToggleAnimation()
    {
        if (toggled == true)
        {
            StopCoroutine(StartAnimation());
            Vector2 offsetPos = targetPos  + offset;
            toggled = !toggled;
            tween = rect.DOAnchorPos(offsetPos, toggleDuration).SetEase(ease).SetUpdate(true);
        }

        else if(toggled == false)
        {
            StopCoroutine(StartAnimation());
            tween = rect.DOAnchorPos(targetPos, toggleDuration).SetEase(ease).SetUpdate(true);
            toggled = !toggled; 
        }
    }

}