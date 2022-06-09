using System.Collections;
using UnityEngine;

public class BaseUnit : Unit
{
    private AnimationController _animationController;
    private AnimationState _currentState = AnimationState.Idle;

    private void Awake()
    {
        _animationController = GetComponent<AnimationController>();
    }

    private void Start()
    {
        _animationController.SetAnimation(_currentState, true);
    }
    
    public override void Attack(Unit target)
    {
        _animationController.SetAnimation(AnimationState.Attack, false);
    }

    public override void TakeDamage(float damage)
    {
        _animationController.SetAnimation(AnimationState.Damage, false);
        Health -= damage;
    }
}
