using UnityEngine;

public class LadderClimbing : MonoBehaviour
{
    [SerializeField] private float climbSpeed = 3f;
    [SerializeField] private LayerMask whatIsLadder;
    [SerializeField] private bool isClimbing;

    // Reference to the Rigidbody2D and Animator of the player
    private Rigidbody2D rb;
    private Animator animator;

    // Variable to store vertical input
    private float verticalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Get the vertical input
        verticalInput = Input.GetAxisRaw("Vertical");
        if (isClimbing && verticalInput != 0)
        {
            rb.velocity = new Vector2(0, verticalInput * climbSpeed);
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
        }
        // If there's vertical input and the player is in the climbing state,
        // the player should climb up or down.

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player has collided with the ladder
        if (((1 << collision.gameObject.layer) & whatIsLadder) != 0)
        {
            isClimbing = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Check if the player has exited the ladder
        if (((1 << collision.gameObject.layer) & whatIsLadder) != 0)
        {
            isClimbing = false;
        }
    }
}
