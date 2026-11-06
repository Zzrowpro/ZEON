using Cinemachine;
using UnityEngine;
public class CameraShakeManager : MonoBehaviour
{
    public static CameraShakeManager instance;

    [SerializeField]private float globalShakeForce = 1;
    [SerializeField]private CinemachineImpulseListener impulseListener;

    private CinemachineImpulseDefinition impulseDefinition;

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

    public void ScreenShakeFromProfile(ScreenShakeProfile profile, CinemachineImpulseSource impulseSource)
    {
        //apply settings
        SetupScreenShakeSettings(profile, impulseSource);
        //screen shake
        impulseSource.GenerateImpulseWithForce(profile.impactForce);
    }

    private void SetupScreenShakeSettings(ScreenShakeProfile profile, CinemachineImpulseSource impulseSource)
    {

        //Change impulse source settings
        impulseDefinition = impulseSource.m_ImpulseDefinition;
        impulseDefinition.m_ImpulseDuration = profile.impactTime;
        impulseSource.m_DefaultVelocity = profile.defaultVelocity; 
        impulseDefinition.m_CustomImpulseShape = profile.impulseCurve;

        //Change impulse listener settings
        impulseListener.m_ReactionSettings.m_AmplitudeGain = profile.listenerAmplitude;
        impulseListener.m_ReactionSettings.m_FrequencyGain = profile.listenerFrequency;
        impulseListener.m_ReactionSettings.m_Duration = profile.listenerDuration; 
         
    }
}