using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChestScript : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    [SerializeField] Sprite spriter;
    [SerializeField] Item_Stats item;
    [SerializeField] int gold;
    [SerializeField] private GameObject textBubblePrefab;
    private GameObject textBubbleInstance;
    bool isRewardGiven = false;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isRewardGiven)
        {
            spriteRenderer.sprite = spriter;
            GiveReward();
            textBubbleInstance = Instantiate(textBubblePrefab, transform.position + Vector3.up, Quaternion.identity);
            textBubbleInstance.GetComponentInChildren<TextMeshPro>().text = "+" + gold;
            textBubbleInstance.transform.SetParent(transform);
            isRewardGiven = true;
        }
    }


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(textBubbleInstance);
        }
    }

    // private void GiveReward()
    // {
    //     int random = Random.Range(0, 2);
    //     if (random == 0)
    //     {
    //         UI_Behaviour_Manager.Instance.AddItemByStat(item);
    //     }
    //     else
    //     {
    //         UI_Behaviour_Manager.Instance.gold += gold;
    //     }
    // }

    private void GiveReward()
    {
        UI_Behaviour_Manager.Instance.gold += gold;
    }
}
