using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuPresenter
{
    public Action OpenSettings { set => _settingsButton.clicked += value; }

    private Button _startButton;
    private Button _settingsButton;
    private Button _exitButton;

    private bool _IsSettingsMenuActive;

    private const int SETTINGS_BUTTON_INDEX = 1;


    private static List<Button> _buttons = new List<Button>();
    private static int _selectedIndex = -1;


    public MainMenuPresenter(VisualElement root) 
    {
        _IsSettingsMenuActive = false;

        _startButton = root.Q<Button>("play-button");
        _settingsButton = root.Q<Button>("settings-button");
        _exitButton = root.Q<Button>("exit-button");

        _buttons.Add(_startButton);
        _buttons.Add(_settingsButton);
        _buttons.Add(_exitButton);

        // Only for debug
        SetDebugToButtons();

        _startButton.clicked += () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    //  _settingsButton.clicked += () => ;
        _exitButton.clicked += () => Application.Quit();
    }



    private void SetDebugToButtons()
    {
        _startButton.clicked += () => Debug.Log("PRESSED PLAY BUTTON");
        _settingsButton.clicked += () => Debug.Log("PRESSED SETTINGS BUTTON");
        _exitButton.clicked += () => Debug.Log("PRESSED EXIT BUTTON");
        
    }
    
    public void DownPressed()
    {
        if (!_IsSettingsMenuActive)
        {
            if (_selectedIndex < 2)
                _selectedIndex += 1;
            _buttons[_selectedIndex].style.opacity = 1f;

            if (_selectedIndex - 1 >= 0)
                _buttons[_selectedIndex - 1].style.opacity = .7f;
        }
    }

    public void UpPressed()
    {
        if (!_IsSettingsMenuActive)
        {
            if (_selectedIndex <= 0)
                _selectedIndex = 0;
            else if (_selectedIndex > 0)
            {
                _selectedIndex -= 1;

                if (_selectedIndex + 1 < 3)
                    _buttons[_selectedIndex + 1].style.opacity = .7f;
            }
            _buttons[_selectedIndex].style.opacity = 1f;
        }
    }

    public void SetSettingsMenu(bool act)
    {
        _IsSettingsMenuActive = act;
    }

    public void SubmitPressed()
    {

        Debug.Log("INDEX SELECTED: " + _selectedIndex);
        if (!_IsSettingsMenuActive)
        {
            Button button = _buttons[_selectedIndex];
            /*
            var e = new NavigationSubmitEvent();
            e.target = null;
            e.target = button;
            Debug.Log(e);
            button.SendEvent(e);
            e.target = null;
            */
        
            using (var e = new NavigationSubmitEvent() { target = button })
                button.SendEvent(e);

            if (_selectedIndex == SETTINGS_BUTTON_INDEX)
            {
                _IsSettingsMenuActive = true;
            }
        }
    }
}
