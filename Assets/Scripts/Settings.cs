using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings: MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    public Slider MasterSlider;
    public Slider MusicSlider;
    public Slider SFXSlider;
    public Slider MouseSenSlider;
    public TextMeshProUGUI MasterVolVal;
    public TextMeshProUGUI MusicVolVal;
    public TextMeshProUGUI SFXVolVal;
    public TextMeshProUGUI MouseSenVal;
    public float MouseSen = 0.5f;

    private void Start()
    {
        var scene = SceneManager.GetActiveScene();
        if (PlayerPrefs.HasKey("MasterVol") || PlayerPrefs.HasKey("MusicVol") || PlayerPrefs.HasKey("SFXVol") || PlayerPrefs.HasKey("MouseSen"))
        {
            LoadSettings();
            Debug.Log("LOADED settings");
        }
        else
        {
            Debug.Log("Default settings");
            SetMasterVol(0.75f);
            SetMusicVol(0.5f);
            SetSFXVol(0.5f);
            SetMouseSen(0.75f);
        }
    }
    public void SetMasterVol(float vol)
    {
        audioMixer.SetFloat("MasterVol", Mathf.Log10(vol) * 20f);
        var percentage = (vol / 1) * 100;
        MasterVolVal.text = percentage.ToString("F0");
        PlayerPrefs.SetFloat("MasterVol", vol);
    }
    public void SetMusicVol(float vol)
    {
        audioMixer.SetFloat("MusicVol", Mathf.Log10(vol) * 20f);
        var percentage = (vol / 1) * 100;
        MusicVolVal.text = percentage.ToString("F0");
        PlayerPrefs.SetFloat("MusicVol", vol);
    }

    public void SetSFXVol(float vol)
    {
        audioMixer.SetFloat("SFXVol", Mathf.Log10(vol) * 20f);
        var percentage = (vol / 1) * 100;
        SFXVolVal.text = percentage.ToString("F0");
        PlayerPrefs.SetFloat("SFXVol", vol);
    }

    public void SetMouseSen(float sen)
    {
        MouseSen = sen;
        sen = MouseSenSlider.value;
        var percentage = (sen / 1) * 100;
        MouseSenVal.text = percentage.ToString("F0");
        PlayerPrefs.SetFloat("MouseSen", MouseSen);
    }

    public void LoadSettings()
    {
        MasterSlider.value = PlayerPrefs.GetFloat("MasterVol");
        audioMixer.SetFloat("MasterVol", Mathf.Log10(PlayerPrefs.GetFloat("MasterVol")) * 20f);
        var masterPercentage = (PlayerPrefs.GetFloat("MasterVol") / 1) * 100;
        MasterVolVal.text = masterPercentage.ToString("F0");

        MusicSlider.value = PlayerPrefs.GetFloat("MusicVol");
        audioMixer.SetFloat("MusicVol", Mathf.Log10(PlayerPrefs.GetFloat("MusicVol")) * 20f);
        var musicPercentage = (PlayerPrefs.GetFloat("MusicVol") / 1) * 100;
        MusicVolVal.text = musicPercentage.ToString("F0");

        SFXSlider.value = PlayerPrefs.GetFloat("SFXVol");
        audioMixer.SetFloat("SFXVol", Mathf.Log10(PlayerPrefs.GetFloat("SFXVol")) * 20f);
        var sfxPercentage = (PlayerPrefs.GetFloat("SFXVol") / 1) * 100;
        SFXVolVal.text = sfxPercentage.ToString("F0");

        MouseSenSlider.value = PlayerPrefs.GetFloat("MouseSen");
        MouseSen = PlayerPrefs.GetFloat("MouseSen");
        var mouseSenPercentage = (MouseSen / 1) * 100;
        MouseSenVal.text = mouseSenPercentage.ToString("F0");
    }

    //void SaveSettings(float vol)
    //{
    //    MasterVol = audioMixer.GetFloat("MasterVol", Mathf.Log10(vol) * 20f);
    //    PlayerPrefs.SetFloat("MasterVol", audioMixer.GetFloat("MasterVol", MasterVol));
    //}
}
