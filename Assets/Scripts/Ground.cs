using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    Player player;

    public float groundHeight;
    public float groundRight;
    public float screenRight;
    BoxCollider2D collider;

    bool didGenerateGround = false;

    // List of obstacles
    public List<Obstacle> obstacles = new List<Obstacle>();

    // New public field to set constant height
    public float constantHeight = 2.0f;

    // New field for obstacle density
    public float obstacleDensity = 0.5f;

    // New field for minimum distance from ledge
    public float minDistanceFromLedge = 2.0f;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        collider = GetComponent<BoxCollider2D>();
        screenRight = Camera.main.transform.position.x * 2;
    }

    void Update()
    {
        groundHeight = transform.position.y + (collider.size.y / 2);
    }

    private void FixedUpdate()
    {
        Vector2 pos = transform.position;
        pos.x -= player.velocity.x * Time.fixedDeltaTime;

        groundRight = transform.position.x + (collider.size.x / 2);

        if (groundRight < 0)
        {
            Destroy(gameObject);
            return;
        }

        if (!didGenerateGround)
        {
            if (groundRight < screenRight)
            {
                didGenerateGround = true;
                generateGround();
            }
        }

        transform.position = pos;
    }

    void generateGround()
    {
        GameObject go = Instantiate(gameObject);
        BoxCollider2D goCollider = go.GetComponent<BoxCollider2D>();
        Vector2 pos;

        // Use constantHeight instead of randomizing
        pos.y = constantHeight - goCollider.size.y / 2;
        if (pos.y > 2.7f)
            pos.y = 2.7f;

        float t1 = player.jumpVelocity / -player.gravity;
        float t2 = Mathf.Sqrt((2.0f * (constantHeight - pos.y)) / -player.gravity);
        float totalTime = t1 + t2;
        float maxX = totalTime * player.velocity.x;
        maxX *= 0.7f;
        maxX += groundRight;
        float minX = screenRight + 5;
        float actualX = groundRight - 5f; // Random.Range(minX, maxX);

        pos.x = actualX + goCollider.size.x / 2;
        go.transform.position = pos;

        Ground goGround = go.GetComponent<Ground>();
        goGround.groundHeight = constantHeight;

        // Calculate obstacle density based on player's distance
        float obstacleDensity = CalculateObstacleDensity(player.distance);

        // Spawn obstacles based on density
        float obstacleSpawnProbability = Random.Range(0.0f, 1.0f);
        if (obstacleSpawnProbability < obstacleDensity)
        {
            SpawnObstacle(goGround);
        }
    }




    void SpawnObstacle(Ground ground)
    {
        if (obstacles.Count == 0)
        {
            Debug.LogError("No obstacles in the list. Add obstacles in the Unity Editor.");
            return;
        }

        // Randomly pick an obstacle from the list
        int randomIndex = Random.Range(0, obstacles.Count);
        Obstacle selectedObstacle = obstacles[randomIndex];

        // Calculate minimum and maximum X positions for spawning the obstacle
        float halfWidth = ground.collider.size.x / 2 - 1;
        float left = ground.transform.position.x - halfWidth + minDistanceFromLedge;
        float right = ground.transform.position.x + halfWidth - minDistanceFromLedge;

        // Spawn the selected obstacle within the calculated range
        float x = Random.Range(left, right);
        float y = ground.groundHeight;
        Vector2 boxPos = new Vector2(x, y);

        GameObject box = Instantiate(selectedObstacle.gameObject, boxPos, Quaternion.identity);
    }

    float CalculateObstacleDensity(float playerDistance)
    {
        // Adjust the density based on the player's distance
        // You can define your own formula here
        // For example, increase density gradually from 0.1 to 1 as player distance increases from 100 to 5000

        float minDistance = 100.0f;
        float maxDistance = 2500.0f;

        float minDensity = 0.3f;
        float maxDensity = 1.0f;

        float normalizedDistance = Mathf.Clamp01((playerDistance - minDistance) / (maxDistance - minDistance));

        return Mathf.Lerp(minDensity, maxDensity, normalizedDistance);
    }
}
