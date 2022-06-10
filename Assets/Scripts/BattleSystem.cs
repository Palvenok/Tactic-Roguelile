using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class BattleSystem : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnBattleStart;
    [HideInInspector] public UnityEvent OnBattleEnd;
    [HideInInspector] public UnityEvent<GroupType> OnRoundEnd;

    [SerializeField] private UnitGroup playerGroup;
    [SerializeField] private UnitGroup aiGroup;
    [SerializeField] private BattlePoint battlePoint;
    [SerializeField] private GameObject selectedUnitIndicatorPrefab;

    private GameObject _indicator;
    private UnitGroup _attackGroup;
    private UnitGroup _defenseGroup;
    private Unit _selectedAttackUnit;
    private Unit _selectedDefenseUnit;
    private Vector2 _unitNormalScale = Vector2.one;
    private Vector2[] _unitsOldPosition = new Vector2[2];
    private bool _attackUnitActionCompleted;
    private bool _defenseUnitActionCompleted;
    private bool _isInitialized;

    private BattleState _peviousState;
    private BattleState _currentState = BattleState.ChangeUnit;
    private int _index;

    public BattleState CurrentState { get => _currentState; set => _currentState = value; }
    public int Index { get => _index; set => _index = value; }

    public int PlauerGroupCount => playerGroup.UnitsCount;
    public int AiGroupCount => aiGroup.UnitsCount;

    private void Start()
    {
        playerGroup.OnGroupDead.AddListener(() => { OnRoundEnd?.Invoke(GroupType.AI); });
        aiGroup.OnGroupDead.AddListener(() => { OnRoundEnd?.Invoke(GroupType.Player); });
    }

    public void Initialize(GroupType groupType)
    {
        if (groupType == GroupType.Unknown) return;

        if (_indicator == null) _indicator = Instantiate(selectedUnitIndicatorPrefab);

        _currentState = BattleState.ChangeUnit;

        if(groupType == GroupType.Player)
        {
            _attackGroup = playerGroup;
            _defenseGroup = aiGroup;
            battlePoint.transform.localScale = new Vector2(1, 1);
        }
        else
        {
            _attackGroup = aiGroup;
            _defenseGroup = playerGroup;
            battlePoint.transform.localScale = new Vector2(-1, 1); 
        }

        _selectedAttackUnit = SelectUnit(_attackGroup, 0);
        _indicator.SetActive(true);

        _isInitialized = true;
    }

    public void Disable()
    {
        _isInitialized  = false;
        _indicator.SetActive(false);
        OnBattleEnd?.Invoke();
    }

    private void Update()
    {
        if (!_isInitialized) return;

        if (_currentState == BattleState.ChangeUnit)
        {
            _peviousState = _currentState;
            _selectedAttackUnit = SelectUnit(_attackGroup, _index);
        }
        if (_currentState == BattleState.ChangeEnemyUnit)
        {
            _peviousState = _currentState;
            _selectedDefenseUnit = SelectUnit(_defenseGroup, _index);
        }
        if (_currentState == BattleState.MoveToBattle)
        {
            _peviousState = _currentState;
            _currentState = BattleState.Delay;
            _indicator.SetActive(false);
            OnBattleStart?.Invoke();
            MoveToBattlePoint();
        }
        if (_currentState == BattleState.Battle)
        {
            _peviousState = _currentState;
            _currentState = BattleState.Delay;
            Battle();
        }
        if (_currentState == BattleState.MoveBack)
        {
            _peviousState = _currentState;
            _currentState = BattleState.Delay;
            MoveUnitsBack();
        }

        if(_currentState == BattleState.BattleEnd)
        {
            _currentState = BattleState.Delay;
            Disable();
        }

        if (UnitsSinc()) NextState(_peviousState);
    }

    public BattleState NextState(BattleState state)
    {
        _index = 0;
        state++;
        _currentState = state;
        return _currentState;
    }

    public void SkipStage()
    {
        _indicator.SetActive(false);
        _currentState = BattleState.Delay;
        Disable();
    }

    private bool UnitsSinc()
    {
        if (_attackUnitActionCompleted && _defenseUnitActionCompleted)
        {
            _attackUnitActionCompleted = _defenseUnitActionCompleted = false;
            return true;
        }
        else return false;
    }
    private void MoveToBattlePoint()
    {
        _unitsOldPosition[0] = _selectedAttackUnit.transform.position;
        _unitsOldPosition[1] = _selectedDefenseUnit.transform.position;
        MoveToPosition(battlePoint.PlayerPoint, battlePoint.AiPoint);
    }

    private void MoveUnitsBack()
    {
        var orientation = new Vector2(-1, 1);
        _selectedAttackUnit.transform.localScale = orientation;
        _selectedDefenseUnit.transform.localScale = orientation;

        MoveToPosition(_unitsOldPosition[0], _unitsOldPosition[1]);
    }

    private void MoveToPosition(Vector2 playerPoint, Vector2 aiPoint)
    {
        _selectedAttackUnit.OnActionComplete.AddListener(() =>
        {
            _attackUnitActionCompleted = true;
            _selectedAttackUnit.transform.localScale = _unitNormalScale;
            _selectedAttackUnit.OnActionComplete.RemoveAllListeners();
        });

        _selectedDefenseUnit.OnActionComplete.AddListener(() =>
        {
            _defenseUnitActionCompleted = true;
            _selectedDefenseUnit.transform.localScale = _unitNormalScale;
            _selectedDefenseUnit.OnActionComplete.RemoveAllListeners();
        });

        _selectedAttackUnit.MoveToPoint(playerPoint);
        _selectedDefenseUnit.MoveToPoint(aiPoint);
    }

    private void Battle()
    {
        _selectedAttackUnit.OnActionComplete.AddListener(() =>
        {
            _attackUnitActionCompleted = true;
            _selectedAttackUnit.OnActionComplete.RemoveAllListeners();
        });

        _selectedDefenseUnit.OnActionComplete.AddListener(() =>
        {
            _defenseUnitActionCompleted = true;
            _selectedDefenseUnit.OnActionComplete.RemoveAllListeners();
        });

        _selectedAttackUnit.Attack(_selectedDefenseUnit);
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

    private void OnDestroy()
    {
        OnBattleStart.RemoveAllListeners();
        OnBattleEnd.RemoveAllListeners();
    }
}

public enum BattleState
{
    ChangeUnit,
    ChangeEnemyUnit,
    MoveToBattle,
    Battle,
    MoveBack,
    BattleEnd,
    Delay
}
