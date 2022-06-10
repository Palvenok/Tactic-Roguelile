using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject gameUi;
    [SerializeField] private GameObject tutorialPanel;

    private bool _uiEnabled;
    private bool _tutorialEnabled = true;

    private void Start()
    {
        gameUi.SetActive(_uiEnabled);
        tutorialPanel.SetActive(_tutorialEnabled);
    }

    public void ChangeUiState()
    {
        _uiEnabled = !_uiEnabled;
        gameUi.SetActive(_uiEnabled);
    }

    public void ChangeTutorialState()
    {
        _tutorialEnabled = !_tutorialEnabled;
        tutorialPanel.SetActive(_tutorialEnabled);
    }
}
