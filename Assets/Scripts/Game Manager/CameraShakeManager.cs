using Cinemachine;
using UnityEngine;
public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;

    [SerializeField]private float globalShakeForce = 1;

    void Awake()
    {
        if( instance == null)
        {
            instance = this;
        }
    } 

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        impulseSource.GenerateImpulseWithForce(globalShakeForce);
    }
}