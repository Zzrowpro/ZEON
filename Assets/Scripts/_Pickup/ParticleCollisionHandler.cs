using UnityEngine;

public class ParticleCollisionHandler : MonoBehaviour
{

    private ParticleSystem m_particleSystem;
    private Transform targetTransform;

    void Awake()
    {
        m_particleSystem = GetComponent<ParticleSystem>();
        targetTransform = GameObject.Find("Player").transform; 
    }

    void Start()
    {
        ParticleSystem.TriggerModule triggerModule = m_particleSystem.trigger;
        triggerModule.enabled = true; 

        triggerModule.SetCollider(0, targetTransform);
        

        triggerModule.enter = ParticleSystemOverlapAction.Callback;
        triggerModule.exit = ParticleSystemOverlapAction.Kill;
    }

    void OnParticleTrigger()
    {
        DataManager.instance.points += 1;

    }
}
