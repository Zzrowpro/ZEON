

using UnityEngine;

public class ProjectileCrateRemodel : MonoBehaviour
{

    void Update()
    {
        HandleMovement();
    }


    private void HandleMovement()
    {
        transform.position = transform.parent.position;
    }

}
