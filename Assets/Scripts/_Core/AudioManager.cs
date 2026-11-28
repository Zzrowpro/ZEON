using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource soundFXObject;
    [SerializeField] private float pitchVariance = 0.05f;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoCreate()
    {
        if (instance == null)
            Instantiate(Resources.Load<AudioManager>("AudioManager"));
    }
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySoundFXClip(AudioClip audioClip, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundFXObject, spawnTransform.position, Quaternion.identity); 
        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.pitch = Random.Range(1f - pitchVariance, 1f + pitchVariance);
        audioSource.Play();
        float clipLength = audioSource.clip.length;
        Destroy(audioSource.gameObject, clipLength);
    }

    public AudioSource PlayLoop(AudioClip clip, Transform parent, float volume)
    {
        if (clip == null) return null;
        AudioSource source = Instantiate(soundFXObject, parent);
        source.transform.localPosition = Vector3.zero;
        source.clip = clip;
        source.volume = volume;
        source.loop = true;
        source.Play();
        return source;
    }

    public void StopLoop(AudioSource source)
    {
        if (source != null) Destroy(source.gameObject);
    }
}
