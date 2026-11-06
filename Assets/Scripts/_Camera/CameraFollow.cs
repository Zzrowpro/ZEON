using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;        // Reference to the player
    private Vector3 offset;         // The start offset between player and camera
    public float smoothing = 5f;   // The catch up speed for the camera


    Vector3 newPosition;

    void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
    }

    void Start()
    {
        // Get start offset 
        transform.position = player.position + new Vector3(0,0,-1);
        offset = transform.position - player.position;
    }


    void FixedUpdate()
    {
        // Move camera
        if (player != null)
            newPosition = player.position + offset;

        transform.position = Vector3.Lerp(transform.position, newPosition, smoothing * Time.fixedDeltaTime);
    }
}
