using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private void OnEnable()
    {
        VisualElement _root = GetComponent<UIDocument>().rootVisualElement;

        Button startButton = _root.Q<Button>("play-button");
        Button somethingButton = _root.Q<Button>("credits-button");
        Button exitButton = _root.Q<Button>("exit-button");

        startButton.clicked += () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        somethingButton.clicked += () => Debug.Log("AAAAAAA DO SOMETHING AAAAAAAAAAA");
        exitButton.clicked += () => Application.Quit();
    }
}
