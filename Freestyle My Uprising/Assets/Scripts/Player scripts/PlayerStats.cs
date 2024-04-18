using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth = 100;
    public int physicalDamage = 10;
    public int magicDamage = 15;

    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component not found on the player!");
        }
    }

    // Adjusted to take a second argument for the attack source
    public void TakeDamage(int damage, Vector3 attackSource)
    {
        currentHealth -= damage;
        if (animator != null)
        {
            animator.SetTrigger("TakeDamage");
        }
        // You can use attackSource here for additional effects like knockback

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    private void Die()
    {
        // Trigger the death animation
        animator.SetTrigger("Die");

        // Optional: Disable player controls/movement here
        GetComponent<PlayerController>().enabled = false; // Assuming Test is your player movement/control script.
        // You might also want to disable other components or trigger other actions like playing a sound effect.
    }
}
