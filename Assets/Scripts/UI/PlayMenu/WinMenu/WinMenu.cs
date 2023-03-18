using UnityEngine.SceneManagement;
using UI.Utils;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class WinMenu : MonoBehaviour
{
    private VisualElement _menu;
    private bool _isPaused;
    public static WinMenu Instance;

    private void Start()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        VisualElement _root = GetComponent<UIDocument>().rootVisualElement;
        _menu = _root.Q<VisualElement>("win-menu-container");

        Button startButton = _menu.Q<Button>("play");
        Button mainMenuButton = _menu.Q<Button>("main-menu");
        Button exitButton = _menu.Q<Button>("exit");

        startButton.clicked += () => 
        { 
            Time.timeScale = 1;
            SpawnManager.Instance.ResetSpawnPoint();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        };
        mainMenuButton.clicked += () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        exitButton.clicked += () => Application.Quit();

        UIUtils.Display(_menu, false);

        _isPaused = false;
    }

    public void DisplayWinMenu()
    {
        Time.timeScale = 0;
        UIUtils.Display(_menu, true);

    }
}
