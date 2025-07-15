using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] CharacterController _controller;
    [SerializeField] Vector3 _playerVelocity;
    [SerializeField] bool _groundedPlayer;
    [SerializeField] float _playerSpeed = 6.0f;
    [SerializeField] float _jumpHeight = 5.0f;
    [SerializeField] float _gravityValue = -9.81f;
    [SerializeField] float _crouchHeight = 0.5f;

    private float _originalHeight;
   [SerializeField] bool _isCrounching = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Move();
        Gravity();
        Jump();
        Crouch();

    }
        

        

        

    void Move()
    {
        _groundedPlayer = _controller.isGrounded;
        if (_groundedPlayer && _playerVelocity.y < 0)
        {
            _playerVelocity.y = 0f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        Vector3 move = new Vector3(horizontalInput, 0, 0);

        if (move.x != 0)
        {
            float targetAngle = move.x > 0 ? 0f : 180f;
            transform.rotation = Quaternion.Euler(0, targetAngle, 0f);
        }

        // Combine horizontal and vertical movement
        Vector3 finalMove = (move * _playerSpeed) + (_playerVelocity.y * Vector3.up);
        _controller.Move(finalMove * Time.deltaTime);
    }
    void Gravity()
    {
        // Apply gravity
        _playerVelocity.y += _gravityValue * Time.deltaTime;
    }

    void Jump()
    {
        // Jump
        if (_groundedPlayer && Input.GetAxisRaw("Vertical") > 0.5f)
        {
            _playerVelocity.y = Mathf.Sqrt(_jumpHeight * -2.0f * _gravityValue);
        }
    }

    void Crouch()
    {
        if (_groundedPlayer && Input.GetAxisRaw("Vertical") < -0.5f)
        {
            _isCrounching = true;
            _controller.height = _originalHeight;
        }
        else if (_isCrounching)
        {
            _isCrounching = false;
            _controller.height = _originalHeight;
        }
    }
}

