using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPuzzelController : MonoBehaviour
{
    // Start is called before the first frame update
    public List<PuzzelItems> inventory;
    public List<PuzzelItems> PuzzelItems;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "pickUpObject")
        {
            for (int i = 0; i < PuzzelItems.Count; i++)
            {
                if (Other.gameObject.name == PuzzelItems[i].ItemName)
                {
                    inventory.Add(PuzzelItems[i]);
                    Debug.Log(PuzzelItems[i].name + "was picked up");
                    Destroy(Other.gameObject);
                }
                else if (Other.gameObject.name == PuzzelItems[i].ItemName)
                {
                    inventory.Add(PuzzelItems[i]);
                    Debug.Log(PuzzelItems[i].name + "was picked up");
                    Destroy(Other.gameObject);


                }
                else if (Other.gameObject.name == PuzzelItems[i].ItemName)
                {
                    inventory.Add(PuzzelItems[i]);
                    Debug.Log(PuzzelItems[i].name + "was picked up");
                    Destroy(Other.gameObject);

                }
            }


        }

    }
}
