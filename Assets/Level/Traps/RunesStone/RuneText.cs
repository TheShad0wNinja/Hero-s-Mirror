using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RuneText : MonoBehaviour
{
    [SerializeField] private GameObject textBubblePrefab;
    private GameObject textBubbleInstance;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered trigger");
            textBubbleInstance = Instantiate(textBubblePrefab, transform.position + Vector3.up, Quaternion.identity);
            textBubbleInstance.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(textBubbleInstance);
        }
    }
}
