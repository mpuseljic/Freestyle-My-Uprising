using System.Security.Cryptography;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 4.0f;
    [SerializeField] private float runMultiplier = 1.5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float jumpForce = 7.5f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.2f;
    [SerializeField] private KeyCode heavyAttackKey = KeyCode.F;
    [SerializeField] private KeyCode blockKey = KeyCode.Mouse1;
    [SerializeField] private KeyCode gunAttackKey = KeyCode.B;
    [SerializeField] private KeyCode magicAttackKey = KeyCode.R;


    private Rigidbody2D body2d;
    private bool grounded = false;
    private bool isRunning = false;
    private bool isDashing = false;
    private Animator animator;
    private float lastDash = -1f;
    private bool isHeavyAttacking = false;
    private bool isAttacking = false;
    private bool isBlocking = false;
    private bool isGunAttacking = false;
    private bool isMagicAttacking = false;
    private IInteractable interactable;

    void Start()
    {
        body2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move();
        if (!grounded)
        {
            if (body2d.velocity.y > 0)
            {
                body2d.gravityScale = 0.3f;
            }
            else
            {
                body2d.gravityScale = 0.7f;
            }
        }
        else
        {
            body2d.gravityScale = 1;
        }
        if (!grounded)
        {
            if (Input.GetMouseButtonDown(0)) 
            {
                JumpAttack();
            }
            else if (Input.GetMouseButtonDown(2)) 
            {
                JumpFlip();
            }
        }
        else
        {
            HeavyAttack();
            if (Input.GetMouseButtonDown(0)) 
            {
                Attack();
            }
            if (Input.GetMouseButtonDown(1)) 
            {
                Block();
            }
            if (Input.GetKeyDown(magicAttackKey))
            {
                MagicAttack();
            }
        }
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); 
        if (stateInfo.IsName("JumpAttack") && stateInfo.normalizedTime >= 0.95f) 
        {
            ResetJumpAttack();
        }
        if (Input.GetKeyDown(gunAttackKey) && !isGunAttacking)
        {
            GunAttack();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            TryDash();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key was pressed.");
            InteractWithObject();
        }

    }
    private void InteractWithObject()
    {
        if (interactable != null)
        {
            Debug.Log("Interacting with object.");
            interactable.Interact();
        }
        else
        {
            Debug.Log("No interactable object found.");
        }
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
    private void TryDash()
    {
        if (Time.time >= lastDash + dashCooldown)
        {
            Dash();
        }
    }
    private void Move()
    {
        float inputX = Input.GetAxis("Horizontal");       
        isRunning = Input.GetKey(KeyCode.LeftControl);
        float moveSpeed = isRunning ? speed * runMultiplier : speed;
        body2d.velocity = new Vector2(inputX * moveSpeed, body2d.velocity.y);
        animator.SetFloat("Speed", Mathf.Abs(inputX));
        bool isWalking = Mathf.Abs(inputX) > 0 && !isRunning;
        animator.SetBool("IsWalking", isWalking);
        animator.SetBool("IsRunning", isRunning);
        if (inputX > 0)
            GetComponent<SpriteRenderer>().flipX = false;
        else if (inputX < 0)
            GetComponent<SpriteRenderer>().flipX = true;
    }




    private void Dash()
    {
        if (grounded && !isDashing)
        {
            isDashing = true;
            float dashDirection = GetComponent<SpriteRenderer>().flipX ? -1 : 1;
            body2d.velocity = new Vector2(dashDirection * dashSpeed, body2d.velocity.y);
            animator.SetTrigger("Dash");
            Invoke("EndDash", 0.5f); 
        }
    }

    private void EndDash()
    {
        isDashing = false;
        animator.ResetTrigger("Dash"); 
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }
    }
    private void HeavyAttack()
    {
        if (Input.GetKeyDown(heavyAttackKey) && grounded && !isDashing && !isHeavyAttacking)
        {
            isHeavyAttacking = true;
            animator.SetTrigger("HeavyAttack");
        }
    }
    public void ResetHeavyAttack()
    {
        isHeavyAttacking = false;
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
    }
    private void Block()
    {
        if (grounded && !isBlocking && !isDashing && !isAttacking) 
        {
            isBlocking = true;
            animator.SetTrigger("Block");
        }
    }
    public void ResetBlock()
    {
        isBlocking = false;
    }
    private void JumpAttack()
    {
        if (!animator.GetBool("JumpAttack") && !grounded)
        {
            animator.SetBool("JumpAttack", true);
            body2d.gravityScale = 0;                                                    
            body2d.velocity = new Vector2(body2d.velocity.x, 0);
        }
    }
    private void JumpFlip()
    {
        if (!animator.GetBool("JumpFlip"))
        {
            animator.SetBool("JumpFlip", true);
        }
    }
    public void ResetJumpAttack()
    {
        animator.SetBool("JumpAttack", false);
        body2d.gravityScale = 1; 
    }

    public void ResetJumpFlip()
    {
        animator.SetBool("JumpFlip", false);
    }
    private void GunAttack()
    {
        isGunAttacking = true;
        animator.SetTrigger("GunAttack");
    }
    public void ResetGunAttack()
    {
        isGunAttacking = false;
    }
    private void MagicAttack()
    {
        if (!isMagicAttacking && !isDashing) 
        {
            isMagicAttacking = true;
            animator.SetTrigger("MagicAttack");
        }
    }

    public void ResetMagicAttack()
    {
        isMagicAttacking = false;
    }
    public interface IInteractable
    {
        void Interact();
    }
}
