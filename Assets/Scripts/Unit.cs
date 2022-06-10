using UnityEngine;
using UnityEngine.Events;

public abstract class Unit : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnMoveComplite;
    [HideInInspector] public UnityEvent<Unit> OnDeath;

    [SerializeField] private float _health = 15;
    [SerializeField] private float _damage = 8;

    private GroupType _groupType;

    public float Health { get => _health; set => _health = value; }
    public float Damage => _damage;
    public GroupType GroupType { get => _groupType; set => _groupType = value; }

    public abstract void Attack(Unit target);
    public abstract void TakeDamage(float damage);
    public abstract void MoveToPoint(Vector2 point);

    private void OnDestroy()
    {
        OnMoveComplite.RemoveAllListeners();
        OnDeath.RemoveAllListeners();
    }
}
