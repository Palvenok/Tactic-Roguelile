using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnitGroup : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnGroupDead;

    [SerializeField] private GroupType groupType = GroupType.Unknown;
    [SerializeField] private SpawnSettings spawnSettings = SpawnSettings.None;
    [Space, SerializeField] private int unitsCount = 3;
    [SerializeField] private Unit unitPrefab;
    [Space, SerializeField] private Unit[] unitsArray;
    [Header("Settings")]
    [SerializeField, Min(0)] private float xOffset = 10;

    private List<Unit> _units = new List<Unit>();
    private Vector2[] _unitsPosition;
    private Vector2 _xOffset;

    public int UnitsCount => _units.Count;
    public Vector2[] UnitsPositions => _unitsPosition;

    private void Start()
    {
        switch (spawnSettings)
        {
            case SpawnSettings.None:
                Debug.Log("Units in group spawned by extern command");
                break;
            case SpawnSettings.SpawnByCount:
                SpawnUnits(unitsCount);
                break;
            case SpawnSettings.SpawnFromArray:
                SpawnUnitsFromArray(unitsArray);
                break;
        }        
    }

    public void SpawnUnits(int count)
    {
        Unit[] units = new Unit[count];
        for (int i = 0; i < count; i++)
        {
            units[i] = unitPrefab;
        }
        SpawnUnitsFromArray(units);
    }

    public void SpawnUnitsFromArray(Unit[] units)
    {
        _unitsPosition = new Vector2[units.Length];

        for (int i = 0; i < units.Length; i++)
        {
            _units.Add(Instantiate(units[i], transform));
            _units[i].OnDeath.AddListener(RemoveUnit);
            _units[i].GroupType = groupType;
            _units[i].transform.localPosition = _xOffset += Vector2.left * xOffset * .1f;
            _unitsPosition[i] = _units[i].transform.position;
        }
    }

    public Unit GetUnit(int index)
    {
        if (index < 0) index = 0;
        if (index >= _units.Count) index = _units.Count;
        return _units[index];
    }

    public void RemoveUnit(Unit unit)
    {
        if(_units.Contains(unit))
        {
            _units.Remove(unit);
        }
        UpdateGroup();
    }

    private void UpdateGroup()
    {
        for (int i = 0; i < _units.Count; i++)
        {
            _units[i].MoveToPoint(_unitsPosition[i]);
        }

        if (_units.Count == 0) OnGroupDead?.Invoke();
    }

    public Vector2 UnitPosition(int index)
    {
        return _unitsPosition[index];
    }

    private void OnDestroy()
    {
        OnGroupDead.RemoveAllListeners();
    }
}

public enum SpawnSettings
{
    None,
    SpawnByCount,
    SpawnFromArray
}

public enum GroupType
{
    Unknown,
    Player,
    AI
}
