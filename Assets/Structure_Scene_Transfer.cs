using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Structure_Scene_Transfer : MonoBehaviour
{
    [SerializeField] string sceneName;
    bool justEntered = true;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && justEntered)
        {
            Scene_Manager.Instance.ChangeScene(sceneName);
        }
        justEntered = false;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            justEntered = true;
        }
    }
}
