using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UI.Utils;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private VisualElement _root;
    private VisualElement _pauseMenu;

    private bool _isPauseMenuActive;

    private void Start()
    {

        VisualElement _root = GetComponent<UIDocument>().rootVisualElement;

        _pauseMenu = _root.Q<VisualElement>("PauseMenu");
        Button startButton = _pauseMenu.Q<Button>("main-menu");
        Button exitButton = _pauseMenu.Q<Button>("exit");

        startButton.clicked += () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        exitButton.clicked += () => Application.Quit();

        UIUtils.Display(_pauseMenu, false);

        _isPauseMenuActive = false;
    }

    public void PressPauseButton(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isPauseMenuActive = !_isPauseMenuActive;
            //UIUtils.Display(_pauseMenu, _isPauseMenuActive);
            tes(_pauseMenu, _isPauseMenuActive);
        }
    }

    public void tes(VisualElement panel, bool finalState)
    {
        if (panel == null)
        {
            Debug.Log("aaaaaaaa");
            return;
        }
        panel.style.display = finalState ? DisplayStyle.Flex : DisplayStyle.None;
    }
}
