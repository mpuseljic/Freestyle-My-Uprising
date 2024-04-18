using TMPro;
using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public GameObject interactionPopup; // Assign your UI text object to this in the inspector
    public TextMeshProUGUI dialogueText; // Assign the Text component in the inspector

    public string[] dialogues; // Assign specific dialogues for each NPC in the inspector

    private Transform player;
    private bool isPlayerInRange = false;
    private int dialogueIndex = 0; // To keep track of which dialogue text to display

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        interactionPopup.SetActive(false); // Assume the interactionPopup is disabled by default
    }

    void Update()
    {
        // If the player is in range and presses the "E" key, toggle the interaction popup
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            DisplayNextDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = true;
            // Show the interactionPopup when the player is in range, if you want
            // interactionPopup.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isPlayerInRange = false;
            interactionPopup.SetActive(false);
            dialogueIndex = 0; // Reset dialogue index when player leaves
        }
    }

    private void DisplayNextDialogue()
    {
        if (!interactionPopup.activeSelf)
        {
            interactionPopup.SetActive(true);
            dialogueIndex = 0; // Start from the beginning of the dialogues
        }

        if (dialogueIndex < dialogues.Length)
        {
            dialogueText.text = dialogues[dialogueIndex]; // Set the dialogue text to the current index
            dialogueIndex++; // Increment the index for the next call
        }
        else
        {
            interactionPopup.SetActive(false); // Hide the interactionPopup if at the end of the dialogues
            dialogueIndex = 0; // Reset the index
        }
    }
}
