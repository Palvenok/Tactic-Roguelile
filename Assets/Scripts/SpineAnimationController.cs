using UnityEngine;
using Spine.Unity;

public class SpineAnimationController : MonoBehaviour
{
    [SerializeField] private AnimationAsset[] animationAssets;

    private SkeletonAnimation _skeletonAnimation;

    public SkeletonAnimation SkeletonAnimation => _skeletonAnimation;

    private void Awake()
    {
        _skeletonAnimation = GetComponent<SkeletonAnimation>();
    }

    public void SetAnimation(AnimationState state, bool loop, float timeScale = 1f)
    {
        foreach (var animationAsset in animationAssets)
        {
            if (animationAsset.State == state)
            {
                _skeletonAnimation.state.SetAnimation(0, animationAsset.Asset, loop).TimeScale = timeScale;
                return;
            }
        }
        Debug.Log("Unassiggned animation");
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