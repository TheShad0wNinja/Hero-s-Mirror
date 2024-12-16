using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Load_UI : MonoBehaviour
{
    public static Load_UI Instance;

    public GameObject leveUIPanelPrefab;
    public GameObject levelPanel;
    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        Invoke("FindOrCreateLevelPanel",0.1f);
    }
    private void FindOrCreateLevelPanel()
    {
        if (SceneManager.GetActiveScene().name != "HomeBase" && SceneManager.GetActiveScene().name != "Combat" ) 
        {
            levelPanel = GameObject.FindGameObjectWithTag("LevelPanel");

            if (levelPanel == null)
            {
                GameObject canvas = GameObject.Find("Canvas");
                if (canvas == null) throw new System.Exception("Canvas not found in scene.");

                levelPanel = Instantiate(leveUIPanelPrefab, canvas.transform);
            }
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
