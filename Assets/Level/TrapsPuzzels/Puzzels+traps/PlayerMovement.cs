using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    // Movement speed of the player
    public float moveSpeed = 5f;

    // Reference to the Rigidbody2D component
    private Rigidbody2D rb;

    // Store the movement input
    private Vector2 movement;
    public List<PuzzelItems> inventory;
    public PuzzelItems itemOne;
    public PuzzelItems itemTwo;
    public PuzzelItems itemThree;


    void Start()
    {
        // Get the Rigidbody2D component attached to the player
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Get movement input from the player (WASD or Arrow Keys)
        movement.x = Input.GetAxisRaw("Horizontal"); // A/D or Left/Right Arrows
        movement.y = Input.GetAxisRaw("Vertical");   // W/S or Up/Down Arrows

        // Normalize the movement vector to prevent faster diagonal movement
        movement = movement.normalized;
    }

    void FixedUpdate()
    {
        // Move the player using Rigidbody2D
        rb.velocity = movement * moveSpeed;
    }
    void OnTriggerEnter2D(Collider2D Other)
    {
        if (Other.gameObject.tag == "pickUpObject")
        {
            if (Other.gameObject.name == itemOne.ItemName)
            {
                inventory.Add(itemOne);
                Debug.Log(itemOne.name + "was picked up");
                Destroy(Other.gameObject);
            }
            else if (Other.gameObject.name == itemTwo.ItemName)
            {
                inventory.Add(itemTwo);
                Debug.Log(itemTwo.name + "was picked up");
                Destroy(Other.gameObject);


            }
            else if (Other.gameObject.name == itemThree.ItemName)
            {
                inventory.Add(itemThree);
                Debug.Log(itemThree.name + "was picked up");
                Destroy(Other.gameObject);

            }

        }

    }

}
