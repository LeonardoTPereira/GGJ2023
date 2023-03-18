using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UI.Utils;
using UnityEngine.InputSystem;
using System;
using static UnityEditor.Recorder.OutputPath;
using UI.Menu.Play.Pause;

namespace UI.Menu.Play.Pause
{
    public class PauseMenuPresenter : MonoBehaviour
    {
        public Action ResumeAction { set => _resumeButton.clicked += value; }

        private VisualElement _frontPage;
        private VisualElement _settingsPage;

        // Main Front Panel
        private Button _resumeButton;
        private Button _settingsButton;
        private Button _mainMenuButton;
        private Button _exitButton;

        // Pause Settings Panel
        private Button _backButton;
        private Slider _mainVolumeSlider;
        private Slider _musicVolumeSlider;
        private Slider _sfxVolumeSlider;

        public PauseMenuPresenter(VisualElement root)
        {
            _frontPage = root.Q<VisualElement>("front-menu");
            _settingsPage = root.Q<VisualElement>("settings-menu");

            SetoutFrontPageButtons();
            SetoutSettingsPageButtons();
            SetoutSettingsVolumeSliders();
            //UIUtils.Display(_frontPage, false);
        }

        private void SetoutFrontPageButtons()
        {
            _resumeButton = _frontPage.Q<Button>("resume-button");
            _settingsButton = _frontPage.Q<Button>("settings-button");
            _mainMenuButton = _frontPage.Q<Button>("main-menu-button");
            _exitButton = _frontPage.Q<Button>("exit-game-button");

#if UNITY_EDITOR
            SetDebugLogs();
#endif

            _settingsButton.clicked += () => ClosePauseMenuSettings(false);
            _mainMenuButton.clicked += MainMenuPopUp;
            _exitButton.clicked += ExitPopUp;
        }

        private void SetoutSettingsPageButtons()
        {
            _backButton = _settingsPage.Q<Button>("back-button");
            _mainVolumeSlider = _settingsPage.Q<Slider>("main-volume-slider");
            _musicVolumeSlider = _settingsPage.Q<Slider>("music-volume-slider");
            _sfxVolumeSlider = _settingsPage.Q<Slider>("sfx-volume-slider");

            _backButton.clicked += () => ClosePauseMenuSettings(true);
        }

        private void SetoutSettingsVolumeSliders()
        {
            SetoutSlider(_settingsPage, _mainVolumeSlider, "MainVolume", "main-volume-slider");
            SetoutSlider(_settingsPage, _musicVolumeSlider, "MusicVolume", "music-volume-slider");
            SetoutSlider(_settingsPage, _sfxVolumeSlider, "SfxVolume", "sfx-volume-slider");
        }

        private void SetoutSlider(VisualElement root, Slider slider, string mixerVolumeName, string sliderName)
        {
            slider = root.Q<Slider>(sliderName);

            float mixerValue;
            AudioManager.Instance.GetMainMixer().GetFloat(mixerVolumeName, out mixerValue);
            slider.value = AudioManager.Instance.ConvertVolumeToSliderValue(mixerValue);

            slider.RegisterValueChangedCallback((value) => AudioManager.Instance.SetVolume(mixerVolumeName, value.newValue, true));
        }

        private void ClosePauseMenuSettings(bool enable)
        {
            _frontPage.Display(enable);
            _settingsPage.Display(!enable);
        }

        // Should ideally create a popup and ask the player if really wants to go back to main menu
        private void MainMenuPopUp()
        {
            PauseMenu.Instance.PauseOrUnpause();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }

        // Should ideally create a popup and ask the player if really wants to exit the game
        private void ExitPopUp()
        {
            Application.Quit();
        }
#if UNITY_EDITOR
        private void SetDebugLogs()
        {
            _resumeButton.clicked += () => Debug.Log("PRESSED RESUME BUTTON");
            _settingsButton.clicked += () => Debug.Log("PRESSED SETTINGS BUTTON");
            _mainMenuButton.clicked += () => Debug.Log("PRESSED BACK MAIN MENU BUTTON");
            _exitButton.clicked += () => Debug.Log("PRESSED EXIT BUTTON");
        }
#endif
    }

}
