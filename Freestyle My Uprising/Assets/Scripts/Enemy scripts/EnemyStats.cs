using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public int attackDamage = 20;


    private Animator animator;
    private Rigidbody2D rb;
    private EnemyController enemyControl;
    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>(); 
        rb = GetComponent<Rigidbody2D>();
        enemyControl = GetComponent<EnemyController>();
    }

    public void TakeDamage(float damage, Vector3 attackSource)
    {
        currentHealth -= (int)damage;  // Cast to int if your health is an int
        if (animator != null)
        {
            animator.SetTrigger("DamageTaken");
        }

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            Vector3 knockbackDirection = (transform.position - attackSource).normalized + Vector3.up * 0.5f;
            float knockbackStrength = 2.5f;
            rb.AddForce(knockbackDirection * knockbackStrength, ForceMode2D.Impulse);

            // Assuming you have a method to cancel enemy actions
            enemyControl.CancelActions();
        }
    }

    private void Die()
    {
        if (animator != null)
        {
            animator.SetTrigger("Die");
        }
        Destroy(gameObject);
    }
}
