using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Dummy : MonoBehaviour
{
    private GameObject _player;
    private Transform _playerTransform;
    private SpriteRenderer _playerSpriteRenderer;
    private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _player = GameObject.Find("Player");
        _playerTransform = _player.GetComponent<Transform>(); 
        _playerSpriteRenderer = _player.GetComponent<SpriteRenderer>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _spriteRenderer.sprite = _playerSpriteRenderer.sprite;
        transform.localScale = new Vector3(_playerTransform.transform.localScale.x,_playerTransform.transform.localScale.y,0);
    } 


}
