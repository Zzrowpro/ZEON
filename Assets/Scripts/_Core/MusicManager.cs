using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] private List<AudioClip> tracks = new List<AudioClip>();
    [SerializeField] private AudioMixerGroup musicGroup;
    [SerializeField] private bool shuffle = true;
    [SerializeField] private bool playOnStart = true;
    [SerializeField, Range(0f, 1f)] private float volume = 0.6f;
    [SerializeField] private float fadeTime = 2f;

    private AudioSource active;
    private AudioSource idle;
    private Coroutine fadeRoutine;
    private int index = -1;
    private bool stopped;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void AutoCreate()
    {
        if (instance != null) return;
        var prefab = Resources.Load<MusicManager>("MusicManager");
        if (prefab == null)
        {
            Debug.LogError("MusicManager prefab not found at Resources/MusicManager");
            return;
        }
        Instantiate(prefab);
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

        active = gameObject.AddComponent<AudioSource>();
        idle = gameObject.AddComponent<AudioSource>();
        active.playOnAwake = idle.playOnAwake = false;
        active.loop = idle.loop = false;
        active.outputAudioMixerGroup = musicGroup;
        idle.outputAudioMixerGroup = musicGroup;
        
    }

    private void Start()
    {
        if (playOnStart && tracks.Count > 0) Next();
    }

    private void Update()
    {
        // Auto-advance: start fading into next track
        if (stopped || fadeRoutine != null || !active.isPlaying || active.clip == null) 
        return;
        if (active.clip.length - active.time <= fadeTime) 
        Next();
    }

    public void Next()
    {
        if (tracks.Count == 0) // PLS DONT FKING CRASH
        return;

        if (shuffle && tracks.Count > 1)
        {
            int i;
            do { i = Random.Range(0, tracks.Count); } 
            while (i == index);
            index = i;
        }
        else
        {
            index = (index + 1) % tracks.Count;
        }

        PlayClip(tracks[index]);
    }

    public void PlayTrack(int i)
    {
        if (i < 0 || i >= tracks.Count) 
        return;
        index = i;
        PlayClip(tracks[i]);
    }

    public void PlayClip(AudioClip clip)
    {
        if (clip == null) return;
        stopped = false;
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(CrossfadeRoutine(clip));
    }

    public void FadeOut(float time = 2f)
    {
        stopped = true;
        if (fadeRoutine != null) StopCoroutine(fadeRoutine);
        fadeRoutine = StartCoroutine(FadeOutRoutine(time));
    }

    public void SetVolume(float v)
    {
        volume = Mathf.Clamp01(v);
        if (fadeRoutine == null && active != null) active.volume = volume;
    }

    private IEnumerator CrossfadeRoutine(AudioClip clip)
    {
        (active, idle) = (idle, active);
        active.clip = clip;
        active.volume = 0f;
        active.Play();

        float idleStart = idle.volume;
        float duration = Mathf.Max(fadeTime, 0.01f);
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime; // keeps fading while the game is paused
            float k = t / duration;
            active.volume = Mathf.Lerp(0f, volume, k);
            idle.volume = Mathf.Lerp(idleStart, 0f, k);
            yield return null;
        }

        active.volume = volume;
        idle.Stop();
        idle.volume = 0f;
        fadeRoutine = null;
    }

    private IEnumerator FadeOutRoutine(float time)
    {
        float a = active.volume;
        float b = idle.volume;
        float duration = Mathf.Max(time, 0.01f);
        float t = 0f;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float k = t / duration;
            active.volume = Mathf.Lerp(a, 0f, k);
            idle.volume = Mathf.Lerp(b, 0f, k);
            yield return null;
        }

        active.Stop();
        idle.Stop();
        fadeRoutine = null;
    }
}