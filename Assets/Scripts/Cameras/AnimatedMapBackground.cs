using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatedMapBackground : MonoBehaviour
{
    public const float ANIMATION_LENTH = 500.19f;
    public const string animationStateName = "animation";
    [SerializeField] Animator animator;
    [SerializeField] Transform mapSprite;
    void Start()
    {
        mapSprite.position = new Vector3(0, -1513,0);
        UnityEngine.Random.InitState(System.DateTime.Now.Millisecond);
        float randomTime = UnityEngine.Random.RandomRange(0.0f, ANIMATION_LENTH);
        animator.Play(animationStateName, 0, randomTime / ANIMATION_LENTH);
    }

    // Update is called once per frame
}
