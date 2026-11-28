using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.EventSystems;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField] Image background;
    [SerializeField] CanvasGroup content;
    [SerializeField] RectTransform title;
    [SerializeField] GameObject firstButton;

    public void Show()
    {
        if (DataManager.instance.isGameOver) return;
        DataManager.instance.isGameOver = true;
        EventSystem.current.firstSelectedGameObject = firstButton;

        gameObject.SetActive(true);
        background.color = new Color(0, 0, 0, 0);
        content.alpha = 0;
        content.interactable = false;
        title.localScale = Vector3.one * 2f;

        DOTween.Sequence()
            .SetUpdate(true)               // runs while timeScale = 0
            .SetLink(gameObject)           // dies with this object
            .Append(background.DOFade(0.85f, 0.6f))
            .Append(content.DOFade(1f, 0.3f))
            .Join(title.DOScale(1f, 0.4f).SetEase(Ease.OutBack))
            .Join(title.DOShakeAnchorPos(0.4f, 20f, 30))
            .OnComplete(() => content.interactable = true);

        Time.timeScale = 0f;
    }
}