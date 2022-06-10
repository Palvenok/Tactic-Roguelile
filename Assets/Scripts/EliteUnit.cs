using DG.Tweening;
using UnityEngine;

public class EliteUnit : Unit
{
    private SpineAnimationController _animationController;
    private AnimationState _currentState = AnimationState.Idle;

    private void Awake()
    {
        _animationController = GetComponent<SpineAnimationController>();
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
        Health -= damage;
        _animationController.SetAnimation(AnimationState.Damage, false);
        if (Health <= 0)
        {
            OnDeath?.Invoke(this);
            Destroy(gameObject);
        }
    }

    public override void MoveToPoint(Vector2 point)
    {
        _animationController.SetAnimation(AnimationState.Pull, false, .3f);
        transform.DOMove(point, 1).OnComplete(() =>
        {
            _animationController.SetAnimation(AnimationState.Idle, true);
            OnMoveComplite?.Invoke();
        });
    }
}
