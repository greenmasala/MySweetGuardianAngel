using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    float MasterVol = -30;
    float MusicVol = -30;
    float SFXVol = -30;

    private void Start()
    {
        
    }
    public void SetMasterVol(float vol)
    {
        audioMixer.SetFloat("MasterVol", Mathf.Log10(vol) * 20f);
        MasterVol = Mathf.Log10(vol) * 20f;
        PlayerPrefs.SetFloat("MasterVol", Mathf.Log10(MasterVol) * 20f);
        //SaveSettings(vol);
    }
    public void SetMusicVol(float vol)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(vol) * 20f);
        MusicVol = Mathf.Log10(vol) * 20f;
        PlayerPrefs.SetFloat("MusicVol", Mathf.Log10(vol) * 20f);
    }

    public void SetSFXVol(float vol)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(vol) * 20f);
        SFXVol = Mathf.Log10(vol) * 20f;
        PlayerPrefs.SetFloat("SFXVol", Mathf.Log10(vol) * 20f);
    }

    //void LoadSettings()
    //{
    //    audioMixer.SetFloat("MasterVol", PlayerPrefs.GetFloat("MasterVol", MasterVol));
    //}

    //void SaveSettings(float vol)
    //{
    //    MasterVol = audioMixer.GetFloat("MasterVol", Mathf.Log10(vol) * 20f);
    //    PlayerPrefs.SetFloat("MasterVol", audioMixer.GetFloat("MasterVol", MasterVol));
    //}
}
