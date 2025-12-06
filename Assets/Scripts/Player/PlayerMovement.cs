using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] public float moveSpeed = 1f;
    public float positionPlayer;
    [SerializeField] public float jumpForce = 2f;

    [SerializeField] private Animator animator;
    private SpriteRenderer spriteRenderer;
    public Transform groundCheck;
    public LayerMask groundLayer;
    bool isGrounded;



    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        MovePlayer();
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
        isGrounded = Physics2D.OverlapCapsule(groundCheck.position, new Vector2(0.95f, 0.25f), CapsuleDirection2D.Horizontal, 0, groundLayer);

        if (Input.GetKey(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }
}
