using System;
using System.Collections;
using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private UnitGroup playerGroup;
    [SerializeField] private UnitGroup aiGroup;
    [SerializeField] private BattlePoint battlePoint;
    [SerializeField] private GameObject selectedUnitIndicatorPrefab;
    [SerializeField] private InputHandler inputHandler;

    private GameObject _indicator;
    private BattleState _currentState = BattleState.ChangePlayerUnit;
    private Unit _selectedPlayerUnit;
    private Unit _selectedAIUnit;
    private int _index;

    private IEnumerator Start()
    {
        _indicator = Instantiate(selectedUnitIndicatorPrefab);
        _indicator.SetActive(false);
        inputHandler.OnKeyDown.AddListener(ProcessInput);

        yield return new WaitForFixedUpdate();
        _selectedPlayerUnit = SelectUnit(playerGroup, 0);
    }

    private void Update()
    {
        if (_currentState == BattleState.ChangePlayerUnit)
        {
            _selectedPlayerUnit = SelectUnit(playerGroup, _index);
        }
        if (_currentState == BattleState.ChangeEnemyUnit)
        {
            _selectedAIUnit = SelectUnit(aiGroup, _index);
        }
        if (_currentState == BattleState.Battle)
        {
            _currentState = BattleState.Delay;
            _indicator.SetActive(false);
            Battle(_selectedPlayerUnit, _selectedAIUnit);
        }
    }

    private void ProcessInput(KeyCode key)
    {
        if (key == Keys.leftKey)
        {
            if (_currentState == BattleState.ChangePlayerUnit) _index++;
            else _index--;
        }
        if (key == Keys.rightKey)
        {
            if (_currentState == BattleState.ChangeEnemyUnit) _index++;
            else _index--;
        }

        if (_currentState == BattleState.Battle || _currentState == BattleState.AiTurn) return;

        if (key == Keys.attackKey) NextState();
        if (key == Keys.skipKey) SkipStage();
    }

    private void NextState()
    {
        _index = 0;
        _currentState++;
    }

    private void SkipStage()
    {
        _currentState = BattleState.AiTurn;
    }

    public void Battle(Unit attackUnit, Unit deffenseUnit)
    {
        attackUnit.OnMoveComplite.AddListener(() => 
        {
            attackUnit.Attack(deffenseUnit);
        });

        deffenseUnit.OnMoveComplite.AddListener(() =>
        {
            deffenseUnit.TakeDamage(attackUnit.Damage);
        });

        attackUnit.MoveToPoint(battlePoint.PlayerPoint);
        deffenseUnit.MoveToPoint(battlePoint.AiPoint);
    }

    public Unit SelectUnit(UnitGroup group, int index)
    {
        if(group.UnitsCount == 0) return null;

        if(index >= group.UnitsCount) _index = group.UnitsCount - 1;
        if(index < 0) _index = 0;

        _indicator.transform.position = group.UnitPosition(_index);
        _indicator.SetActive(true);
        return group.GetUnit(_index);
    }
}

public enum BattleState
{
    ChangePlayerUnit,
    ChangeEnemyUnit,
    Battle,
    AiTurn,
    Delay
}
