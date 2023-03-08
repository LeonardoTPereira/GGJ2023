using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;
using static UnityEditor.Recorder.OutputPath;

public class MainMenuSettingsViewPresenter
{
    private List<string> _resolutions = new List<string>()
    {
        "3840x2160",
        "2560x1448",
        "1920x1080",
        "1600x900",
        "1366x768",
        "1280x720"
    };

    public Action BackAction { set => _backButton.clicked += value; }

    private Button _backButton;
    private Toggle _fullScreenToggle;
    private DropdownField _resolutionSelection;
    private Slider _mainVolumeSlider;
    private Slider _musicVolumeSlider;
    private Slider _sfxVolumeSlider;

    private AudioMixer _mixer;

    public MainMenuSettingsViewPresenter(VisualElement root)
    {
        _backButton = root.Q<Button>("back-button");
        _fullScreenToggle = root.Q<Toggle>("fullscreen-toggle");
        _resolutionSelection = root.Q<DropdownField>("resolution-dropdown");

        _mixer = AudioManager.Instance.GetMainMixer();

        SetoutVolumeSliders(root);

        _fullScreenToggle.RegisterCallback<MouseUpEvent>((evt) => { SetFullscreen(_fullScreenToggle.value); }, TrickleDown.TrickleDown);
        _resolutionSelection.choices = _resolutions;
        _resolutionSelection.RegisterValueChangedCallback((value) => SetResolution(value.newValue));
        _resolutionSelection.index = 0;
    }

    private void SetoutVolumeSliders(VisualElement root)
    {
        _mainVolumeSlider = root.Q<Slider>("main-volume-slider");
        _musicVolumeSlider = root.Q<Slider>("music-volume-slider");
        _sfxVolumeSlider = root.Q<Slider>("sfx-volume-slider");

        SetoutSlider(_mainVolumeSlider, "MainVolume");
        SetoutSlider(_musicVolumeSlider, "MusicVolume");
        SetoutSlider(_sfxVolumeSlider, "SfxVolume");
    }

    private void SetoutSlider(Slider slider, string mixerVolume)
    {
        float mixerValue;
        _mixer.GetFloat(mixerVolume, out mixerValue);
        slider.value = Mathf.Pow(10, mixerValue / 20 + 2);

        slider.RegisterValueChangedCallback((value) => SetVolume(mixerVolume, value.newValue));
    }


    private void SetResolution(string newResolution)
    {
        string[] resolutionArray = newResolution.Split("x");
        int[] valuesInArray = new int[] { int.Parse(resolutionArray[0]), int.Parse(resolutionArray[1]) };

        Screen.SetResolution(valuesInArray[0], valuesInArray[1], _fullScreenToggle.value);
    }

    private void SetFullscreen(bool enabled)
    {
        Screen.fullScreen = enabled;
    }

    public void SetVolume(string mixerVolume, float sliderValue)
    {
        _mixer.SetFloat(mixerVolume, Mathf.Log10((sliderValue + 0.01f) / 100) * 20);
    }
}
