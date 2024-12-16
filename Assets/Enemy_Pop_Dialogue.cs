using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Pop_Dialogue : MonoBehaviour
{
    bool oneTime = false;
    [SerializeField] Conversation conversation1;
    [SerializeField] Conversation conversation2;
    [SerializeField] ParticleSystem deathParticles;
    bool switchConvo = false;
    public int xp, gold;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!oneTime) 
        {
            oneTime = true;
           Dialogue_Manager.Instance.InitializeConversation(conversation1,gameObject);
        }
    }
    private void Start()
    {
        Dialogue_Manager.Instance.onDialogueEnd += dialougueEnded;
    }
    void dialougueEnded(GameObject user) 
    {
        // start combat
        print("dialogue ended");
        print("is user : " + (gameObject == user));
        if (gameObject == user) 
        {
            if (!switchConvo) CombatWon();
            else
            {
                Death();
                print("Death Event");
            } 
        }

    }
    void CombatWon() 
    {
        Dialogue_Manager.Instance.InitializeConversation(conversation2, gameObject);
        switchConvo = true;
        print("combatwon" + switchConvo);
    }
    void Death() 
    {
        print("die");
        Instantiate(deathParticles, transform.position, Quaternion.identity);
        Reward();
        Dialogue_Manager.Instance.onDialogueEnd -= dialougueEnded;
        Destroy(gameObject);
    }
    void Reward() 
    {
        UI_Behaviour_Manager.Instance.gold += gold;
        Load_UI.Instance.levelPanel.GetComponent<Level_UI_Manager>().UpdateUI();
        foreach (var element in UI_Behaviour_Manager.Instance.teamAssembleCharacters) 
        {
            element.xp += xp;
        }
    }
}
