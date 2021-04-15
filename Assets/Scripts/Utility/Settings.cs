using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public AudioMixer Mixer;
    public Slider[] AudioSliders;
    public string[] SoundEffectNames;
    public TMP_Dropdown QualitySettingsDropdown;
    public TMP_Dropdown ResolutionDropdown;
    public Toggle FullscreenToggle;
    Resolution[] m_resolutions;
    Resolution m_currentResolution;
    private void Start()
    {
        LoadSettings();
    }
    public void LoadSettings()
    {
        LoadSoundSettings();
        LoadGraphicsSettings();
    }
    #region Sound
    public void LoadSoundSettings()
    {
        for (int i = 0; i < SoundEffectNames.Length; ++i)
        {
            Mixer.SetFloat(SoundEffectNames[i], PlayerPrefs.GetFloat(SoundEffectNames[i]));
            AudioSliders[i].value = PlayerPrefs.GetFloat(SoundEffectNames[i]);
        }
    }
    public void SetMasterVolume(float _value)
    {
        Mixer.SetFloat("Master", _value);
        PlayerPrefs.SetFloat("Master", _value);
    }
    public void SetEffectsVolume(float _value)
    {
        Mixer.SetFloat("Effects", _value);
        PlayerPrefs.SetFloat("Effects", _value);
    }
    public void SetMusicVolume(float _value)
    {
        Mixer.SetFloat("Music", _value);
        PlayerPrefs.SetFloat("Music", _value);
    }
    #endregion
    #region Graphics
    public void SetFullscreen(bool _state)
    {
        Screen.fullScreen = _state;
        PlayerPrefs.SetInt("Fullscreen", _state ? 1 : 0);
    }
    public void GetResolutions()
    {
        m_resolutions = Screen.resolutions;
        ResolutionDropdown.ClearOptions();
        List<string> ResolutionNames = new List<string>();
        string resolutionName;
        foreach (Resolution res in m_resolutions)
        {
            if (res.width >= 1366 && res.height >= 768)
            {
                resolutionName = "" + res.width + " x " + res.height + " @ " + res.refreshRate + " hz";
                ResolutionNames.Add(resolutionName);
            }
        }
        ResolutionDropdown.AddOptions(ResolutionNames);
    }
    public void SetResolution(int _index)
    {
        Resolution res = m_resolutions[_index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
        PlayerPrefs.SetInt("ResIndex", _index);
    }
    public void SetQualitySetting(int _index)
    {
        QualitySettings.SetQualityLevel(_index);
        PlayerPrefs.SetInt("QualityLevel", _index);
    }
    public void LoadGraphicsSettings()
    {
        QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("QualityLevel"));
        QualitySettingsDropdown.value = PlayerPrefs.GetInt("QualityLevel");
        GetResolutions();
        m_currentResolution = m_resolutions[PlayerPrefs.GetInt("ResIndex")];
        Screen.SetResolution(m_currentResolution.width, m_currentResolution.height, true);
        ResolutionDropdown.value = PlayerPrefs.GetInt("ResIndex");
        int boolState = PlayerPrefs.GetInt("Fullscreen");
        if (boolState <= 0)
        {
            Screen.fullScreen = false;
            FullscreenToggle.isOn = false;
        }
        else
        {
            Screen.fullScreen = true;
            FullscreenToggle.isOn = true;
        }
    }
    #endregion
}

