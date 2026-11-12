using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float flashTime = 0.25f;

    private static readonly int FlashColorID = Shader.PropertyToID("_FlashColor");
    private static readonly int FlashAmountID = Shader.PropertyToID("_FlashAmount");

    private Material[] _materials;
    private Coroutine _damageFlashCoroutine;

    void Awake()
    {
        var renderers = GetComponentsInChildren<SpriteRenderer>(); // includes self
        _materials = new Material[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
        {
            _materials[i] = renderers[i].material; // per-object instance
            _materials[i].SetColor(FlashColorID, _flashColor);
            _materials[i].SetFloat(FlashAmountID, 0f);
        }
    }

    public void CallDamageFlash()
    {
        if (_damageFlashCoroutine != null) StopCoroutine(_damageFlashCoroutine);
        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher()
    {
        SetColor(_flashColor);
        float elapsed = 0f;
        while (elapsed < flashTime)
        {
            elapsed += Time.deltaTime;
            SetAmount(Mathf.Lerp(1f, 0f, elapsed / flashTime)); 
            yield return null;
        }
        SetAmount(0f);
        _damageFlashCoroutine = null;
    }

    private void SetColor(Color c)
    {
        foreach (var m in _materials) m.SetColor(FlashColorID, c);
    }

    private void SetAmount(float a)
    {
        foreach (var m in _materials) m.SetFloat(FlashAmountID, a);
    }

    void OnDestroy()
    {
        if (_materials == null) return;
        foreach (var m in _materials) if (m != null) Destroy(m);
    }
}