using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using UI.Utils;

public class StartViewPresenter : MonoBehaviour
{
    private VisualElement _settingsView;
    private VisualElement _startView;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        _startView = root.Q("FrontPageMainMenu");
        _settingsView = root.Q("MainSettingsMenu");

        SetStartButtonsEvents();
    }

    private void SetStartButtonsEvents()
    {
        MainMenuPresenter menuPresenter = new MainMenuPresenter(_startView);
        menuPresenter.OpenSettings = () => ToggleSettingsMenu(true);

        MainMenuSettingsViewPresenter settingsPresenter = new MainMenuSettingsViewPresenter(_settingsView);
        settingsPresenter.BackAction = () => ToggleSettingsMenu(false);
    }

    private void ToggleSettingsMenu(bool enable)
    {
        _startView.Display(!enable);
        _settingsView.Display(enable);
    }
}
