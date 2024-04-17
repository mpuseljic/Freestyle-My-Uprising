using UnityEngine;

public class LadderClimbing : MonoBehaviour
{
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private LayerMask whatIsLadder;
    [SerializeField] private bool isClimbing;

    private Rigidbody2D rb;
    private Animator animator;
    private float verticalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        verticalInput = Input.GetAxisRaw("Vertical");

        if (isClimbing)
        {
            animator.SetBool("IsClimbing", true);  // Set climbing animation on

            if (verticalInput != 0)
            {
                rb.velocity = new Vector2(0, verticalInput * climbSpeed);
            }
            else
            {
                // Consider allowing some minor movements or zero out only vertical movement
                rb.velocity = new Vector2(rb.velocity.x, 0);
            }
        }
        else
        {
            animator.SetBool("IsClimbing", false);  // Ensure climbing animation is off when not climbing
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & whatIsLadder) != 0)
        {
            isClimbing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) & whatIsLadder) != 0)
        {
            isClimbing = false;
        }
    }
}
