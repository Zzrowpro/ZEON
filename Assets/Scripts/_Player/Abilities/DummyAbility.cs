using UnityEngine;

public class DummyAbility : Ability
{

    [SerializeField]private GameObject dummyPrefab;
    private GameObject _player;
    private Transform _playerTransform;
    private Rigidbody2D _playerRigidbody2D;

    void Awake()
    {
        _player = GameObject.Find("Player");
        _playerTransform = _player.transform;
        _playerRigidbody2D = _player.GetComponent<Rigidbody2D>();
    }


    //The script should spawn a dummy that looks like the player and has same name
    protected override void Activate()
    {
        Vector2 position = _playerTransform.position;
        Quaternion rotation = _playerTransform.rotation;
        GameObject dummy = Instantiate(dummyPrefab, position, rotation);
        dummy.GetComponent<Rigidbody2D>().AddForce(dummy.transform.up * _playerRigidbody2D.linearVelocity.magnitude, ForceMode2D.Impulse);
    }

}
