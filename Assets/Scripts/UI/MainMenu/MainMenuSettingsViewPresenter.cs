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
        SetoutSlider(root, _mainVolumeSlider, "MainVolume", "main-volume-slider");
        SetoutSlider(root, _musicVolumeSlider, "MusicVolume", "music-volume-slider");
        SetoutSlider(root, _sfxVolumeSlider, "SfxVolume", "sfx-volume-slider");
    }

    private void SetoutSlider(VisualElement root, Slider slider, string mixerVolumeName, string sliderName)
    {
        slider = root.Q<Slider>(sliderName);

        float mixerValue;
        _mixer.GetFloat(mixerVolumeName, out mixerValue);
        slider.value = AudioManager.Instance.ConvertVolumeToSliderValue(mixerValue);

        slider.RegisterValueChangedCallback((value) => AudioManager.Instance.SetVolume(mixerVolumeName, value.newValue, true));
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
}
