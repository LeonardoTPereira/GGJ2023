using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuPresenter
{
    public Action OpenSettings { set => _settingsButton.clicked += value; }

    private Button _startButton;
    private Button _settingsButton;
    private Button _exitButton;

    public MainMenuPresenter(VisualElement root)
    {
        _startButton = root.Q<Button>("play-button");
        _settingsButton = root.Q<Button>("settings-button");
        _exitButton = root.Q<Button>("exit-button");

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
}
