using UnityEngine;

public class BattleSystem : MonoBehaviour
{
    [SerializeField] private UnitGroup playerGroup;
    [SerializeField] private UnitGroup aiGroup;
    [SerializeField] private bool isPlayerTurn;


    public void Fight(int playerUnitId, int aiUnitId)
    {
        playerGroup.GetUnit(0).Attack(aiGroup.GetUnit(0));
    }
}
