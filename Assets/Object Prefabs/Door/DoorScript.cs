using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class DoorScript : MonoBehaviour
{
    [SerializeField] Sprite openDoor;
    SpriteRenderer spriteRenderer;
    public bool canOpenKey = false;
    public bool canOpenPuzzle = false;
    public string sceneName ="";
    Audio_Manager audioManager;
    AudioClip audioClip;

    [SerializeField] private GameObject textBubblePrefab;
    private GameObject textBubbleInstance;

    void Start()
    {
        audioManager = FindObjectOfType<Audio_Manager>();
        audioClip = Resources.Load<AudioClip>("DoorOpen");
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void UnlockDoorKey() 
    {
        canOpenKey = true;
    }
    public void UnlockDoorPuzzle ()
    {
        canOpenPuzzle = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") ) 
        {
            if (canOpenKey && canOpenPuzzle)
            {
                audioManager.PlaySFX(audioClip);
                spriteRenderer.sprite = openDoor;
                GetComponent<BoxCollider2D>().enabled = false;
                if (sceneName == "")
                {
                    UI_Behaviour_Manager.Instance.count++;
                    Scene_Manager.Instance.GoToHomebase();
                }
                else 
                {
                    Scene_Manager.Instance.ChangeScene(sceneName);
                }
            }
            else if (canOpenKey && !canOpenPuzzle)
            {
                InstantiateBubble("Look for a rubix cube buddy");
            }
            else if (canOpenPuzzle && !canOpenKey)
            {
                InstantiateBubble("I have 2 locks, sorry");
            }
            else 
            {
                InstantiateBubble("Hey");
            }
        }
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player")) 
        {
            Destroy(textBubbleInstance);
        }
    }
    void InstantiateBubble(string input) 
    {
        if(textBubbleInstance != null)
        {
            Destroy(textBubbleInstance);
        }
        textBubbleInstance = Instantiate(textBubblePrefab, transform.position + Vector3.up, Quaternion.identity);
        textBubbleInstance.GetComponentInChildren<TextMeshPro>().text = input;
        textBubbleInstance.transform.SetParent(transform);
    }
}
