using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuddenDialogue : MonoBehaviour
{
    [SerializeField] private Conversation conversation;
    private Dialogue_Manager dialogueManager;

    private void Awake()
    {
        dialogueManager = FindObjectOfType<Dialogue_Manager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialogueManager.InitializeConversation(conversation, this.gameObject);
        }
    }
}
