// DoorInteraction.cs
using UnityEngine;
using static PlayerController;

public class DoorInteraction : MonoBehaviour, IInteractable
{
    public LevelLoader levelLoader;

    private void Awake()
    {
        levelLoader = FindObjectOfType<LevelLoader>();
    }

    public void Interact()
    {
        levelLoader.LoadNextLevel();
    }
}
