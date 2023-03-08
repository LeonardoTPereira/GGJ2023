using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayMenuPresenter : MonoBehaviour
{
    private VisualElement _lifeBar;
    private VisualElement _pauseMenu;
    //private VisualElement _settingsMenu;
    //private VisualElement _gameoverMenu;
    private VisualElement _winMenu;

    void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;
        _lifeBar = root.Q<VisualElement>("life-bar-container");
        _pauseMenu = root.Q<VisualElement>("pause-menu-container");
        _winMenu = root.Q<VisualElement>("win-menu-container");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
