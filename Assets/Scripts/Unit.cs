using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Unit : MonoBehaviour
{
    [HideInInspector] public UnityEvent OnActionComplete;
    [HideInInspector] public UnityEvent<Unit> OnDeath;

    [SerializeField] private float _maxHealth = 15;
    [SerializeField] private float _health = 15;
    [SerializeField] private float _damage = 8;
    [SerializeField] private Slider[] _healthBar;

    private GroupType _groupType;

    public float Health 
    { 
        get => _health; 
        set 
        { 
            _health = value;
            foreach (var item in _healthBar)
                item.value = Utils.Map(_health, 0, _maxHealth, 0, 1);
        } 
    }

    public float Damage => _damage;
    public GroupType GroupType { get => _groupType; set => _groupType = value; }

    public abstract void Attack(Unit target);
    public abstract void TakeDamage(float damage);
    public abstract void MoveToPoint(Vector2 point);

    private void OnDestroy()
    {
        OnActionComplete.RemoveAllListeners();
        OnDeath.RemoveAllListeners();
    }
}
