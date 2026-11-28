using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Collider2D))]
public class ExtractionShip : MonoBehaviour
{
    public InputActionAsset InputActions;
    public GameObject LevelMenu;
    public GameObject LevelMenuFirst;
    [SerializeField] float duration = 1f;
    [SerializeField] Vector2 startOffset = new Vector2(200, 0);

    Vector2 targetPos;
    Tween tween;
    InputAction extract;
    bool inRange;
    bool extracted;

    void Awake()
    {
        extract = InputActions.FindActionMap("Player", true).FindAction("extract", true);
        targetPos = transform.position;
    }

    void OnEnable()
    {
        extract.Enable();
        transform.position = targetPos + startOffset;
        tween = transform.DOMove(targetPos, duration).SetUpdate(true);
    }

    void OnDisable()
    {
        extract.Disable();
        tween?.Kill();
    }

    void Update()
    {
        if (extracted || !inRange || !extract.WasPressedThisFrame()) return;

        extracted = true;
        Time.timeScale = 0;
        if (LevelMenu != null) LevelMenu.SetActive(true);
        EventSystem.current.SetSelectedGameObject(LevelMenuFirst);
        
    }

    public IEnumerator DisableGameObject()
    {
        tween = transform.DOMove(targetPos + startOffset, duration).SetEase(Ease.OutBack).SetUpdate(true);
        yield return new WaitForSecondsRealtime(duration);
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D c) { if (c.CompareTag("Player")) inRange = true; }
    void OnTriggerExit2D(Collider2D c)  { if (c.CompareTag("Player")) inRange = false; }
}