using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Next_Level_Door : MonoBehaviour
{
    [SerializeField] Sprite openDoor;
    SpriteRenderer spriteRenderer;
    public string sceneName = "";
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    // Update is called once per frame
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            if (sceneName == "")
            {

            }
            spriteRenderer.sprite = openDoor;
            GetComponent<BoxCollider2D>().enabled = false;
            Scene_Manager.Instance.ChangeScene(sceneName);
        }
    }
}
