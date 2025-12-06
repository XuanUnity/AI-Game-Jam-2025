using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rd;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    private bool isGrounded = true;
    private void Start()
    {
        rd = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        MovePlayer();
    }
    public void MovePlayer()
    {
        float pressHorizontal = Input.GetAxis("Horizontal");

        if(pressHorizontal != 0)
        {
            rd.velocity = new Vector2(pressHorizontal * moveSpeed, rd.velocity.y);
        }
        else
        {
            rd.velocity = new Vector2(0, rd.velocity.y);
        }

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rd.velocity = new Vector2(rd.velocity.x, jumpForce);
            //isGrounded = false;
        }
    }
}
