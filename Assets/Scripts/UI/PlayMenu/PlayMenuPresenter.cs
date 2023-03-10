using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UI.Utils;
using UnityEngine.InputSystem;

namespace UI.Presenters
{
    public class PlayMenuPresenter : MonoBehaviour
    {
        private VisualElement _lifeBar;
        private VisualElement _pauseMenu;
        //private VisualElement _settingsMenu;
        //private VisualElement _gameoverMenu;
        private VisualElement _winMenu;

        private bool _isPaused;

        void Start()
        {
            _isPaused = false;

            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            _lifeBar = root.Q<VisualElement>("life-bar-container");
            _pauseMenu = root.Q<VisualElement>("pause-menu-container");
            _winMenu = root.Q<VisualElement>("win-menu-container");
        }

        private void SetPauseMenuEvents()
        {
            PauseMenuPresenter pauseMenuPresenter = new PauseMenuPresenter(_pauseMenu);
            pauseMenuPresenter.ResumeAction = () => TogglePauseMenu(false);
            pauseMenuPresenter.ResumeAction = () => PauseOrUnpause();
        }

        private void TogglePauseMenu(bool enable)
        {
            _lifeBar.Display(!enable);
            _pauseMenu.Display(enable);
        }

        private void PauseOrUnpause()
        {
            Time.timeScale = 0;
            if (_isPaused)
                Time.timeScale = 1;

            _isPaused = !_isPaused;
        }
        public void PressPauseButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                PauseOrUnpause();
                TogglePauseMenu(_isPaused);
            }
        }
    }
}