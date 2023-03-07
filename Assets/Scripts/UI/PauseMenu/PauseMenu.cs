using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UI.Utils;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    private VisualElement _settingsView;
    private VisualElement _menu;
    private bool _isPaused;

    private void Start()
    {

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        _menu = root.Q<VisualElement>("pause-menu-container");

        Button startButton = _menu.Q<Button>("play");
        Button mainMenuButton = _menu.Q<Button>("main-menu");
        Button exitButton = _menu.Q<Button>("exit");

        startButton.clicked += PauseOrUnpause;
        mainMenuButton.clicked += () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        exitButton.clicked += () => Application.Quit();

        UIUtils.Display(_menu, false);

        _isPaused = false;
    }

    public void PressPauseButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PauseOrUnpause();
        }
    }

    private void PauseOrUnpause()
    {
        Time.timeScale = 0;
        if (_isPaused)
            Time.timeScale = 1;

        _isPaused = !_isPaused;
        UIUtils.Display(_menu, _isPaused);
    }
}
