using UnityEngine;
using System;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;        // Movement speed
    public float jumpForce = 7f;    // Jump power
    public LayerMask groundLayer;   // Ground detection layer

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private bool isGrounded;
    private bool isFacingBackward = false;
    bool Attacking = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Get Animator component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get SpriteRenderer component
    }

    void Update()
    {
        // Move left or right
        float moveInput = 0;
        if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1; // Move right
            if (isFacingBackward)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0); // Face forward
                isFacingBackward = false;
            }
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1; // Move left
            if (!isFacingBackward)
            {
                transform.rotation = Quaternion.Euler(0, 180, 0); // Face backward
                isFacingBackward = true;
            }
        }

        rb.linearVelocity = new Vector2(moveInput * speed, rb.linearVelocity.y);
        
        // Set animation state
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.Play("atk");
            StartCoroutine(Attack()); // Play attack animation when space is pressed
        }
        else if (moveInput != 0 && !Attacking)
        {
            animator.Play("dash"); // Play dash animation when moving
        }
        else if(!Attacking)
        {
            animator.Play("idle"); // Play idle animation when stationary
        }

        // Jump if on the ground
        if (Input.GetKeyDown(KeyCode.W) && IsGrounded())
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        }
    }
    IEnumerator Attack(){
        Attacking = true;
        yield return new WaitForSeconds(2f);
        Attacking = false;
    }

    bool IsGrounded()
    {
        // Check if player is touching the ground using a small raycast
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1.5f, groundLayer);
        return hit.collider != null;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * 1.5f);
    }
}
