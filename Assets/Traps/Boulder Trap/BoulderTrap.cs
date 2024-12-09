using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoulderTrap : MonoBehaviour

{
    [SerializeField] private GameObject boulder; // Reference to the boulder
    [SerializeField] private Transform targetPosition; // Position of the pressure plate where the boulder will roll to
    [SerializeField] private float speed = 2f;
    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
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
