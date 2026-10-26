

using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshPro))]
public class Popup : MonoBehaviour
{
    private TextMeshPro text;
    [SerializeField] private float lifeTime = 2;
    [SerializeField]private float driftSpeed = 0.25f;
    [SerializeField]private float rotateSpeed = 10;
    private float age;
    public int points = 1;

    void Awake()
    {
        text = GetComponent<TextMeshPro>();
        Destroy(gameObject, lifeTime);
    }

    void Start()
    {
       text.text = $"{points} {(points > 1 ?"points" : "point" )}"; 
    }

    void Update()
    {
        age += Time.deltaTime;

        transform.Translate(driftSpeed * Time.deltaTime * Vector3.up, Space.World);
        transform.Rotate(rotateSpeed * Time.deltaTime * transform.forward);

        float fadeStart = lifeTime/2;
        if(age > fadeStart)
        text.alpha = Mathf.InverseLerp(lifeTime, fadeStart, age);
    }
}
