using System.Collections;
using UnityEngine;

public class Test : MonoBehaviour
{
    private float horizontal;
    private float speed = 0.7f;  
    private float runningMultiplier = 1.3f;  
    private float jumpingPower = 2f;
    private bool isFacingRight = true;
    private bool isDashing = false;
    private float dashSpeed = 1.2f;
    private float dashCooldown = 0f;
    private float lastDashTime = -10f;
    private IInteractable interactable;
    private bool isHeavyAttacking = false;
    private bool isGunAttacking = false;
    private bool isMagicAttacking = false;
    private Animator animator;
    private bool isAttacking = false;
    private bool isJumpAttacking = false;
    private bool isClimbing = false;
    private int jumpCount = 0;  
    private int maxJumpCount = 2;
    private bool isBlocking = false;


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundlayer;
    [SerializeField] private KeyCode heavyAttackKey = KeyCode.F;
    [SerializeField] private KeyCode gunAttackKey = KeyCode.B;
    [SerializeField] private KeyCode magicAttackKey = KeyCode.R;
    [SerializeField] private KeyCode attackKey = KeyCode.Mouse0;
    [SerializeField] private KeyCode blockKey = KeyCode.Mouse1;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("Animator component not found on the player!");
        }
    }

    void Update()
    {
        HandleInput();
        HeavyAttack();
        GunAttack();
        MagicAttack();
        Attack();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }
    private void HandleInput()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDashTime + dashCooldown && !isDashing)
        {
            animator.SetTrigger("Dash");
            StartCoroutine(Dash());
        }

        if (Input.GetButtonDown("Jump") && IsGrounded() && !isClimbing) // Make sure climbing doesn't block jumping
        {
            Jump();
        }

        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0)
        {
            CutJumpShort();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            InteractWithObject();
        }
        if (Input.GetKeyDown(blockKey))
        {
            StartBlock();
        }
        if (Input.GetKeyUp(blockKey))
        {
            EndBlock();
        }
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            Debug.Log("Middle mouse button pressed.");
            if (!IsGrounded() && !isClimbing)
            {
                PerformJumpFlip();
            }
        }
    }

    private void HandleMovement()
    {
        if (!isDashing)  // Only control movement if not currently dashing
        {
            float move = horizontal * speed;
            bool isMoving = Mathf.Abs(move) > 0;
            bool isRunning = isMoving && Input.GetKey(KeyCode.LeftControl);

            // Apply the running multiplier if the character is running
            if (isRunning)
            {
                move *= runningMultiplier;
            }

            // Update the speed in the animator to handle walking/running animations.
            rb.velocity = new Vector2(move, rb.velocity.y);
            animator.SetFloat("Speed", Mathf.Abs(move));
            animator.SetBool("IsRunning", isRunning);
            animator.SetBool("IsWalking", isMoving && !isRunning);

            // Set the character's velocity based on input.
            rb.velocity = new Vector2(move, rb.velocity.y);

            // Flip the character to face the direction of movement.
            Flip();
            if (IsGrounded() && jumpCount != 0)
            {
                jumpCount = 0;
                animator.SetBool("IsJumping", false);
            }
        }
        else
        {
            // Optionally reduce movement speed or prevent movement altogether while blocking
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        // Handle grounded state for jumping and falling animations.
        animator.SetBool("Grounded", IsGrounded());
    }

    private IEnumerator Dash()
    {
        float dashTime = 0.3f;  // Duration of the dash
        isDashing = true;
        animator.SetBool("IsDashing", true); // Set the IsDashing parameter to true when starting the dash.

        float dashDirection = isFacingRight ? 1 : -1;
        rb.velocity = new Vector2(dashDirection * dashSpeed, rb.velocity.y);
        lastDashTime = Time.time;

        yield return new WaitForSeconds(dashTime);

        isDashing = false;
        animator.SetBool("IsDashing", false); // Set the IsDashing parameter to false when the dash ends.
    }

    private void Jump()
    {
        if (IsGrounded() || jumpCount < maxJumpCount)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            if (jumpCount == 0)
            {
                animator.SetTrigger("Jump");  // Activate the Jump trigger in the Animator
            }
            jumpCount++;
        }
    }

    private void CutJumpShort()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
    }

    private void Flip()
    {
        if ((isFacingRight && horizontal < 0) || (!isFacingRight && horizontal > 0))
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundlayer);
    }
    public interface IInteractable
    {
        void Interact();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable"))
        {
            interactable = collision.GetComponent<IInteractable>();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Interactable") && interactable != null)
        {
            interactable = null;
        }
    }
    private void InteractWithObject()
    {
        if (interactable != null)
        {
            interactable.Interact();
        }
        else
        {
            Debug.Log("No interactable object found.");
        }
    }
    private void HeavyAttack()
    {
        if (Input.GetKeyDown(heavyAttackKey) && !isDashing && !isHeavyAttacking) // Add other conditions as needed
        {
            isHeavyAttacking = true;
            animator.SetTrigger("HeavyAttack");
        }
    }

    public void ResetHeavyAttack()
    {
        isHeavyAttacking = false;
    }
    private void GunAttack()
    {
        if (Input.GetKeyDown(gunAttackKey) && !isDashing && !isGunAttacking)
        {
            isGunAttacking = true;
            animator.SetTrigger("GunAttack");
        }
    }

    public void ResetGunAttack()
    {
        isGunAttacking = false;
    }
    private void MagicAttack()
    {
        if (Input.GetKeyDown(magicAttackKey) && !isDashing && !isMagicAttacking)
        {
            isMagicAttacking = true;
            animator.SetTrigger("MagicAttack");
        }
    }

    public void ResetMagicAttack()
    {
        isMagicAttacking = false;
    }
    private void Attack()
    {
        if (Input.GetKeyDown(attackKey) && !isDashing && !isAttacking && !isJumpAttacking)
        {
            isAttacking = true;
            animator.SetTrigger("Attack");
        }
    }

    public void ResetAttack()
    {
        isAttacking = false;
    }
    private void StartBlock()
    {
        isBlocking = true;
        animator.SetBool("Block", true); // Assume you have an "IsBlocking" bool parameter in your animator
                                              // Additional logic to reduce damage or prevent movement, if needed
    }

    private void EndBlock()
    {
        isBlocking = false;
        animator.SetBool("Block", false);
        // Reset any modified states or effects from blocking
    }
    private void PerformJumpFlip()
    {
        Debug.Log($"Attempting to jump flip: IsGrounded = {IsGrounded()}, isClimbing = {isClimbing}, isDashing = {isDashing}, jumpCount = {jumpCount}");
        if (!IsGrounded() && !isClimbing && !isDashing && jumpCount > 0)
        {
            animator.SetTrigger("JumpFlip");
        }
    }



}
