using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private UnitGroup leftGroup;
    [SerializeField] private UnitGroup rightGroup;

    [ContextMenu("Fight")]
    public void Fight()
    {
        leftGroup.GetUnit(0).Attack(rightGroup.GetUnit(0));
    }
}
