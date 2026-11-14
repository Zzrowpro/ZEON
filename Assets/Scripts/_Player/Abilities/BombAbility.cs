using UnityEngine;

public class BombAbility : Ability
{
    private GameObject _player;
    private Rigidbody2D _rigibody2D;
    private Transform _transform;
    [SerializeField]private GameObject bombPrefab;
    [SerializeField]private int totalBombs = 2;
    [SerializeField]private float bombCooldown = 4.0f;
    [SerializeField]private bool canSpawnBomb = true;

    void Awake()
    {
        _player = GameObject.Find("Player"); 
        _rigibody2D = _player.GetComponent<Rigidbody2D>( );
        _transform = _player.GetComponent<Transform>();
        

    } 

    protected override void Activate()
    {
        SpawnBomb();
    }

     private void SpawnBomb()
    {
        GameObject bomb = Instantiate(bombPrefab, _transform.position, _transform.rotation);
        bomb.GetComponent<Rigidbody2D>().AddForce(bomb.transform.up * _rigibody2D.linearVelocity.magnitude, ForceMode2D.Impulse);
        totalBombs --;
    }
}