using UnityEngine;

public class ParallaxBackground : MonoBehaviour
{
    public float scrollSpeed = 0.5f; // Adjust this value to control the scroll speed
    private Transform playerTransform;
    private float startPosX;
    private float length;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        startPosX = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void Update()
    {
        float distX = (playerTransform.position.x - transform.position.x) * (1 - scrollSpeed);
        transform.position = new Vector3(startPosX + distX, transform.position.y, transform.position.z);

        // Wrap the background if it moves beyond its length
        float temp = (playerTransform.position.x - startPosX) * (1 - scrollSpeed);
        if (temp > length) startPosX += length;
        else if (temp < -length) startPosX -= length;
    }
}
