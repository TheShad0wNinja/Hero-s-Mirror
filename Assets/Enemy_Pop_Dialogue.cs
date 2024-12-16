using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CombatBackgroundTypes
{
    FOREST,
    DUNGEON,
    HOUSE
}
public class Enemy_Pop_Dialogue : MonoBehaviour
{
    bool oneTime = false;
    [SerializeField] Conversation conversation1;
    [SerializeField] Conversation conversation2;
    [SerializeField] ParticleSystem deathParticles;
    bool switchConvo = false;
    public int gold;
    public List<UnitSO> enemies;
    public CombatBackgroundTypes combatBackgroundType = CombatBackgroundTypes.FOREST;
    bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!oneTime && collision.CompareTag("Player"))
        {
            oneTime = true;
            Dialogue_Manager.Instance.InitializeConversation(conversation1, gameObject);
        }
    }
    void initiateCombat()
    {
        CombatEnemyManager.Instance.AssignEnemies(enemies);
        CombatEnemyManager.Instance.combatBackgroundType = combatBackgroundType;
        Scene_Manager.Instance.ChangeSceneAdditiveRemoveLight("Combat");
    }
    private void Start()
    {
        Dialogue_Manager.Instance.onDialogueEnd += dialougueEnded;
        CombatEnemyManager.Instance.OnCombatEnd += dialougueEnded2;
    }
    void dialougueEnded(GameObject user)
    {
        if (gameObject == user)
        {
            if (switchConvo == false)
            {
                initiateCombat();
                hasTriggered = true;
                this.GetComponent<CapsuleCollider2D>().enabled = false;
            }
            else
            {
                Death();
            }
        }
    }
    void dialougueEnded2(bool s) 
    {
        if (s && hasTriggered)
        {
            print("end1");
            CombatEnemyManager.Instance.OnCombatEnd -= dialougueEnded2;
            switchConvo = true;
            Invoke("CombatWon",0.5f);
        }
    }
    void CombatWon()
        {
            Dialogue_Manager.Instance.InitializeConversation(conversation2, gameObject);
            switchConvo = true;
        }
        void Death()
        {
            print("die");
            Instantiate(deathParticles, transform.position, Quaternion.identity);
            Reward();
            Dialogue_Manager.Instance.onDialogueEnd -= dialougueEnded;
        Destroy(this.gameObject);
        }
        void Reward()
        {
            UI_Behaviour_Manager.Instance.gold += gold;
            Load_UI.Instance.levelPanel.GetComponent<Level_UI_Manager>().UpdateUI();
        }
    }