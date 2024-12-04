using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "MouseChannel")]
public class MouseChannel : ScriptableObject
{
    public UnityAction<Unit> OnUnitSelect;
    public UnityAction<Unit> OnUnitHover;
    public UnityAction<Unit> OnUnitUnhover;
    public void RaiseOnUnitHover(Unit unit)
    {
        Debug.Log("Mouse Hover");
        OnUnitHover.Invoke(unit);
    }

    public void RaiseOnUnitSelect(Unit unit)
    {
        Debug.Log("Mouse Select");
        OnUnitSelect.Invoke(unit);
    }

    public void RaiseOnUnitUnhover(Unit unit)
    {
        Debug.Log("Mouse Unhover");
        OnUnitUnhover.Invoke(unit);
    }
}