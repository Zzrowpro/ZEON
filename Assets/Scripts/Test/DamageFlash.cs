using System.Collections;
using UnityEngine;

public class DamageFlash : MonoBehaviour
{
    [SerializeField] private Color _flashColor = Color.white;
    [SerializeField] private float flashTime = 0.25f;

    private static readonly int FlashColorID = Shader.PropertyToID("_FlashColor");
    private static readonly int FlashAmountID = Shader.PropertyToID("_FlashAmount");

    private SpriteRenderer spriteRenderer;
    private Material material;
    private Coroutine _damageFlashCoroutine;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        material = spriteRenderer.material; // creates a per-object instance
        material.SetColor(FlashColorID, _flashColor);
        material.SetFloat(FlashAmountID, 0f);
    }

    public void CallDamageFlash()
    {
        if (_damageFlashCoroutine != null)StopCoroutine(_damageFlashCoroutine);
        _damageFlashCoroutine = StartCoroutine(DamageFlasher());
    }

    private IEnumerator DamageFlasher()
    {
        material.SetColor(FlashColorID, _flashColor);   
        float elapsedTime = 0f;
        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            float amount = Mathf.Lerp(1f, 0f, elapsedTime / flashTime);
            material.SetFloat(FlashAmountID, amount);
            yield return null;
        }

        material.SetFloat(FlashAmountID, 0f);
        _damageFlashCoroutine = null;
    }

    void OnDestroy()
    {
        if (material != null) Destroy(material); // clean up the instance
    }
}