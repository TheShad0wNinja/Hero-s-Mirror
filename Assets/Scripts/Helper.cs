using System.Collections;
using UnityEngine;

internal static class Helper
{
    public static IEnumerator WaitForAnimation(Animator animator, int baseLayer, string animationName)
    {
        Debug.Log("Animation: Init");
        animator.Play(animationName);

        // Wait for current animation to end
        while (!animator.GetCurrentAnimatorStateInfo(baseLayer).IsName(animationName))
            yield return new WaitForEndOfFrame();

        Debug.Log("Animation: Start");

        // Wait for selected animation to end
        while (animator.GetCurrentAnimatorStateInfo(baseLayer).IsName(animationName) &&
                animator.GetCurrentAnimatorStateInfo(baseLayer).normalizedTime < 1.0f)
            yield return new WaitForEndOfFrame();

        Debug.Log("Animation: End");

        yield return null;
    }
}