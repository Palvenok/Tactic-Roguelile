using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] private GameObject gameUi;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private GameObject playerAttackSprite;
    [SerializeField] private GameObject aiAttackSprite;
    [SerializeField] private Text log;

    private bool _uiEnabled;
    private bool _tutorialEnabled = true;

    private void Start()
    {
        gameUi.SetActive(_uiEnabled);
        tutorialPanel.SetActive(_tutorialEnabled);
    }

    public void ChangeLog(string value)
    {
        log.text = value;
    }

    public void ChangeBattleUi(GroupType groupType, bool isEnabled)
    {
        playerAttackSprite.SetActive(false);
        aiAttackSprite.SetActive(false);

        if (!isEnabled) return;

        switch (groupType)
        {
            case GroupType.Player:
                playerAttackSprite.SetActive(true);
                break;
            case GroupType.AI:
                aiAttackSprite.SetActive(true);
                break;
            case GroupType.Unknown:
                playerAttackSprite.SetActive(true);
                aiAttackSprite.SetActive(true);
                break;
        }
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
