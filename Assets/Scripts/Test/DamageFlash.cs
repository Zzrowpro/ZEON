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
        SetFlashColor(); 
        float elapsedTime = 0f;
        while (elapsedTime < flashTime)
        {
            elapsedTime += Time.deltaTime;
            float amount = Mathf.Lerp(1f, 0f, elapsedTime / flashTime);
            SetFlashAmount(amount);
            yield return null;
        }

        material.SetFloat(FlashAmountID, 0f);
        _damageFlashCoroutine = null;
    }

     private void SetFlashColor()
    {
        material.SetColor(FlashColorID, _flashColor);
    }

    private void SetFlashAmount(float amount)
    {
        material.SetFloat(FlashAmountID, amount);
    }

    void OnDestroy()
    {
        if (material != null) Destroy(material); // clean up the instance
    }
}