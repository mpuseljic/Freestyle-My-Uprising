// DoorInteraction.cs
using UnityEngine;
using static Test;

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
