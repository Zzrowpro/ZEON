using UnityEngine;

public class UfoProjectile : MonoBehaviour
{
    [SerializeField]private bool targetPlayer = true;
    [SerializeField]private float time;

    private Transform _target;
    private Transform playerTransform;


}