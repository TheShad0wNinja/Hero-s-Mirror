using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    Vector2 _movementInput;
    Rigidbody2D _rb;
    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        GetKeyInput();        
    }

    void FixedUpdate()
    {
        Move();
    }

    void Move()
    {
        _rb.velocity = _movementInput * movementSpeed;
    }
    void GetKeyInput()
    {
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");
        _movementInput.Normalize();
    }
}
