using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    public MouseChannel channel;
    RaycastHit2D[] hits;
    bool isHovered = false;
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            foreach(var hit in hits)
            {
                if (hit.collider.CompareTag("Unit"))
                {
                    channel.RaiseOnUnitSelect(hit.collider.GetComponent<Unit>());
                }
            }
        } else {
            Unit character = null;
            foreach(var hit in hits)
            {
                if (hit.collider.CompareTag("Unit"))
                {
                    character = hit.collider.GetComponent<Unit>();
                }
            }

            if (character == null && isHovered)
            {
                channel.RaiseOnUnitUnhover(character);
                isHovered = false;
            } else if (character != null && !isHovered)
            {
                channel.RaiseOnUnitHover(character);
                isHovered = true;
            }

        }
    }

    void FixedUpdate()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        hits = Physics2D.GetRayIntersectionAll(ray, 1500f);
    }
}
