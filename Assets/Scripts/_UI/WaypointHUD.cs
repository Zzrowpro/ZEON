using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaypointHUD : MonoBehaviour
{
    [SerializeField] RectTransform iconPrefab;   // Image + a child called "Arrow"
    [SerializeField] float edgeMargin = 40f;
    [SerializeField] float arrowDistance = 30f;

    readonly Dictionary<WaypointTarget, RectTransform> icons = new();
    Camera cam;

    Canvas canvas;
    RectTransform rect;
    Camera uiCam;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>().rootCanvas;
        rect = (RectTransform)transform;
        uiCam = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
    }

    void OnEnable()
    {
        WaypointTarget.Added   += Add;
        WaypointTarget.Removed += Remove;
        foreach (var t in WaypointTarget.All) Add(t);   // ones that enabled before us
    }

    void OnDisable()
    {
        WaypointTarget.Added   -= Add;
        WaypointTarget.Removed -= Remove;
    }

    void Add(WaypointTarget t)
    {
        if (icons.ContainsKey(t)) return;
        var icon = Instantiate(iconPrefab, transform);
        var img = icon.GetComponent<Image>();
        if (t.icon != null) img.sprite = t.icon;
        img.color = t.color;
        icons[t] = icon;
    }

    void Remove(WaypointTarget t)
    {
        if (icons.Remove(t, out var icon) && icon != null)
            Destroy(icon.gameObject);
    }

    void LateUpdate()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        Vector2 center = new Vector2(Screen.width, Screen.height) * 0.5f;
        Vector2 half = center - Vector2.one * edgeMargin;

        foreach (var kv in icons)
        {
            Vector2 p = cam.WorldToScreenPoint(kv.Key.transform.position + kv.Key.offset);
            Vector2 dir = p - center;

            bool onScreen = Mathf.Abs(dir.x) < half.x && Mathf.Abs(dir.y) < half.y;
            if (!onScreen)
            {
                // scale the direction until it touches the screen border
                float s = Mathf.Min(half.x / Mathf.Abs(dir.x), half.y / Mathf.Abs(dir.y));
                p = center + dir * s;
            }

            RectTransform icon = kv.Value;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, p, uiCam, out Vector2 local);
            icon.localPosition = local;

            Transform arrow = icon.Find("Arrow");
            arrow.gameObject.SetActive(!onScreen);
            if (!onScreen)
            {
                Vector2 n = dir.normalized;
                arrow.localPosition = n * arrowDistance;
                arrow.localRotation = Quaternion.Euler(0, 0, Mathf.Atan2(n.y, n.x) * Mathf.Rad2Deg - 90f);
            }
        }
    }
}