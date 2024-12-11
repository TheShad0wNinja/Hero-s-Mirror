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

    public static IEnumerator MoveObject(Vector2 origin, Vector2 destination, Transform objectTransform, float movementDuration)
    {
        float currentMovementTime = 0f;
        while (Vector2.Distance(objectTransform.position, destination) > 0.01)
        {
            Debug.Log($"From {objectTransform.position} to {destination}");
            currentMovementTime += Time.deltaTime;
            objectTransform.localPosition = Vector3.Lerp(origin, destination, currentMovementTime / movementDuration);
            yield return null;
        }
        Debug.Log("Movement: End");
    }
}