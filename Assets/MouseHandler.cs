using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseHandler : MonoBehaviour
{
    public Character selectedCharacter;
    public List<Character> selectedCharacters;
    public bool canSelect;
    public int selectCount = 1;
    void Update()
    {
        if (canSelect && Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var hits = Physics2D.GetRayIntersectionAll(ray, 1500f);

            int count = 0;
            foreach (var hit in hits)
            {
                Character c = hit.collider.GetComponent<Character>();
                if (c != null && count < selectCount && !selectedCharacters.Contains(c))
                {
                    selectedCharacters.Add(c);
                    count++;
                }

                if (c != null){
                    selectedCharacter = c;
                    break;
                }
            }
        }
    }

    public void UnselectCharacter()
    {
        selectedCharacter = null;
        selectedCharacters.Clear();
        canSelect = false; 
    }
}
