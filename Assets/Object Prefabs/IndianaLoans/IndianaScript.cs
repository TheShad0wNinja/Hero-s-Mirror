using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndianaScript : MonoBehaviour
{
    Dialogue_Manager dialogueManager;
    [SerializeField] Conversation conversationBefore;
    [SerializeField] Conversation conversationAfter;
    UI_Behaviour_Manager uiManager;
    private int counter = 0;

    private void Start()
    {
        dialogueManager = Dialogue_Manager.Instance;
        uiManager = UI_Behaviour_Manager.Instance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (counter == 0)
            {
                dialogueManager.InitializeConversation(conversationBefore);
                counter++;
            }
            else
            {
                uiManager.gold -= 100;
                FindObjectOfType<Level_UI_Manager>().UpdateUI();
                dialogueManager.InitializeConversation(conversationAfter);
            }
        }
    }
}
