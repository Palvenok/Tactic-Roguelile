using System.Collections;
using UnityEngine;
using DG.Tweening;
using Spine.Unity;
using System;

public class BaseUnit : Unit
{
    private SpineAnimationController _animationController;
    private AnimationState _currentState = AnimationState.Idle;
    private SkeletonAnimation _animationSkeleton;
    private Unit _target;

    private void Awake()
    {
        _animationController = GetComponent<SpineAnimationController>();
    }

    private void Start()
    {
        _animationController.SetAnimation(_currentState, true);
        _animationSkeleton = _animationController.SkeletonAnimation;
    }
    
    public override void Attack(Unit target)
    {
        _target = target;
        _animationSkeleton.state.Event += OnEvent;
        StartCoroutine(AttackCoroutine());
    }

    private void OnEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data.Name == "Hit") _target.TakeDamage(Damage);
    }

    private IEnumerator AttackCoroutine()
    {
        _animationController.SetAnimation(AnimationState.Attack, false);
        yield return new WaitForSeconds(1.2f);
        _animationSkeleton.state.Event -= OnEvent;
        _target = null;
        OnActionComplete?.Invoke();
    }

    public override void TakeDamage(float damage)
    {
        Health -= damage;
        _animationController.SetAnimation(AnimationState.Damage, false);
        if (Health <= 0) Death();
        OnActionComplete?.Invoke();
    }

    private void Death()
    {
        OnDeath?.Invoke(this);
        gameObject.SetActive(false);
    }

    public override void MoveToPoint(Vector2 point)
    {
        _animationController.SetAnimation(AnimationState.Pull, false, .3f);
        transform.DOMove(point, 1).OnComplete(() =>
        {
            _animationController.SetAnimation(AnimationState.Idle, true);
            OnActionComplete?.Invoke();
        });
    }
}
