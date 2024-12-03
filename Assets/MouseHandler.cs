using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    public CombatUIChannel uiChannel;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics2D.GetRayIntersectionAll(ray, 1500f);
            foreach(var hit in hits)
            {
                if (hit.collider.CompareTag("Unit"))
                {
                    uiChannel.RaiseOnUnitSelect(hit.collider.GetComponent<Character>());
                }
            }
        }
    }
}
