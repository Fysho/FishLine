using System;
using UnityEngine;

// A simple cleanup that destroys GameObject when animator is done.
public class AnimatorCleanup : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        var stateInfo = animator.GetCurrentAnimatorStateInfo(0);
        
        if (stateInfo.normalizedTime > 1)
        {
            Destroy(gameObject);
        }
    }
}