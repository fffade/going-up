using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    
    public bool IsMovingRight { get; private set; }
    public bool IsMovingLeft { get; private set; }

    public bool isGrounded = true;

    public bool IsAtApexPoint { get; private set; }

    public bool isWallSliding = false;
    public int wallSlideDirection = 1;
    
    // Movement settings
    [Space(10f)]
    [Header("Movement & Jump")]
    
    [SerializeField] private float moveSpeed;

    [SerializeField] private float jumpForce,
                                   wallJumpYForce;

    [SerializeField] private float jumpBufferTime;
    private float _jumpBufferTimer;
    
    [SerializeField] private float jumpReleaseForce;
    private bool _isJumpReleased = false;
    
    [SerializeField] private float coyoteTime;
    private float _coyoteTimer;

    [Space(10f)]
    [Header("Apex")]
    
    [SerializeField] private float apexSpeedBonus;

    [SerializeField] private float apexTime;
    private float _apexTimer;

    [Space(10f)]
    [Header("Wall Slide & Jump")]
    
    [SerializeField] private float wallSlideBonus;

    [SerializeField] private float wallJumpSpeed;

    [SerializeField] private float wallJumpDelay,
                                    wallJumpTime;
    private float _wallJumpDelayTimer,
                  _wallJumpTimer;

    [SerializeField] private float wallJumpMovePauseTime;
    private float _wallJumpMovePauseTimer = 0f;


    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        CheckInput();
    }

    void FixedUpdate()
    {
        UpdateMovement();
    }
    
    // Use input to adjust velocity
    private void UpdateMovement()
    {
        /* Jump buffer allows a period of time to press jump before hitting the ground */
        if (_jumpBufferTimer > 0f)
        {
            _jumpBufferTimer = Mathf.Max(_jumpBufferTimer - Time.fixedDeltaTime, 0f);
        
            // Once hitting the ground, jump upwards
            if (isGrounded || _coyoteTimer > 0f) // Coyote time allows player to jump briefly after falling
            {
                Jump();
        
                _jumpBufferTimer = 0f;
            }
        }

        _coyoteTimer = Mathf.Max(_coyoteTimer - Time.fixedDeltaTime, 0f);
        
        /* Wall jumps pause horizontal movement for a brief period to allow for a smooth jump off */
        if (_wallJumpMovePauseTimer > 0f)
        {
            IsMovingLeft = false;
            IsMovingRight = false;

            _wallJumpMovePauseTimer = Mathf.Max(_wallJumpMovePauseTimer - Time.fixedDeltaTime, 0f);
        }
        
        float xDirection = IsMovingLeft ? -1f : (IsMovingRight ? 1f : 0f);

        _rigidbody.velocity = new Vector2(xDirection * moveSpeed * Time.fixedDeltaTime, _rigidbody.velocity.y);
        
        
        /* Apex calculations for bonus movement speed */
        IsAtApexPoint = !isGrounded && (Mathf.Abs(_rigidbody.velocity.y) <= 0.1f);

        if (IsAtApexPoint)
        {
            // Debug.Log("Apex bonus provided");
            _apexTimer = apexTime;
        }

        if (_apexTimer > 0)
        {
            // Count down timer
            _apexTimer = Mathf.Max(_apexTimer - Time.deltaTime, 0f);
            
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x * apexSpeedBonus, _rigidbody.velocity.y);
        }
        
        /* Jump release forces player down on release of jump key */
        if (!_isJumpReleased && !isGrounded && !Input.GetKey(KeyCode.W))
        {
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y - jumpReleaseForce * Time.fixedDeltaTime);

            _isJumpReleased = true;
        }
        
        /* Wall sliding provides a slight upward force */

        if (isGrounded)
            isWallSliding = false;
        
        if (isWallSliding && !Input.GetKey(KeyCode.S))
        {
            _wallJumpDelayTimer = Mathf.Max(_wallJumpDelayTimer - Time.fixedDeltaTime, 0f);
            
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y + wallSlideBonus * Time.fixedDeltaTime);
        }
        
        /* Wall jump provides a boost of speed horizontally */
        if (!isGrounded && _wallJumpTimer > 0f)
        {
            _wallJumpTimer = Mathf.Max(_wallJumpTimer - Time.fixedDeltaTime, 0f);
            
            // Wall jump continues speed boost in direction jumped
            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x + -wallSlideDirection * wallJumpSpeed * Time.fixedDeltaTime, _rigidbody.velocity.y);
        }
    }

    // Reads input keys
    private void CheckInput()
    {
        IsMovingLeft = (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D));
        IsMovingRight = (!IsMovingLeft && Input.GetKey(KeyCode.D));

        // Read jump input
        if (Input.GetKeyDown(KeyCode.W))
        {
            _jumpBufferTimer = jumpBufferTime;

            _isJumpReleased = false;
        }
        
        // Read wall jump input
        if (isWallSliding && (wallSlideDirection == -1 && Input.GetKey(KeyCode.D) || wallSlideDirection == 1 && Input.GetKey(KeyCode.A)))
        {
            WallJump();

            _wallJumpMovePauseTimer = wallJumpMovePauseTime;
        }
    }
    
    // Forces player to jump upwards
    public void Jump()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, (jumpForce * Time.fixedDeltaTime));
        
        // Debug.Log("JUMP");
    }
    
    // Forces player to wall jump sideways
    public void WallJump()
    {
        _rigidbody.velocity = new Vector2(-wallSlideDirection * wallJumpSpeed * Time.fixedDeltaTime, (wallJumpYForce * Time.fixedDeltaTime));

        _wallJumpTimer = wallJumpTime;
        
        // Debug.Log("Wall jump initiated");
    }
    
    // Push upwards
    public void PushUpwards(float amount)
    {
        _rigidbody.MovePosition(new Vector2(_rigidbody.position.x, _rigidbody.position.y + amount));
    }
    
    // Resets the wall jump timer when hitting a wall slide
    public void ResetWallJumpTimer()
    {
        _wallJumpDelayTimer = wallJumpDelay;
    }
    
    // Resets coyote time to allow for brief jump period
    public void ResetCoyoteTimer()
    {
        _coyoteTimer = coyoteTime;
    }
}
