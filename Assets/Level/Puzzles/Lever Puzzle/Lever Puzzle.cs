using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverPuzzle : MonoBehaviour
{
    [System.Serializable]
    public class Lever
    {
        public GameObject leverObject;
        public bool isOn = false;
        [HideInInspector] public Collider proximityCollider;
    }

    public Lever[] levers;
    public bool[] correctCombination;
    private Lever currentLever = null;

    private void Start()
    {

        foreach (Lever lever in levers)
        {
            if (lever.leverObject != null)
            {
                lever.proximityCollider = lever.leverObject.GetComponent<Collider>();
                if (lever.proximityCollider == null)
                {
                    Debug.LogError($"Lever object '{lever.leverObject.name}' does not have a Collider. Add one to use proximity detection.");
                }
                else if (!lever.proximityCollider.isTrigger)
                {
                    lever.proximityCollider.isTrigger = true;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && currentLever != null)
        {
            ToggleLever(currentLever);
        }
    }

    private void ToggleLever(Lever lever)
    {
        lever.isOn = !lever.isOn;

        CheckCombination();
    }

    private void CheckCombination()
    {
        for (int i = 0; i < levers.Length; i++)
        {
            if (levers[i].isOn != correctCombination[i])
            {
                Debug.Log("Incorrect combination.");
                return;
            }
        }

        OnPuzzleSolved();
    }

    private void OnPuzzleSolved()
    {
        Debug.Log("Puzzle solved!");
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (Lever lever in levers)
        {
            if (other == lever.proximityCollider)
            {
                currentLever = lever;
                return;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (Lever lever in levers)
        {
            if (other == lever.proximityCollider && lever == currentLever)
            {
                currentLever = null;
                return;
            }
        }
    }
}