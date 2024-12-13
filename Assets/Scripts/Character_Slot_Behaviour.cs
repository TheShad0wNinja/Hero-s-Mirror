using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Character_Slot_Behaviour : EventTrigger
{
    Training_Manager trainingManager;
    // Start is called before the first frame update
    void Start()
    {
        trainingManager = FindObjectOfType<Training_Manager>();
    }

    // Update is called once per frame
    public override void OnPointerClick(PointerEventData data)
    {
        trainingManager.SelectItemUI(this.gameObject);
    }
}
