// DoorInteraction.cs
using UnityEngine;
using static PlayerController;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    public LevelLoader levelLoader;
    public AudioSource doorAudioSource;
    private void Awake()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
        doorAudioSource = GetComponent<AudioSource>();
        if (doorAudioSource == null)
        {
            Debug.LogWarning("AudioSource component not found on the door!");
        }
    }

    public void Interact()
    {
        levelLoader.LoadNextLevel();
    }
}
