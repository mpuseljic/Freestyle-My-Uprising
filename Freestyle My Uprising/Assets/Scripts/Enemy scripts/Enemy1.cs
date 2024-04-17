using UnityEngine;

public class Enemy1 : MonoBehaviour
{
    public float patrolSpeed = 0.5f;
    public Transform edgeDetection;

    private bool movingRight = true;
    private float groundDistance = 1f;
    private LayerMask groundlayer;

    private Animator animator; // If your enemy has animations
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundlayer = LayerMask.GetMask("Ground");
        // Set your ground layer here. Example: groundlayer = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);

        RaycastHit2D groundInfo = Physics2D.Raycast(edgeDetection.position, Vector2.down, groundDistance, groundlayer);

        // Since edgeDetection.position is a Vector3 and Vector2.down is a Vector2,
        // we need to cast Vector2.down to a Vector3 to add them together.
        Debug.DrawLine(edgeDetection.position, edgeDetection.position + (Vector3)Vector2.down * groundDistance, Color.red);

        if (!groundInfo.collider)
        {
            Debug.Log("No ground detected - flipping");
            Flip();
        }
        else
        {
            Debug.Log("Ground detected - continuing patrol");
        }
    }



    private void Flip()
    {
        movingRight = !movingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        patrolSpeed = -patrolSpeed; // Change the direction of the patrol
    }
}
