using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    // Start is called before the first frame update
    Dialogue_Manager dialogueManager;
    [SerializeField] Conversation conversation;
    UI_Behaviour_Manager uiManager;

    private void Start()
    {
        dialogueManager = Dialogue_Manager.Instance;
        uiManager = UI_Behaviour_Manager.Instance;
        Dialogue_Manager.Instance.onDialogueEnd += dialougueEnded;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
                dialogueManager.InitializeConversation(conversation,this.gameObject);
                Debug.Log("convo1");
            
            
        }
    }
    void dialougueEnded(GameObject user)
    {
        if (gameObject == user)
        {
            print("dialogue ended ");
        }
    }
}
