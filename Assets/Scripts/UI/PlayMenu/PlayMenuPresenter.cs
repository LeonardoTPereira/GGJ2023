using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UI.Utils;
using UnityEngine.InputSystem;
using UI.Menu.Play.Pause;

namespace UI.Menu.Play
{
    public class PlayMenuPresenter : MonoBehaviour
    {
        private VisualElement _lifeBarPanel;
        private VisualElement _pauseMenuPanel;
        //private VisualElement _winMenu;

        private PauseMenu _pauseMenu;

        void Start()
        {
            VisualElement root = GetComponent<UIDocument>().rootVisualElement;
            _lifeBarPanel = root.Q<VisualElement>("lifebar-container");
            _pauseMenuPanel = root.Q<VisualElement>("pause-menu-container");
            //_winMenu = root.Q<VisualElement>("win-menu-container");

            SetPauseMenuEvents();

            _pauseMenu = PauseMenu.Instance;
        }

        private void SetPauseMenuEvents()
        {
            PauseMenuPresenter pauseMenuPresenter = new PauseMenuPresenter(_pauseMenuPanel);
            pauseMenuPresenter.ResumeAction = () => TogglePauseMenu(false);
            pauseMenuPresenter.ResumeAction = () => _pauseMenu.PauseOrUnpause();
        }

        private void TogglePauseMenu(bool enable)
        {
            UIUtils.Display(_lifeBarPanel, !enable);
            UIUtils.Display(_pauseMenuPanel, enable);
        }

        public void PressPauseButton(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                TogglePauseMenu(_pauseMenu.IsPaused);
            }
        }
    }
}