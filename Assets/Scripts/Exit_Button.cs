using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Exit_Button : EventTrigger
{
    public override void OnPointerClick(PointerEventData data)
    {
        Scene_Manager.Instance.GoToHomebase();
    }
}
