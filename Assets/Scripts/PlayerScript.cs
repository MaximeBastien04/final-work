using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator.SetBool("isWalking", false);
    }

    void Update()
    {
        Movement();
    }

    private void Movement()
    {
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;
            spriteRenderer.flipX = true;
            animator.SetBool("isWalking", true);
        }

        if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;
            spriteRenderer.flipX = false;
            animator.SetBool("isWalking", true);
        }

        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            animator.SetBool("isWalking", false);
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }
}
