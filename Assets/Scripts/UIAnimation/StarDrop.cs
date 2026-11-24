using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private List <GameObject> gameObjects;
    [SerializeField] private Vector2 offset = new Vector2(0, 954);
    [SerializeField] private float duration = 1f;
    [SerializeField] private float pause = 1f;
    [SerializeField] private Ease ease = Ease.OutBack;
    
    private List<RectTransform> rectTransforms = new List<RectTransform>();
    private List<Vector2> targetPos = new List<Vector2>();
    private Tween tween;


    void Awake()
    {
        AssignRectTransforms();
        AssignTargetPos();
        SetStartPosition();
    }

    void OnEnable()
    {
        SlideIn.OnDone += HandleAnimation;
        
    }
    void  OnDisable()
    {
        SlideIn.OnDone -= HandleAnimation;
        tween.Kill(); 
    }

    private void HandleAnimation()
    {
        // InvokeRepeating(nameof(NewTween), 0f,1f);
        StartCoroutine(Animation());
    }

    private void AssignRectTransforms()
    {
        foreach(GameObject gameObject in gameObjects)
        {
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            CustomDebug.LogMessage(rectTransform.ToString());
            rectTransforms.Add(rectTransform);
        }
    }

    private void AssignTargetPos()
    {
        foreach(RectTransform rectTransform in rectTransforms)
        {
            targetPos.Add(rectTransform.anchoredPosition);
        }
    }

    private void SetStartPosition()
    {
        int count = Mathf.Min(rectTransforms.Count, targetPos.Count);
        CustomDebug.LogMessage(count.ToString());
        for (int i = 0; i < count; i++)
        {
            rectTransforms[i].anchoredPosition = targetPos[i] + offset;
        }
    }

    private IEnumerator Animation()
    {
        int count = Mathf.Min(rectTransforms.Count, targetPos.Count);
        for (int i = 0; i < count; i++)
        {
            tween = rectTransforms[i].DOAnchorPos(targetPos[i], duration).SetEase(ease).SetUpdate(true); 
            yield return new WaitForSecondsRealtime(pause);
        }
    }

    // private void NewTween()
    // {
    //     int count = Mathf.Min(rectTransforms.Count, targetPos.Count);
    //     for (int i = 0; i < count; i++)
    //     {
    //        tween = rectTransforms[i].DOAnchorPos(targetPos[i], duration).SetEase(Ease.OutBack).SetUpdate(true);
    //     }
        
    // }




}