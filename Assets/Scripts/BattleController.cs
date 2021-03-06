using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour
{
    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AiController aiController;
    [Space]
    [SerializeField] private UIController uiController;

    private bool _isBattleActive;
    private bool _isPlayerTurn;

    private void Start()
    {
        battleSystem.OnBattleStart.AddListener(OnBattleStart);
        battleSystem.OnBattleEnd.AddListener(OnBattleEnd);
        battleSystem.OnRoundEnd.AddListener(OnRoundEnd);
        battleSystem.OnBattleSkip.AddListener(OnBattleSkip);

        int rand = Random.Range(0, 2);
        _isPlayerTurn = rand == 0;
    }

    public void StartGame()
    {
        _isBattleActive = true;
        NextBattle();

        var type = _isPlayerTurn ? GroupType.Player : GroupType.AI;
        uiController.ChangeBattleUi(type, true);
        uiController.ChangeLog("Attack: " + type);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void NextBattle()
    {
        if (!_isBattleActive) return;

        if (_isPlayerTurn)
        {
            battleSystem.Initialize(GroupType.Player);
            playerController.IsActive = true;
        }
        else
        { 
            battleSystem.Initialize(GroupType.AI);
            aiController.Initialize(battleSystem.PlayerGroupCount, battleSystem.AiGroupGroupCount);
        }
    }

    private void OnBattleStart()
    {
        playerController.IsActive = false;
        uiController.ChangeBattleUi(GroupType.Unknown, true);
        uiController.ChangeLog("Battle!");
    }

    private void OnBattleEnd()
    {
        if (!_isBattleActive) return;

        playerController.IsActive = false;
        _isPlayerTurn = !_isPlayerTurn;
        NextBattle();

        var type = _isPlayerTurn ? GroupType.Player : GroupType.AI;
        uiController.ChangeBattleUi(type, true);
        uiController.ChangeLog("Attack: " + type);
    }

    private void OnBattleSkip()
    {
        Debug.Log("Battle Skiped");
    }

    private void OnRoundEnd(GroupType groupType)
    {
        _isBattleActive = false;
        playerController.IsActive = false;

        uiController.ChangeBattleUi(GroupType.Unknown, false);
        uiController.ChangeLog("Win: " + groupType + "!");
    }
}
