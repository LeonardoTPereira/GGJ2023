using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UI.Utils;
using UnityEngine.InputSystem;

public class StartViewPresenter : MonoBehaviour
{
    [SerializeField] private UIDocument _UIDocument;
    private VisualElement _settingsView;
    private VisualElement _startView;

    private MainMenuPresenter _menuPresenter;

    private void Start()
    {
        VisualElement root = _UIDocument.rootVisualElement;

        _startView = root.Q("FrontPageMainMenu");
        _settingsView = root.Q("MainSettingsMenu");

        SetStartButtonsEvents();
    }

    private void SetStartButtonsEvents()
    {
        _menuPresenter = new MainMenuPresenter(_startView);
        _menuPresenter.OpenSettings = () => ToggleSettingsMenu(true);

        MainMenuSettingsViewPresenter settingsPresenter = new MainMenuSettingsViewPresenter(_settingsView);
        settingsPresenter.BackAction = () => ToggleSettingsMenu(false);
    }

    public void ToggleSettingsMenu(bool enable)
    {
        _startView.Display(!enable);
        _settingsView.Display(enable);
    }
    
    public void OnDownButton()
    {
        _menuPresenter.DownPressed();
    }

    public void OnUpButton()
    {
        _menuPresenter.UpPressed();
    }
    
    
    public void OnSubmit()
    {
        _menuPresenter.SubmitPressed();
    }
    
}
