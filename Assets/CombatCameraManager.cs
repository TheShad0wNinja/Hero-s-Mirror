using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CombatCameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera defaultCamera, actionCamera;
    CinemachineVirtualCamera currentCamera;

    public static CombatCameraManager Instance;

    void Start()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);

        defaultCamera.Priority = 20;
        actionCamera.Priority = 10;

        currentCamera = defaultCamera;
    }

    public static void SwitchCamera()
    {
        if (Instance == null) return;

        if (Instance.currentCamera == Instance.defaultCamera) 
        {
            Instance.currentCamera.Priority = 10;
            Instance.currentCamera = Instance.actionCamera;
            Instance.currentCamera.Priority = 20;
        }
        else 
        {

            Instance.currentCamera.Priority = 10;
            Instance.currentCamera = Instance.defaultCamera;
            Instance.currentCamera.Priority = 20;
        }
    }
}
