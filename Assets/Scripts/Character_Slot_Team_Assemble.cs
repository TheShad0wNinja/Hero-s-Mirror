using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character_Slot_Team_Assemble : EventTrigger
{
    Map_Manager mapManager;
    // Start is called before the first frame update
    void Start()
    {
        mapManager = FindObjectOfType<Map_Manager>();
    }

    // Update is called once per frame
    public override void OnPointerClick(PointerEventData data)
    {
        mapManager.SelectItemUI(this.gameObject);
    }
}
