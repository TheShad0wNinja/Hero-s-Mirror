using UnityEngine;

public class ManagersParent : MonoBehaviour
{
    public static ManagersParent instance;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(this);
    }
}