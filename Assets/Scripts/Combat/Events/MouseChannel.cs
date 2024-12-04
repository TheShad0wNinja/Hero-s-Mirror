using System;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "MouseChannel")]
public class MouseChannel : ScriptableObject
{
    public UnityAction<Unit> OnUnitSelect;
    public UnityAction<Unit> OnUnitHover;
    public UnityAction<Unit> OnUnitUnhover;
    public void RaiseOnUnitHover(Unit character)
    {
        Debug.Log("Mouse Hover");
        OnUnitHover.Invoke(character);
    }

    public void RaiseOnUnitSelect(Unit character)
    {
        Debug.Log("Mouse Select");
        OnUnitSelect.Invoke(character);
    }

    public void RaiseOnUnitUnhover(Unit character)
    {
        Debug.Log("Mouse Unhover");
        OnUnitUnhover.Invoke(character);
    }
}