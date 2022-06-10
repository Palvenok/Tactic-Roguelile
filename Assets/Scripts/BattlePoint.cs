using UnityEngine;

public class BattlePoint : MonoBehaviour
{
    [SerializeField] private Transform playerPoint;
    [SerializeField] private Transform aiPoint;

    public Vector2 PlayerPoint => playerPoint.position;
    public Vector2 AiPoint => aiPoint.position;
}
