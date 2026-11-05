using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(TextMeshProUGUI))]
public class PointHud : MonoBehaviour
{
    [Header("Scale")]
    [SerializeField] private Vector3 targetScale = new Vector3(2f, 2f, 1f);

    [Header("Alpha")]
    [SerializeField] private float idleAlpha = 0f;

    [Header("Timing")]
    [SerializeField] private float inflateDuration = 0.15f;
    [SerializeField] private float holdTime = 1f;
    [SerializeField] private float deflateDuration = 1.5f;

    private RectTransform rect;
    private TextMeshProUGUI text;
    private Vector3 initialScale;
    private int m_points;
    private Coroutine routine;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        text = GetComponent<TextMeshProUGUI>();
        initialScale = rect.localScale;

        SetLook(initialScale, idleAlpha);
    }

    
    // Call this every time the player earns points
    public void Pulse(int points)
    {
        if (routine != null)
            StopCoroutine(routine);

        routine = StartCoroutine(PulseRoutine(points));
    }

    private IEnumerator PulseRoutine(int points)
    {
        m_points += points;
        text.text = $"{m_points}x"; 
        Vector3 startScale = rect.localScale;
        float startAlpha = text.alpha;

        // Inflate: from wherever we are right now, up to full size and opacity
        for (float t = 0f; t < 1f; t += Time.deltaTime / inflateDuration)
        {
            SetLook(Vector3.Lerp(startScale, targetScale, t),
                    Mathf.Lerp(startAlpha, 1f, t));
            yield return null;
        }
        SetLook(targetScale, 1f);

        // Hold while the player might still be earning points
        yield return new WaitForSeconds(holdTime);

        // Deflate: shrink and fade back to idle
        for (float t = 0f; t < 1f; t += Time.deltaTime / deflateDuration)
        {
            SetLook(Vector3.Lerp(targetScale, initialScale, t),
                    Mathf.Lerp(1f, idleAlpha, t));
            yield return null;
        }
        SetLook(initialScale, idleAlpha);
        m_points = 0;
        routine = null;
    }

    private void SetLook(Vector3 scale, float alpha)
    {
        rect.localScale = scale;
        text.alpha = alpha;
    }
}