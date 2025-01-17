using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressureScript : MonoBehaviour

{
    [SerializeField] private GameObject boulder; // Reference to the boulder
    [SerializeField] private Transform targetPosition; // Position of the pressure plate where the boulder will roll to
    [SerializeField] private float speed = 2f;
    private bool isActivated = false;
    Audio_Manager audioManager;
    AudioClip audioClip;

    private void Start()
    {
        audioManager = FindObjectOfType<Audio_Manager>();
        audioClip = Resources.Load<AudioClip>("BoulderTrap");
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            audioManager.PlaySFX(audioClip);
            isActivated = true;
            StartCoroutine(MoveBoulder());
        }
    }

    private IEnumerator MoveBoulder()
    {
        while (boulder != null && Vector2.Distance(boulder.transform.position, targetPosition.position) > 0.1f)
        {
            boulder.transform.position = Vector2.MoveTowards(boulder.transform.position, targetPosition.position, speed * Time.deltaTime);
            yield return null;
        }

        if (boulder != null)
        {
            Destroy(boulder);
        }

        isActivated = false;
    }
}
