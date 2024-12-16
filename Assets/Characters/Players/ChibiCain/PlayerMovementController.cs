using System;
using UnityEngine;

public class PlayerMovementController : MonoBehaviour
{
    [SerializeField] float movementSpeed = 2f;
    Vector2 _movementInput;
    Vector2 _prevInput;
    Rigidbody2D _rb;
    Animator _anim;
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
            _anim.Play("walkRight"); 
        else if (_rb.velocity.x < 0)
            _anim.Play("walkLeft");
        else if (_rb.velocity.y > 0)
            _anim.Play("walkUp");
        else if (_rb.velocity.y < 0)
            _anim.Play("walkDown");
        else if (_prevInput.x > 0)
            _anim.Play("idleRight"); 
        else if (_prevInput.x < 0)
            _anim.Play("idleLeft");
        else if (_prevInput.y > 0)
            _anim.Play("idleUp");
        else if (_prevInput.y < 0)
            _anim.Play("idleDown");
    }

    void GetKeyInput()
    {
        if (_movementInput != Vector2.zero)
            _prevInput = _movementInput;
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");
        _movementInput.Normalize();
    }
}
