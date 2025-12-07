using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] public float moveSpeed = 1f;
    public float positionPlayer;
    [SerializeField] public float jumpForce = 2f;

    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;
    public Transform groundCheck;
    public LayerMask groundLayer;
    bool isGrounded;
    bool isPaused = false;

    private bool doubleJump;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (isPaused)
        {
            rb.velocity = Vector2.zero;
            return;
        }
        MovePlayer();
    }

    public void SetMove(bool pause)
    {
        isPaused = pause;
        if(isPaused)
        {
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = 1;
        }
    }

    public void MovePlayer()
    {
        // Movement
        if (Input.GetKey(KeyCode.A))
        {
            positionPlayer = -1;
            animator.SetBool("isRunning", true);
            spriteRenderer.flipX = true;  
        }
        else if (Input.GetKey(KeyCode.D))
        {
            positionPlayer = 1;
            animator.SetBool("isRunning", true);
            spriteRenderer.flipX = false;
        }
        else
        {
            positionPlayer = 0;
            animator.SetBool("isRunning", false);
        }
            

        transform.Translate(Vector3.right * moveSpeed * positionPlayer * Time.deltaTime);

        // Jump
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.5f, 0.25f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            doubleJump = true;
        }
        else if (Input.GetButtonDown("Jump") && doubleJump) {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            doubleJump = false;
        }
        
        
    }
}
