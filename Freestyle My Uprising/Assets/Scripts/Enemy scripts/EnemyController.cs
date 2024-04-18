using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float patrolSpeed = 0.5f;
    public Transform edgeDetection;
    public float attackCooldown = 2f;
    public float detectionRange = 5f;
    public Transform player;
    public float attackRange = 3f;

    private bool movingRight = true;
    private float groundDistance = 1f;
    private LayerMask groundLayer;
    private float lastAttackTime = -10f;
    private bool isAttacking = false;
    private EnemyStats enemyStats;

    private Animator animator;
    private Rigidbody2D rb;

    [SerializeField] private float baseAttackDamage = 20f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundLayer = LayerMask.GetMask("Ground");
        enemyStats = GetComponent<EnemyStats>();
        if (enemyStats == null)
        {
            Debug.LogError("EnemyStats component not found on the enemy!");
        }
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            ManagePatrolAndDetection(distanceToPlayer);
        }
    }

    private void ManagePatrolAndDetection(float distanceToPlayer)
    {
        if (distanceToPlayer <= detectionRange)
        {
            Debug.Log("Player detected within detection range.");
            if (distanceToPlayer <= attackRange && Time.time > lastAttackTime + attackCooldown)
            {
                if (!isAttacking)
                {
                    AttackPlayer();
                }
            }
            else
            {
                isAttacking = false; // Allow to re-enter attack mode if conditions are met again
            }
        }
        else
        {
            Patrol();
            isAttacking = false; // Reset attacking state when out of range
        }
    }

    private void Patrol()
    {
        if (!isAttacking) // Ensure patrol only happens when not attacking
        {
            transform.Translate(Vector2.right * patrolSpeed * Time.deltaTime);
            RaycastHit2D groundInfo = Physics2D.Raycast(edgeDetection.position, Vector2.down, groundDistance, groundLayer);
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
    }

    private void Flip()
    {
        movingRight = !movingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        patrolSpeed = -patrolSpeed; // Change the direction of the patrol
    }

    private void AttackPlayer()
    {
        if (player == null)
            return;

        lastAttackTime = Time.time;
        isAttacking = true;
        FacePlayer();
        animator.SetTrigger("Attack");

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                // Cast baseAttackDamage to int before passing it
                playerStats.TakeDamage((int)baseAttackDamage, transform.position);
            }
        }
    }


    public void ResetAttack()
    {
        isAttacking = false; // This might be used externally if needed
    }

    private void FacePlayer()
    {
        if ((player.position.x < transform.position.x && movingRight) || (player.position.x > transform.position.x && !movingRight))
        {
            Flip();
        }
    }
    public void CancelActions()
    {
        // Reset all the flags
        isAttacking = false;

        // Reset animator triggers or states if necessary
        if (animator != null)
        {
            animator.ResetTrigger("Attack");
            animator.SetBool("IsAttacking", false); // Change this to your actual parameter name if different
            // Add more animator resets if needed
        }

        // Stop the current movement
        if (rb != null)
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }

        // Add any other resets for your enemy actions here
    }
    public void ExecuteAttackDamage()
    {
        if (player == null)
            return;

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            if (playerStats != null)
            {
                playerStats.TakeDamage((int)baseAttackDamage, transform.position);
                Debug.Log($"Dealt {baseAttackDamage} damage to player.");
            }
        }
    }

}
