using UnityEngine;

public class BattleController : MonoBehaviour
{
    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private AiController aiController;
    [Space]
    [SerializeField] private UIController uiController;

    private bool _isPlayerTurn;

    private void Start()
    {
        battleSystem.OnBattleStart.AddListener(OnBattleStart);
        battleSystem.OnBattleEnd.AddListener(OnBattleEnd);
        battleSystem.OnRoundEnd.AddListener(OnRoundEnd);

        int rand = Random.Range(0, 2);
        _isPlayerTurn = rand == 0;

        StartGame();
    }

    public void StartGame()
    {
        NextBattle();
    }

    private void NextBattle()
    {
        Debug.Log(_isPlayerTurn);
        if (_isPlayerTurn)
        {
            battleSystem.Initialize(GroupType.Player);
            playerController.IsActive = true;
        }
        else
        { 
            battleSystem.Initialize(GroupType.AI);
            aiController.Initialize(battleSystem.PlauerGroupCount, battleSystem.AiGroupCount);
        }
    }

    private void OnBattleStart()
    {
        playerController.IsActive = false;
    }

    private void OnBattleEnd()
    {
        _isPlayerTurn = !_isPlayerTurn;
        NextBattle();
    }

    private void OnRoundEnd(GroupType groupType)
    {
        playerController.IsActive = false;
    }
}
