using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class Dummy : Decoy
{
    [SerializeField] private float lifeTime = 4f;
    private GameObject _player;
    private Transform _playerTransform;
    private SpriteRenderer _playerSpriteRenderer;
    private SpriteRenderer _spriteRenderer;
    private Health health;

    void Awake()
    {
        health = GetComponent<Health>();
        _player = GameObject.Find("Player");
        _playerTransform = _player.GetComponent<Transform>(); 
        _playerSpriteRenderer = _player.GetComponent<SpriteRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = _playerSpriteRenderer.sprite;
        transform.localScale = new Vector3(_playerTransform.transform.localScale.x,_playerTransform.transform.localScale.y,0);
        Destroy(gameObject, lifeTime);

        }

    void OnCollisionEnter2D(Collision2D collision)
    {
        health.TakeDamage(1);
    }


}
