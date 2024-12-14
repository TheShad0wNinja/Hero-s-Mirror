using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Clickable : MonoBehaviour
{
    public string locationName;
    public string sceneName;
    private void OnMouseDown()
    {
        FindObjectOfType<Drag_Camera>().TransferRevision(this.transform, locationName);
        FindObjectOfType<Map_Manager>().setSceneName(sceneName);
    }
}
