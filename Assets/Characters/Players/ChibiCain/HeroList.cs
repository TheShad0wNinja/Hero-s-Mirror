using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroList : MonoBehaviour
{
    public List<int> HeroLists = new List<int>();

    private void Start()
    {
        HeroLists.Add(100);
        HeroLists.Add(101);
        HeroLists.Add(102);
        HeroLists.Add(103);
    }
}
