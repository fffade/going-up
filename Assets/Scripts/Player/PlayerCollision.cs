using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;

public class PlayerCollision : MonoBehaviour
{
    private PlayerMovement _movement;

    private Collider2D _collider;

    private Rigidbody2D _rigidbody;

    [Header("Edge Detection")]
    [SerializeField]
    private float edgeYThreshold;

    [FormerlySerializedAs("edgeDetectionSpeed")]
    [SerializeField] private float edgeDetectionAmount;
    

    void Awake()
    {
        _movement = GetComponent<PlayerMovement>();

        _collider = GetComponent<Collider2D>();

        _rigidbody = GetComponent<Rigidbody2D>();
    }


    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            _movement.isGrounded = false;
            
            _movement.ResetCoyoteTimer();
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            _movement.isWallSliding = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckGrounded(collision);

        CheckWallSlide(collision);
        
        /* Check for losing game when hitting the fog */
        if (collision.gameObject.CompareTag("Fog"))
        {
            GameManager.Instance.OnLose();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        CheckGrounded(collision);
        
        CheckWallSlide(collision);
    }
    
    /* CHecks if hitting the ground with gravity for grounding */
    private void CheckGrounded(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Player should only 'ground' on the top of boxes and not on the side
            float yGap = (_collider.bounds.min.y - collision.collider.bounds.max.y);
            
            // Debug.Log("Hit ground with a gap of: " + yGap);

            // Detects side collision vs. above collision
            if (yGap < -0.1f)
            {
                // Minimal side collision allows for an edge detection push up
                // This allows for the illusion of fine control
                if (!collision.gameObject.GetComponent<EdgeDetectionIgnore>() && Mathf.Abs(yGap) <= edgeYThreshold)
                {
                    _movement.PushUpwards(edgeDetectionAmount);
                    
                    // Debug.Log("Edge detection triggered");
                }
                
                return;
            }

            if (_rigidbody.velocity.y > 0f)
                return;

            _movement.isGrounded = true;
            
            // Debug.Log("Grounded = true");
        }
    }

    /* Checks if hitting a wall sideways for a slide up */
    private void CheckWallSlide(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall") && _rigidbody.velocity.y > 0f)
        {
            _movement.isWallSliding = true;

            float xGap = (_collider.bounds.center.x - collision.collider.bounds.center.x);
            _movement.wallSlideDirection = (int) -(Mathf.Abs(xGap) / xGap);
            
            _movement.ResetWallJumpTimer();
        }
    }
    
}
