using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WaypointHUD : MonoBehaviour
{
    [SerializeField] RectTransform iconPrefab;   // Image + a child with a TMP_Text
    [SerializeField] Transform player;           // distance is measured from here
    [SerializeField] float edgeMargin = 40f;
    [SerializeField] float unitsToMeters = 1f;   // scale to taste
    [SerializeField] bool showWhenOnScreen = true;

    class Marker
    {
        public RectTransform rect;
        public TMP_Text label;
        public int lastDist = -1;
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
        foreach (var t in WaypointTarget.All) Add(t);
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
        if (t.icon != null) img.sprite = t.icon;
        img.color = t.color;

        markers[t] = new Marker { rect = icon, label = icon.GetComponentInChildren<TMP_Text>() };
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

        Vector2 center = new Vector2(Screen.width, Screen.height) * 0.5f;
        Vector2 half = center - Vector2.one * edgeMargin;
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
                float s = Mathf.Min(half.x / Mathf.Abs(dir.x), half.y / Mathf.Abs(dir.y));
                p = center + dir * s;
            }

            RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, p, uiCam, out Vector2 local);
            m.rect.localPosition = local;

            m.label.gameObject.SetActive(showWhenOnScreen || !onScreen);

            int d = Mathf.RoundToInt(Vector2.Distance(origin, t.transform.position) * unitsToMeters);
            if (d != m.lastDist)
            {
                m.lastDist = d;
                m.label.text = d + "m";
            }
        }
    }
}