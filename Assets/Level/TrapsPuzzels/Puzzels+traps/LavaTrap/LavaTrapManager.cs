using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaTrapManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] LavaDisappearingRocks;
    public float visibleTime = 2f;
    public float invisibleTime = 2f;
    void Start()
    {
        foreach (GameObject plateform in LavaDisappearingRocks)
        {
            StartCoroutine(TogglePlateform(plateform));
        }
    }

    private IEnumerator TogglePlateform(GameObject plateform)
    {
        while (true)
        {
            SetPlatformState(plateform, true);
            yield return new WaitForSeconds(visibleTime);

            SetPlatformState(plateform, false);
            yield return new WaitForSeconds(invisibleTime);
        }

    }
    private void SetPlatformState(GameObject plateform, bool isVisible)
    {
        Renderer renderer = plateform.GetComponent<Renderer>();
        Collider2D collider2D = plateform.GetComponent<Collider2D>();
        if (renderer) renderer.enabled = isVisible;
        if (collider2D) collider2D.enabled = isVisible;

    }

    // Update is called once per frame

}
