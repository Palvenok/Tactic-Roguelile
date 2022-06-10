using UnityEngine;

public abstract class Unit : MonoBehaviour
{
    [SerializeField] private float _health = 15;
    [SerializeField] private float _damage = 8;

    private int _id = 0;

    public float Health { get => _health; set => _health = value; }
    public float Damage => _damage;
    public int ID { get => _id; set => _id = value; }

    public abstract void Attack(Unit target);
    public abstract void TakeDamage(float damage);
}
