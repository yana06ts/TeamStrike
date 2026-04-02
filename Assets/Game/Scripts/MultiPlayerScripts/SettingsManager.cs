using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio")]
    public Slider VolumeSlider;

    [Header("Graphics")]
    public Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    void Start()
    {
        VolumeSlider.value = PlayerPrefs.GetFloat("Volume", 1f);

        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResIndex = 0;
        var options = new System.Collections.Generic.List<string>();
        var seen = new System.Collections.Generic.HashSet<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            if (!seen.Add(option)) continue; 

            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResIndex = options.Count - 1; 
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float value)
    {
        AudioManager.instance.musicSource.volume = value;
        AudioManager.instance.sfxSource.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}