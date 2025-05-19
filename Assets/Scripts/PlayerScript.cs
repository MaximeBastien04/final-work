using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool canMove = true;

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

    void FixedUpdate()
    {
        if (canMove)
            Movement();
        else
            animator.SetBool("isWalking", false);
    }


    public void EnableMovement() => canMove = true;
    public void DisableMovement() => canMove = false;

    private void Movement()
    {
        float moveInput = 0f;

        if (Input.GetKey(KeyCode.D))
        {
            moveInput = 1f;

            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            animator.SetBool("isWalking", true);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            moveInput = -1f;

            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
            animator.SetBool("isWalking", true);
        }
        else
        {
            animator.SetBool("isWalking", false);
        }

        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    public void PutDown()
    {
        GameObject.Find("Glass").GetComponent<GlassPickup>().PutDown();
    }
}
