using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map_Clickable : MonoBehaviour
{
    public string locationName;
    private void OnMouseDown()
    {
        FindObjectOfType<Drag_Camera>().TransferRevision(this.transform, locationName);
    }
}
