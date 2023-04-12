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
    private MainMenuSettingsViewPresenter _settingsPresenter;

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

        _settingsPresenter = new MainMenuSettingsViewPresenter(_settingsView);
        _settingsPresenter.BackAction = () => ToggleSettingsMenu(false);
//      _settingsPresenter.BackAction = () => _menuPresenter.SetSettingsMenu(false);
    }

    public void ToggleSettingsMenu(bool enable)
    {
        _startView.Display(!enable);
        _settingsView.Display(enable);
        _settingsPresenter.SetSettingsMenu(enable);
    }


    #region GET INPUT
    public void OnDownButton()
    {
        _settingsPresenter.DownPressed();
    }

    public void OnUpButton()
    {
        _settingsPresenter.UpPressed();
    }

    public void OnLeftButton()
    {
        _settingsPresenter.LeftPressed();
    }

    public void OnRightButton()
    {
        _settingsPresenter.RightPressed();
    }    
    
    public void OnSubmit()
    {
        _settingsPresenter.SubmitPressed();
    }
    
    public void OnClick()
    {
        _settingsPresenter.ClickPressed();
    }
    #endregion
}
