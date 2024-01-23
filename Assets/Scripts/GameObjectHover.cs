using UnityEngine;

public class GameObjectHover : MonoBehaviour
{
    public float hoverHeight = 0.5f;    // The maximum height of the hover
    public float hoverSpeed = 1.0f;     // The speed of the hover animation

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;

        // Start the hover animation automatically when the scene starts
        StartHoverAnimation();
    }

    void Update()
    {
        // Perform the hover animation continuously
        HoverAnimation();
    }

    void StartHoverAnimation()
    {
        // No input needed, just set the initial position and let the animation run
    }

    void HoverAnimation()
    {
        // Calculate the new Y position based on a sine wave for smooth up and down motion
        float newY = startPos.y + Mathf.Sin(Time.time * hoverSpeed) * hoverHeight;

        // Update the object's position with the new Y coordinate
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
