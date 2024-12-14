using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rune_Puzzle : MonoBehaviour
{
    public Button[] runeButtons;
    public string[] correctRunes;
    private string[] selectedRunes = new string[3];
    private int currentSelection = 0;
    private int incorrectGuesses = 0;

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject postProcessing;
    [SerializeField] private PlayerMovementController player;


    void Start()
    {
        foreach (Button rune in runeButtons)
        {
            rune.onClick.AddListener(() => OnRuneClick(rune.name));
        }
        player = FindObjectOfType<PlayerMovementController>();
    }

    void OnRuneClick(string runeName)
    {
        if (currentSelection < 3)
        {
            selectedRunes[currentSelection] = runeName;
            currentSelection++;

            Button clickedButton = System.Array.Find(runeButtons, b => b.name == runeName);
            clickedButton.image.color = Color.yellow;

            if (System.Array.Exists(correctRunes, correctRune => correctRune == runeName))
            {
                Debug.Log($"{runeName} is correct!");
            }
            else
            {
                Debug.Log($"{runeName} is incorrect!");
                incorrectGuesses++;
            }

            if (currentSelection == 3)
            {
                if (incorrectGuesses >= 3)
                {
                    Debug.Log("Game Over!");
                    ResetGame();
                }
                else
                {
                    CheckWinCondition();
                }
            }
        }
    }

    void CheckWinCondition()
    {
        int correctCount = 0;
        foreach (string selected in selectedRunes)
        {
            if (System.Array.Exists(correctRunes, correctRune => correctRune == selected))
            {
                correctCount++;
            }
        }

        if (correctCount == 3)
        {
            FindObjectOfType<DoorScript>().UnlockDoorPuzzle();
            player.runeSolved = true;
            canvas.SetActive(false);
            postProcessing.SetActive(false);
            ResetGame();
        }
        else
        {
            ResetGame();
        }
    }

    void ResetGame()
    {
        currentSelection = 0;
        incorrectGuesses = 0;

        foreach (Button rune in runeButtons)
        {
            rune.image.color = Color.white;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.SetActive(true);
            postProcessing.SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canvas.SetActive(false);
            postProcessing.SetActive(false);
        }
    }
}
