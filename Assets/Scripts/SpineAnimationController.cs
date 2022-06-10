using UnityEngine;
using Spine.Unity;

public class SpineAnimationController : MonoBehaviour
{
    [SerializeField] private AnimationAsset[] animationAssets;

    private SkeletonAnimation skeletonAnimation;

    private void Awake()
    {
        skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    public void SetAnimation(AnimationState state, bool loop)
    {
        foreach (var animationAsset in animationAssets)
        {
            if (animationAsset.State == state)
                skeletonAnimation.state.SetAnimation(0, animationAsset.Asset, loop);
        }
    }

    [System.Serializable]
    private class AnimationAsset
    {
        public AnimationReferenceAsset Asset;
        public AnimationState State;
    }
}

public enum AnimationState
{
    Unknown,
    Idle,
    Pull,
    Damage,
    Attack
}