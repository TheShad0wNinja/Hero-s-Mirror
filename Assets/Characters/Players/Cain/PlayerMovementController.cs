using System;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    Vector2 _movementInput;
    Rigidbody2D _rb;
    Animator _anim;
    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();    
        _anim = GetComponentInChildren<Animator>();
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
        ChooseAnimation();
    }

    private void ChooseAnimation()
    {
        if (_rb.velocity.x > 0)
            _anim.Play("right"); 
        else if (_rb.velocity.x < 0)
            _anim.Play("left");
        else if (_rb.velocity.y > 0)
            _anim.Play("up");
        else if (_rb.velocity.y < 0)
            _anim.Play("down");
    }

    void GetKeyInput()
    {
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");
        _movementInput.Normalize();
    }
}
