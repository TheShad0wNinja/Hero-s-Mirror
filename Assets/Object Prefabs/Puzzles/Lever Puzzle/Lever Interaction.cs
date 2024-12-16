using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverInteraction : MonoBehaviour
{
    private LeverPuzzle puzzleManager;
    private LeverPuzzle.Lever associatedLever;

    public void Setup(LeverPuzzle manager, LeverPuzzle.Lever lever)
    {
        puzzleManager = manager;
        associatedLever = lever;

         if (puzzleManager == null || associatedLever == null)
    {
        Debug.LogError("Invalid puzzleManager or associatedLever reference.");
        return;
    }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log(associatedLever);
            if(associatedLever!=null)
            puzzleManager.SetCurrentLever(associatedLever);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) 
        {
            if(associatedLever!=null)
            puzzleManager.ClearCurrentLever(associatedLever);
        }
    }
}