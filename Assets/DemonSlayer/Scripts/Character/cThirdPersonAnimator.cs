using UnityEngine;

public class cThirdPersonAnimator : MonoBehaviour
{
    protected Animator animator;

    protected virtual void Start()
    {
        animator = GetComponent<Animator>();
    }

    protected virtual void UpdateAnimator(float h, float v, float magnitude)
    {
        if (!animator) return;

        animator.SetFloat("InputHorizontal", h, 0.1f, Time.deltaTime);
        animator.SetFloat("InputVertical", v, 0.1f, Time.deltaTime);
        animator.SetFloat("InputMagnitude", magnitude, 0.1f, Time.deltaTime);
    }
}
