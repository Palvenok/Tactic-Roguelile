using UnityEngine;

public class UnitGroup : MonoBehaviour
{
    [SerializeField] private SpawnSettings spawnSettings = SpawnSettings.None;
    [Space, SerializeField] private int unitsCount = 3;
    [SerializeField] private Unit unitPrefab;
    [Space, SerializeField] private Unit[] unitsArray;
    [Header("Settings")]
    [SerializeField, Min(0)] private float xOffset = 10;

    private Unit[] _units;
    private Vector2[] _unitsPosition;
    private Vector2 _xOffset;

    public int UnitsCount => _units.Length;

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
        _units = new Unit[units.Length];
        _unitsPosition = new Vector2[units.Length];

        for (int i = 0; i < units.Length; i++)
        {
            _units[i] = Instantiate(units[i], transform);
            _units[i].ID = i;
            _unitsPosition[i] = _units[i].transform.localPosition = _xOffset += Vector2.left * xOffset * .1f;
        }
    }

    public Unit GetUnit(int index)
    {
        if (index < 0) index = 0;
        if (index >= _units.Length) index = _units.Length;
        return _units[index];
    }
}

public enum SpawnSettings
{
    None,
    SpawnByCount,
    SpawnFromArray
}
