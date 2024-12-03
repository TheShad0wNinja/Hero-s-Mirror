using UnityEngine;

public abstract class PassiveSO : ScriptableObject
{
    public abstract void SubscribeToEvent(Passive instance);
}