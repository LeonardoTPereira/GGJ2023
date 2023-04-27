using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UIElements;

public class MainMenuSettingsViewPresenter
{
    private const int START_INDEX = -1;
    private const int FULLSCREEN_INDEX = 0;
    private const int RESOLUTION_INDEX = 1;
    private const int MAIN_VOLUME_INDEX = 2;
    private const int MUSIC_VOLUME_INDEX = 3;
    private const int SFX_VOLUME_INDEX = 4;
    private const int BACK_BUTTON_INDEX = 5;

    private int _selectedIndex = START_INDEX;
    private bool _IsActive = false;

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

    public void SetSettingsMenu(bool act)
    {
        _IsActive = act;

        if (_IsActive)
            _selectedIndex = START_INDEX;
    }


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
        _mainVolumeSlider = SetoutSlider(root, _mainVolumeSlider, "MainVolume", "main-volume-slider");
        _musicVolumeSlider = SetoutSlider(root, _musicVolumeSlider, "MusicVolume", "music-volume-slider");
        _sfxVolumeSlider = SetoutSlider(root, _sfxVolumeSlider, "SfxVolume", "sfx-volume-slider");
    }

    private Slider SetoutSlider(VisualElement root, Slider slider, string mixerVolumeName, string sliderName)
    {
        slider = root.Q<Slider>(sliderName);

        float mixerValue;
        _mixer.GetFloat(mixerVolumeName, out mixerValue);
        slider.value = AudioManager.Instance.ConvertVolumeToSliderValue(mixerValue);

        slider.RegisterValueChangedCallback((value) => AudioManager.Instance.SetVolume(mixerVolumeName, value.newValue, true));

        return slider;
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

    public void DownPressed()
    {
        if (_IsActive)
        {
            if (_selectedIndex == START_INDEX || _selectedIndex == BACK_BUTTON_INDEX)
            {
                _selectedIndex = 0;
            }
            else
            {
                _selectedIndex += 1;
            }
        }
    }

    public void UpPressed()
    {
        if (_IsActive)
        {
            if (_selectedIndex == START_INDEX || _selectedIndex == FULLSCREEN_INDEX)
            {
                _selectedIndex = BACK_BUTTON_INDEX;
            }
            else
            {
                _selectedIndex -= 1;
            }
        }
    }

    private const float MIN_VOLUME_VALUE = 5.0f;

    public void RightPressed()
    {
        if (_IsActive)
        {
            switch(_selectedIndex)
            {
                case START_INDEX:
                    _selectedIndex = FULLSCREEN_INDEX;
                    break;
                case MAIN_VOLUME_INDEX:
                    _mainVolumeSlider.value += MIN_VOLUME_VALUE;
                    break;
                case MUSIC_VOLUME_INDEX:
                    _musicVolumeSlider.value += MIN_VOLUME_VALUE;
                    break;
                case SFX_VOLUME_INDEX:
                    _sfxVolumeSlider.value += MIN_VOLUME_VALUE;
                    break;
            }
        }
    }
    public void LeftPressed()
    {
        if (_IsActive)
        {
            switch (_selectedIndex)
            {
                case START_INDEX:
                    _selectedIndex = FULLSCREEN_INDEX;
                    break;
                case MAIN_VOLUME_INDEX:
                    _mainVolumeSlider.value -= MIN_VOLUME_VALUE;
                    break;
                case MUSIC_VOLUME_INDEX:
                    _musicVolumeSlider.value -= MIN_VOLUME_VALUE;
                    break;
                case SFX_VOLUME_INDEX:
                    _sfxVolumeSlider.value -= MIN_VOLUME_VALUE;
                    break;
            }
        }
    }

    public void ClickPressed()
    {
        if (_IsActive)
        {
            _selectedIndex = START_INDEX;
        }
    }

    // BUG DA UNITY. NAO DA PARA ABRIR NESSA VERSAO DE UNITY (TALVEZ DÊ NA VERSÃO 2022.1)
    public void SubmitPressed()
    {
        if (_IsActive && _selectedIndex == RESOLUTION_INDEX)
        {
            var e = new NavigationSubmitEvent();
            e.target = _resolutionSelection;
            _resolutionSelection.SendEvent(e);
            //_resolutionSelection.SendEvent(new NavigationSubmitEvent() { target = _resolutionSelection });
            //Debug.Log("SUBMITED");
            _resolutionSelection.RegisterCallback<NavigationSubmitEvent>(evt => Debug.Log("SUBMITED"), TrickleDown.TrickleDown);
        }
    }
}
