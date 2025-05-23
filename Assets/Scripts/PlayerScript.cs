using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    private bool canMove = true;

    private Rigidbody2D rb;
    private Animator animator;


    PlayerControls controls;
    Vector2 move;

    void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Interact.performed += ctx => Interact();
        controls.Gameplay.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        controls.Gameplay.Move.canceled += ctx => move = Vector2.zero;
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        animator.SetBool("isWalking", false);
    }

    void FixedUpdate()
    {
        if (canMove)
            // Movement();
            Move();
        else
            animator.SetBool("isWalking", false);
    }

    public void Interact()
    {
        InteractionManager.Instance.Interact();
    }

    public void Move()
    {

        rb.linearVelocity = new Vector2(move.x * moveSpeed, rb.linearVelocity.y);
        if (move.x != 0f)
        {
            animator.SetBool("isWalking", true);
        }
        else
            animator.SetBool("isWalking", false);

        if (move.x < 0f) // going right
        {
            if (transform.localScale.x < 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
        else if (move.x > 0) // going left
        {
            if (transform.localScale.x > 0)
            {
                transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
            }
        }
    }


    public void EnableMovement() => canMove = true;
    public void DisableMovement() => canMove = false;

    // private void Movement()
    // {
    //     float moveInput = 0f;

    //     if (Input.GetKey(KeyCode.D))
    //     {
    //         moveInput = 1f;

    //         if (transform.localScale.x > 0)
    //         {
    //             transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    //         }
    //         animator.SetBool("isWalking", true);
    //     }
    //     else if (Input.GetKey(KeyCode.A))
    //     {
    //         moveInput = -1f;

    //         if (transform.localScale.x < 0)
    //         {
    //             transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    //         }
    //         animator.SetBool("isWalking", true);
    //     }
    //     else
    //     {
    //         animator.SetBool("isWalking", false);
    //     }

    //     rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    // }

    public void PutDown()
    {
        GameObject.Find("Glass").GetComponent<GlassPickup>().PutDown();
    }
}
