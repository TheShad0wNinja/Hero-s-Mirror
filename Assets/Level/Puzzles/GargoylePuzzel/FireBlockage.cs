using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBlockage : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool FireSwitch(bool isFire)
    {
        var renderer = GetComponent<Renderer>();
        var collider = GetComponent<Collider2D>(); // Use Collider for 3D or Collider2D for 2D

        if (isFire)
        {
            renderer.enabled = false;
            collider.enabled = false;
            Debug.Log("Fire turned off");
            return !isFire;
        }
        else
        {
            renderer.enabled = true;
            collider.enabled = true;
            Debug.Log("Fire turned on");
            return !isFire;

        }
    }
}
