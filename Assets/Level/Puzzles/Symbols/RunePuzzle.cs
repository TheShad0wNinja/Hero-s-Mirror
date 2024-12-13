using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class RunePuzzle : MonoBehaviour
{
    public Button[] runeButtons; // Assign all rune buttons in the inspector
    public string[] correctRunes; // Set correct rune names in the inspector
    private string[] selectedRunes = new string[3];
    private int currentSelection = 0;
    private int incorrectGuesses = 0;

    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject postProcessing;


    void Start()
    {
        foreach (Button rune in runeButtons)
        {
            rune.onClick.AddListener(() => OnRuneClick(rune.name));
        }
    }

    void OnRuneClick(string runeName)
    {
        if (currentSelection < 3)
        {
            selectedRunes[currentSelection] = runeName;
            currentSelection++;

            // Change button color to indicate selection
            Button clickedButton = System.Array.Find(runeButtons, b => b.name == runeName);
            clickedButton.image.color = Color.yellow;

            // Check if it's correct
            if (System.Array.Exists(correctRunes, correctRune => correctRune == runeName))
            {
                Debug.Log($"{runeName} is correct!");
            }
            else
            {
                Debug.Log($"{runeName} is incorrect!");
                incorrectGuesses++;
            }

            // End logic
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
            Debug.Log("You Win!");
            // Add win logic here
        }
        else
        {
            Debug.Log("Try Again!");
        }

        ResetGame();
    }

    void ResetGame()
    {
        currentSelection = 0;
        incorrectGuesses = 0;

        foreach (Button rune in runeButtons)
        {
            rune.image.color = Color.white; // Reset button colors
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