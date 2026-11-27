using DG.Tweening;
using TMPro;
using UnityEngine;

public class LevelCompleteMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] ObjectiveManager manager;
    [SerializeField] Transform rowContainer;   // Vertical Layout Group inside the dark panel
    [SerializeField] TMP_Text rowPrefab;
    [SerializeField] TMP_Text pointsText;

    [Header("Timing")]
    [SerializeField] float rowDelay = 0.35f;
    [SerializeField] float countDuration = 1.5f;

    [Header("Testing")]
    [SerializeField] int testPoints = 1250;    // remove once GetPoints reads your real score

    Sequence seq;
    int shownPoints;

    void OnEnable()
    {
        foreach (Transform child in rowContainer) Destroy(child.gameObject);

        shownPoints = 0;
        pointsText.text = Format(0);

        // SetUpdate(true): keep animating while Time.timeScale is 0
        seq = DOTween.Sequence().SetUpdate(true);

        foreach (var o in manager.level.objectives)
        {
            TMP_Text row = Instantiate(rowPrefab, rowContainer);
            int count = Mathf.Min(manager.GetCount(o), o.amount);
            row.text = $"- {o.description} {count}/{o.amount}";
            row.transform.localScale = Vector3.zero;

            seq.AppendInterval(rowDelay);
            seq.Append(row.transform.DOScale(1f, 0.2f).SetEase(Ease.OutBack));
        }

        seq.AppendInterval(0.3f);
        seq.Append(
            DOTween.To(() => shownPoints,
                       v => { shownPoints = v; pointsText.text = Format(v); },
                       GetPoints(), countDuration)
                   .SetEase(Ease.OutQuad));
    }

    void OnDisable() => seq?.Kill();

    // hook this to a button or key to skip the animation
    public void Skip()
    {
        if (seq != null && seq.IsActive()) seq.Complete();
    }

    int GetPoints() => testPoints;   // TODO: return your HUD score here

    static string Format(int v) => v.ToString("D5");   // 00021 style, matches your HUD
}