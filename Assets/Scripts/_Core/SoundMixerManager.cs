using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;    

    public void SetMasterVolume(float level)
    {
        // audioMixer.SetFloat("masterVolume", level)
        audioMixer.SetFloat("masterVolume", Mathf.Log10(level)* 20f);  // i have no clue what this math shi means but apparently u cant use sliders for volume bc mixers are not linear they are exponential
    }

    public void SetSoundFXVolume(float level)
    {
        // audioMixer.SetFloat("soundFXVolume", level);
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(level)*20); 
    }

    public void SetMusicVolume(float level)
    {
        // audioMixer.SetFloat("musicVolume", level);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(level)* 20f); 
    }
}
