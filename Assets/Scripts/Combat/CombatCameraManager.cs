using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CombatCameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera defaultCamera, actionCamera;

    public static CombatCameraManager Instance;
    float prevBlendTime;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        defaultCamera.Priority = 20;
        actionCamera.Priority = 10;

        var brain = Camera.main.GetComponent<CinemachineBrain>();

        prevBlendTime = brain.m_DefaultBlend.m_Time;
        brain.m_DefaultBlend.m_Time = 0.1f;

    }

    void OnDisable()
    {
        if (Camera.main != null)
        {
            var brain = Camera.main.GetComponent<CinemachineBrain>();

            brain.m_DefaultBlend.m_Time = prevBlendTime;
        }
    }

    public static void SwitchToDefaultCamera()
    {
        if (Instance == null) return;

        Instance.defaultCamera.Priority = 20;
        Instance.actionCamera.Priority = 10;
    }

    public static void SwitchToActionCamera()
    {
        if (Instance == null) return;

        Instance.actionCamera.Priority = 20;
        Instance.defaultCamera.Priority = 10;
    }
}
