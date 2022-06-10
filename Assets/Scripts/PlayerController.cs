using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private BattleSystem battleSystem;
    [SerializeField] private InputHandler inputHandler;

    private bool _isActive;

    public bool IsActive { get => _isActive; set => _isActive = value; }

    private void Awake()
    {
        inputHandler.OnKeyDown.AddListener(ProcessInput);
    }

    private void ProcessInput(KeyCode key)
    {
        if (!_isActive) return;

        if (key == Keys.leftKey)
        {
            if (battleSystem.CurrentState == BattleState.ChangeUnit) battleSystem.Index++;
            else battleSystem.Index--;
        }
        if (key == Keys.rightKey)
        {
            if (battleSystem.CurrentState == BattleState.ChangeEnemyUnit) battleSystem.Index++;
            else battleSystem.Index--;
        }

        if (key == Keys.attackKey) battleSystem.NextState(battleSystem.CurrentState, 0);
        if (key == Keys.skipKey) battleSystem.SkipStage();
    }
}
