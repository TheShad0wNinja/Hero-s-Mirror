using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnimation : MonoBehaviour
{
    // Start is called before the first frame update
    public Image fadeImage; // Reference to the FadeOverlay image.
    public float fadeDuration = 1.0f; // Time in seconds for the fade.
    public bool fadein = false;


    private void Start()
    {

        if (fadeImage == null)
            fadeImage = GetComponent<Image>(); // Auto-assign if not set.
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(1, 0)); // From black to transparent.
        fadein = true;

    }

    public void FadeOut()
    {
        StartCoroutine(Fade(0, 1)); // From transparent to black.
    }

    private IEnumerator Fade(float startAlpha, float endAlpha)
    {
        fadeImage.enabled = true;

        float elapsedTime = 0f;
        Color color = fadeImage.color;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, endAlpha, elapsedTime / fadeDuration);
            fadeImage.color = new Color(color.r, color.g, color.b, alpha);
            yield return null;
        }

        // Ensure exact final value
        fadeImage.color = new Color(color.r, color.g, color.b, endAlpha);
        if (fadein)
        {
            fadeImage.enabled = false;

        }

    }
}
