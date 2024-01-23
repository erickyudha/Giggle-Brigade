using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public float depth = 1;
    public bool backgroundParallaxMode = true; // Toggle for background parallax mode
    public float leftBoundary = -25; // Left boundary for child repositioning
    public float closeGap = 1f;

    Player player;
    List<Transform> parallaxChildren = new List<Transform>();

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Start()
    {
        if (backgroundParallaxMode)
        {
            // Create left and right copies based on original child position and width
            CreateParallaxChildren();
        }
    }

    void FixedUpdate()
    {
        float realVelocity = player.velocity.x / depth;
        Vector2 pos = transform.position;

        pos.x -= realVelocity * Time.fixedDeltaTime;

        if (backgroundParallaxMode)
        {
            // Move each child based on their order/index and sprite width
            for (int i = 0; i < parallaxChildren.Count; i++)
            {
                Transform child = parallaxChildren[i];
                float childWidth = GetSpriteWidth(child);
                child.position = new Vector2(child.position.x - realVelocity * Time.fixedDeltaTime, child.position.y);

                // Check if the right edge (considering the pivot at the center) of the child is out of the camera on the left, then move it to the right
                if (child.position.x + childWidth / 2 <= leftBoundary)
                {
                    float rightmostX = GetRightmostChildX();
                    child.position = new Vector2(rightmostX + childWidth - closeGap, child.position.y);
                }
            }
        }
        else
        {
            if (pos.x <= leftBoundary)
                pos.x = 80;
        }

        transform.position = pos;
    }

    // Function to create left and right copies of the child sprite based on original child position and width
    void CreateParallaxChildren()
    {
        GameObject originalChild = transform.GetChild(0).gameObject;
        float originalChildWidth = GetSpriteWidth(originalChild.transform);

        // Create the initial copies based on the original child's position and width
        GameObject leftChild = Instantiate(originalChild, new Vector3(originalChild.transform.position.x - originalChildWidth + closeGap, originalChild.transform.position.y, 0), Quaternion.identity, transform);
        GameObject rightChild = Instantiate(originalChild, new Vector3(originalChild.transform.position.x + originalChildWidth - closeGap, originalChild.transform.position.y, 0), Quaternion.identity, transform);

        // Add them to the list
        parallaxChildren.Add(leftChild.transform);
        parallaxChildren.Add(originalChild.transform);
        parallaxChildren.Add(rightChild.transform);
    }

    // Helper function to get the rightmost child's X position
    float GetRightmostChildX()
    {
        float rightmostX = float.MinValue;
        foreach (Transform child in parallaxChildren)
        {
            if (child.position.x > rightmostX)
            {
                rightmostX = child.position.x;
            }
        }
        return rightmostX;
    }

    // Helper function to get the sprite width
    float GetSpriteWidth(Transform spriteTransform)
    {
        SpriteRenderer spriteRenderer = spriteTransform.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            return spriteRenderer.bounds.size.x;
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on the child sprite.");
            return 0f;
        }
    }
}
