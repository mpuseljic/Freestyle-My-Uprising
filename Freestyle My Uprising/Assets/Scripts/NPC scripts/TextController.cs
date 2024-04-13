using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public GameObject interactionPopup; // Assign your UI text object to this in the inspector

    private Transform Player;
    private bool isPlayerInRange = false;

    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player").transform; // Find the player by tag
        // Assume the interactionPopup is disabled by default in the editor
    }

    void Update()
    {
        // If the player is in range and presses the "E" key, toggle the interaction popup
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TogglePopup();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // Optionally, automatically show the popup when the player is in range
            // interactionPopup.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            // Optionally, automatically hide the popup when the player exits the range
            interactionPopup.SetActive(false);
        }
    }

    private void TogglePopup()
    {
        bool isActive = interactionPopup.activeSelf;
        interactionPopup.SetActive(!isActive);
        if (!isActive)
        {
            Interact(); // Call your interaction function if the popup is being shown
        }
    }

    private void Interact()
    {
        // Add interaction logic here, e.g., dialogue
        Debug.Log("Interacting with NPC!");
    }
}
