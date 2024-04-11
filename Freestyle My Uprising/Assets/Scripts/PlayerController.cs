using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 4.0f;          // Speed of the character movement
    [SerializeField] private float runMultiplier = 1.5f;   // Multiplier for running speed
    [SerializeField] private float dashSpeed = 10f;        // Speed of the dash
    [SerializeField] private float jumpForce = 7.5f;       // Force of the jump
    [SerializeField] private float dashCooldown = 1f;      // Cooldown time for dash
    [SerializeField] private LayerMask groundLayer;        // Layer mask to determine what is ground
    [SerializeField] private Transform groundCheck;        // Transform to determine where to check for ground
    [SerializeField] private float groundCheckRadius = 0.2f;// Radius for ground check
    [SerializeField] private KeyCode heavyAttackKey = KeyCode.F; // The key to trigger the heavy attack


    private Rigidbody2D body2d;                           // Reference to the Rigidbody2D component
    private bool grounded = false;                        // Is the character on the ground?
    private bool isRunning = false;                       // Is the character running?
    private bool isDashing = false;                       // Is the character dashing?
    private Animator animator;                            // Reference to the Animator component
    private float lastDash = -1f;                          // Time when the last dash was initiated
    private bool isHeavyAttacking = false; // Is the character performing a heavy attack?
    private bool isAttacking = false; // Indicates if the attack is in progress


    void Start()
    {
        body2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        Jump();
        HeavyAttack();
        // Check for dash input and call Dash method
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDash + dashCooldown)
        {
            lastDash = Time.time; // Update the time when dash was last initiated
            Dash();
        }
        if (Input.GetMouseButtonDown(0)) // 0 is the left mouse button
        {
            Attack();
        }
        CheckGrounded();
    }

    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal");       // Get horizontal input

        // Determine if running based on input (example uses Left Control to run)
        isRunning = Input.GetKey(KeyCode.LeftControl);

        // Apply the run multiplier if running
        float moveSpeed = isRunning ? speed * runMultiplier : speed;

        // Move the character
        body2d.velocity = new Vector2(inputX * moveSpeed, body2d.velocity.y);

        // Update the animator with movement information
        animator.SetFloat("Speed", Mathf.Abs(inputX));
        animator.SetBool("IsRunning", isRunning);

        // Flip the character sprite direction depending on the move direction
        if (inputX > 0)
            GetComponent<SpriteRenderer>().flipX = false;
        else if (inputX < 0)
            GetComponent<SpriteRenderer>().flipX = true;
    }

    private void Jump()
    {
        // Handle jumping if the character is grounded and the jump key is pressed
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
            animator.SetTrigger("Jump");
            grounded = false; // Assuming there is another mechanism or method to set `grounded` true when landing
        }
    }

    private void Dash()
    {
        // Check if the character is on the ground to allow dash
        if (grounded && !isDashing)
        {
            isDashing = true;
            // Apply dash velocity
            float dashDirection = GetComponent<SpriteRenderer>().flipX ? -1 : 1;
            body2d.velocity = new Vector2(dashDirection * dashSpeed, body2d.velocity.y);
            animator.SetTrigger("Dash");
            // Reset the isDashing flag after a short duration
            Invoke("EndDash", 0.5f); // Assuming the dash lasts 0.5 seconds
        }
    }

    private void EndDash()
    {
        isDashing = false;
        animator.ResetTrigger("Dash"); // Reset the Dash trigger
    }

    // Example of a method to detect ground
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }
    }
    private void CheckGrounded()
    {
        // Check if the player's 'feet' are colliding with the ground
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // Update the animator with the grounded state
        animator.SetBool("Grounded", grounded);
    }
    private void HeavyAttack()
    {
        if (Input.GetKeyDown(heavyAttackKey) && grounded && !isDashing && !isHeavyAttacking)
        {
            isHeavyAttacking = true;
            animator.SetTrigger("HeavyAttack");
            // Other heavy attack logic goes here (e.g., applying force, enabling hitboxes)
        }
    }
    public void ResetHeavyAttack()
    {
        isHeavyAttacking = false;
        // Other logic to reset the heavy attack goes here
    }
    private void Attack()
    {
        if (!isAttacking && !isDashing)
        {
            Debug.Log("Attack initiated.");
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }
    public void ResetAttack()
    {
        isAttacking = false;
        // Additional logic to reset the attack goes here
    }
}
