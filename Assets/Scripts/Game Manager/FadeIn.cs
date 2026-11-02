using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public float duration = 0.5f;

    private SpriteRenderer[] renderers;
    private float[] targetAlphas;

    private Collider2D[] colliders;
    private bool[] colliderWasEnabled;

    private float age;
    
    void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        targetAlphas = new float[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            targetAlphas[i] = renderers[i].color.a;
            SetAlpha(renderers[i], 0f);
        }

        colliders = GetComponentsInChildren<Collider2D>();
        colliderWasEnabled = new bool[colliders.Length];

        for (int i = 0; i < colliders.Length; i++)
        {
            colliderWasEnabled[i] = colliders[i].enabled;
            colliders[i].enabled = false;
        }
    }

    void Update()
    {
        age += Time.deltaTime;
        float t = Mathf.Clamp01(age / duration);

        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i] == null) continue;
            SetAlpha(renderers[i], targetAlphas[i] * t);
        }

        if (t >= 1f)
        {
            RestoreColliders();
            Destroy(this);
        }
    }

    private void RestoreColliders()
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i] == null) continue;
            colliders[i].enabled = colliderWasEnabled[i];
        }
    }

    private void SetAlpha(SpriteRenderer sr, float alpha)
    {
        Color c = sr.color;
        c.a = alpha;
        sr.color = c;
    }
}