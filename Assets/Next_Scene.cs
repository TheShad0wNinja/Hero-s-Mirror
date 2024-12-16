using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Next_Scene : MonoBehaviour
{
    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (sceneName ==  "")
        {
            UI_Behaviour_Manager.Instance.count++;
            Scene_Manager.Instance.GoToHomebaseOrigin();
        }
        else
        Scene_Manager.Instance.ChangeScene(sceneName);
    }
}