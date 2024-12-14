using System;
using UnityEngine;
using UnityEngine.Events;

public class MouseManager : MonoBehaviour
{
    public UnityAction<Unit> OnUnitSelect;
    public UnityAction<Unit> OnUnitHover;
    public UnityAction<Unit> OnUnitUnhover;

    public static MouseManager Instance {get; private set;}

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    public void RaiseOnUnitHover(Unit unit)
    {
        // Debug.Log("Mouse Hover");
        OnUnitHover?.Invoke(unit);
    }

    public void RaiseOnUnitSelect(Unit unit)
    {
        // Debug.Log("Mouse Select");
        OnUnitSelect?.Invoke(unit);
    }

    public void RaiseOnUnitUnhover(Unit unit)
    {
        // Debug.Log("Mouse Unhover " + unit.name);
        OnUnitUnhover?.Invoke(unit);
    }
}