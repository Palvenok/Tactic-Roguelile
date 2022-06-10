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

    public void SetAnimation(AnimationState state, bool loop, float timeScale = 1f)
    {
        foreach (var animationAsset in animationAssets)
        {
            if (animationAsset.State == state)
            {
                skeletonAnimation.state.SetAnimation(0, animationAsset.Asset, loop).TimeScale = timeScale;
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