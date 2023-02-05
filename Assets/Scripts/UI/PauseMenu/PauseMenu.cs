using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using UI.Utils;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private VisualElement _root;

    private void OnEnable()
    {
        VisualElement _root = GetComponent<UIDocument>().rootVisualElement;

        Button startButton = _root.Q<Button>("play-button");
        Button exitButton = _root.Q<Button>("exit-button");

        startButton.clicked += () => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        exitButton.clicked += () => Application.Quit();

        UIUtils.Display(_root, true);
    }


    private void SetPanelEnableState (VisualElement panel, bool finalState)
    {
        if (finalState)
            panel.style.display = DisplayStyle.Flex;
        else
            panel.style.display = DisplayStyle.None;

        panel.SetEnabled(finalState);
        
    }
}
