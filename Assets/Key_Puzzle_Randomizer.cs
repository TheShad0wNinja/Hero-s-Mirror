using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Key_Puzzle_Randomizer : MonoBehaviour
{
    public ParticleSystem particleSystem = null;
    [SerializeField] GameObject chest;
    [SerializeField] GameObject door;
    public float interval = 2f; 
    public Color collectableColor = Color.blue; 

    public bool isCollectable = false, isDestructuble = false;
    SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject textBubblePrefab;
    private GameObject textBubbleInstance;

    private List<Color> colors = new List<Color>()
    {
        Color.red,
        Color.blue,
        Color.yellow,
        Color.white
    };

    private ParticleSystem.MainModule mainModule;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        if (particleSystem == null)
        {
            Debug.LogError("Particle system is not assigned!");
            return;
        }

        mainModule = particleSystem.main;
        StartCoroutine(RandomizeColor());
    }

    IEnumerator RandomizeColor()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);
            Color selectedColor = colors[Random.Range(0, colors.Count)];
            mainModule.startColor = selectedColor;
            isCollectable = selectedColor == collectableColor;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isCollectable)
        {
            if (textBubbleInstance == null && !isDestructuble)
            {
                textBubbleInstance = Instantiate(textBubblePrefab, transform.position + Vector3.up, Quaternion.identity);
                textBubbleInstance.GetComponentInChildren<TextMeshPro>().text = "Key Acquired";
                textBubbleInstance.transform.SetParent(transform);
                Color color = spriteRenderer.color;
                color.a = 0f;
                spriteRenderer.color = color;
                isDestructuble = true;
                StopCoroutine(RandomizeColor());
                particleSystem.gameObject.SetActive(false);
                chest.GetComponent<ChestScript>().canOpen = true;
                if (door.GetComponent<DoorScript>().canOpenKey)
                {
                    door.GetComponent<DoorScript>().UnlockDoorPuzzle();
                }
                else
                {
                    door.GetComponent<DoorScript>().UnlockDoorKey();
                }
            }
        }

        else if(!isCollectable && other.CompareTag("Player")) 
        {
            Color selectedColor = colors[3];
            mainModule.startColor = selectedColor;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isDestructuble)
        {
            Invoke("DestroyThis", 0.5f);
        }
    }
    void DestroyThis()
    {
        Destroy(this.gameObject);
    }
}
