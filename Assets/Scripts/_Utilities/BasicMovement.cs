using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
public class BasicMovement : MonoBehaviour
{
    [SerializeField]private float driftSpeed = 0.25f;
    [SerializeField]private float rotateSpeed = 10;

    void Update()
    {
        HandleMovement();
    }


    protected void HandleMovement()
    {
        transform.Translate(driftSpeed * Time.deltaTime * Vector3.up, Space.World);
        transform.Rotate(rotateSpeed * Time.deltaTime * transform.forward);
    }
}