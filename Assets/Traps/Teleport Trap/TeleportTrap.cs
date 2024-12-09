using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportTrap : MonoBehaviour
{
[SerializeField] private Transform teleportLocation;
private bool isTriggered;
private GameObject playerOnFloor = null;


private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            isTriggered = true;
            StartCoroutine(TeleportPlayer(other.gameObject));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.gameObject == playerOnFloor)
        {
            isTriggered = false;
            playerOnFloor = null;
            StopAllCoroutines();
        }
    }

    private IEnumerator TeleportPlayer(GameObject player)
    {
        yield return new WaitForSeconds(0.5f);
        
        if (player != null && teleportLocation != null)
        {
            player.transform.position = teleportLocation.position;
        }

        isTriggered = false;
    }


}
