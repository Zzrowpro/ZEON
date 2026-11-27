using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaypointHUD : MonoBehaviour
{
    [SerializeField] RectTransform iconPrefab;   // Image + a child TMP_Text (anchors: middle-center)
    [SerializeField] Transform player;           // distance is measured from here
    [SerializeField] float marginLeft = 30f;
    [SerializeField] float marginRight = 30f;
    [SerializeField] float marginTop = 80f;
    [SerializeField] float marginBottom = 90f;
    [SerializeField] float labelGap = 12f;       // space between dot and text
    [SerializeField] float unitsToMeters = 1f;
    [SerializeField] bool showWhenOnScreen = true;

    class Marker
    {
        public RectTransform rect;
        public TMP_Text label;
        public int lastDist = -1;
        public int side;   // 1 = marker on left half (label goes right), -1 = right half
    }

    readonly Dictionary<WaypointTarget, Marker> markers = new();
    Canvas canvas;
    RectTransform rect;
    Camera uiCam;
    Camera cam;

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
        foreach (var t in WaypointTarget.All) Add(t);   // targets that enabled before us
    }

    void OnDisable()
    {
        WaypointTarget.Added   -= Add;
        WaypointTarget.Removed -= Remove;
    }

    void Add(WaypointTarget t)
    {
        if (markers.ContainsKey(t)) return;

        var icon = Instantiate(iconPrefab, transform);
        var img = icon.GetComponent<Image>();
        var label = icon.GetComponentInChildren<TMP_Text>();

        if (img == null || label == null)
        {
            Debug.LogError($"WaypointHUD: icon prefab needs an Image on the ROOT and a TMP_Text child. " +
                        $"Image found: {img != null}, Label found: {label != null}", iconPrefab);
            Destroy(icon.gameObject);
            return;
        }

        if (t.icon != null) img.sprite = t.icon;
        img.color = t.color;

        markers[t] = new Marker { rect = icon, label = label };
    }

    void Remove(WaypointTarget t)
    {
        if (markers.Remove(t, out var m) && m.rect != null)
            Destroy(m.rect.gameObject);
    }

    void LateUpdate()
    {
        if (cam == null) cam = Camera.main;
        if (cam == null) return;

        // area markers are allowed to sit in (keeps them off your HUD)
        Rect safe = Rect.MinMaxRect(marginLeft, marginBottom,
                            Screen.width - marginRight, Screen.height - marginTop);
        Vector2 center = safe.center;
        Vector2 half = safe.size * 0.5f;

        Vector2 origin = player != null ? player.position : cam.transform.position;

        foreach (var kv in markers)
        {
            WaypointTarget t = kv.Key;
            Marker m = kv.Value;

            Vector2 p = cam.WorldToScreenPoint(t.transform.position + t.offset);
            Vector2 dir = p - center;

            bool onScreen = Mathf.Abs(dir.x) < half.x && Mathf.Abs(dir.y) < half.y;
            if (!onScreen)
            {
                // stretch the direction until it touches the safe-area border
                float s = Mathf.Min(half.x / Mathf.Abs(dir.x), half.y / Mathf.Abs(dir.y));
                p = center + dir * s;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, p, uiCam, out Vector2 local);
            m.rect.localPosition = local;

            // keep the label on the inner side so it never hangs off the screen
            int side = p.x < Screen.width * 0.5f ? 1 : -1;
            if (side != m.side)
            {
                m.side = side;
                RectTransform lr = m.label.rectTransform;
                lr.pivot = new Vector2(side == 1 ? 0f : 1f, 0.5f);
                lr.anchoredPosition = new Vector2(side * labelGap, 0f);
                m.label.alignment = side == 1 ? TextAlignmentOptions.MidlineLeft
                                              : TextAlignmentOptions.MidlineRight;
            }

            m.label.gameObject.SetActive(showWhenOnScreen || !onScreen);

            // only rebuild the string when the number changes
            int d = Mathf.RoundToInt(Vector2.Distance(origin, t.transform.position) * unitsToMeters);
            if (d != m.lastDist)
            {
                m.lastDist = d;
                m.label.text = d + "m";
            }
        }
    }
}