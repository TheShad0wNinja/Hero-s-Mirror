using UnityEngine;

[CreateAssetMenu(fileName = "NewConversation", menuName = "Conversation")]
public class Conversation : ScriptableObject
{
    public DialogueEntry[] dialogueEntries;
}
[System.Serializable]
public struct DialogueEntry
{
    public Sprite unitImage;  
    public string unitName;      
    public string dialogueText;  
}
