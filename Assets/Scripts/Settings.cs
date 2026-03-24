using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings: MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SFXSlider;
    public Slider MouseSenSlider;
    public float MouseSen = 0.5f;

    private void Start()
    {
        if (PlayerPrefs.HasKey("MasterVol") || PlayerPrefs.HasKey("MusicVol") || PlayerPrefs.HasKey("SFXVol") || PlayerPrefs.HasKey("MouseSen"))
        {
            LoadSettings();
            Debug.Log("LOADED");
        }
        else
        {
            SetMasterVol(0.75f);
            SetMusicVol(0.5f);
            SetSFXVol(0.5f);
            SetMouseSen(0.75f);
        }
    }
    public void SetMasterVol(float vol)
    {
        audioMixer.SetFloat("MasterVol", Mathf.Log10(vol) * 20f);
        PlayerPrefs.SetFloat("MasterVol", vol);
    }
    public void SetMusicVol(float vol)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(vol) * 20f);
        PlayerPrefs.SetFloat("MusicVol", vol);
    }

    public void SetSFXVol(float vol)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(vol) * 20f);
        PlayerPrefs.SetFloat("SFXVol", vol);
    }

    public void SetMouseSen(float sen)
    {
        sen = MouseSenSlider.value;
        MouseSen = sen;
        PlayerPrefs.SetFloat("MouseSen", MouseSen);
    }

    public void LoadSettings()
    {
        MasterSlider.value = PlayerPrefs.GetFloat("MasterVol");
        MusicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        SFXSlider.value = PlayerPrefs.GetFloat("SFXVol");
        MouseSenSlider.value = PlayerPrefs.GetFloat("MouseSen");
    }

    //void SaveSettings(float vol)
    //{
    //    MasterVol = audioMixer.GetFloat("MasterVol", Mathf.Log10(vol) * 20f);
    //    PlayerPrefs.SetFloat("MasterVol", audioMixer.GetFloat("MasterVol", MasterVol));
    //}
}
