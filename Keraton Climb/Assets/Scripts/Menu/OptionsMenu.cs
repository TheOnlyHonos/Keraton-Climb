using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer masterVolumeMixer;

    [Header("Resolution Dropdown")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;

    [Header("Main Menu Obj")]
    [SerializeField] private bool isPauseMenu = false;
    [SerializeField] private GameObject player;
    private MainMenu mainMenu;
    private PlayerUI pauseMenu;

    private Resolution[] resolutions;

    void Start()
    {
        if (!isPauseMenu)
        {
            mainMenu = GetComponent<MainMenu>();
        } else pauseMenu = player.GetComponent<PlayerUI>();
        

        resolutions = Screen.resolutions;

        int currentResIndex = 0;

        List<string> resolutionOptions = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            resolutionOptions.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);
        resolutionDropdown.value = currentResIndex;
        resolutionDropdown.RefreshShownValue();
    }

    public void SetVolume(float volume)
    {
        masterVolumeMixer.SetFloat("masterVolume", volume);
        Debug.Log(volume);
    }

    public void SetGraphicsQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

        Debug.Log(resolution);
    }

    public void CloseOptionsMenu()
    {
        if (!isPauseMenu)
        {
            mainMenu.CloseOptionsMenu();
        }
        else pauseMenu.CloseOptionsMenu();
    }
}
