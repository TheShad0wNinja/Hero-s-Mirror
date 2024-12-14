using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Structure_Scene_Transfer : MonoBehaviour
{
    [SerializeField] string sceneName;
    string triggerID; 
    private static HashSet<string> enteredTriggers = new();

    private void Awake()
    {
        triggerID = sceneName;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player") && !enteredTriggers.Contains(triggerID))
        {
            enteredTriggers.Add(triggerID);
            Scene_Manager.Instance.ChangeScene(sceneName);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Invoke("exitedTrigger", 0.1f);
        }
    }
    void exitedTrigger() 
    {
        enteredTriggers.Remove(triggerID);
    }
}
