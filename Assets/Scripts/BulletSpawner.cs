using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    public GameObject bullet;
    public float spawnInterval; // Time interval between pipe spawns
    private float timer; // Timer to track the time since the last spawn
    private Rigidbody2D rb; // Reference to the Rigidbody2D component
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnInterval)
        {
            timer += Time.deltaTime; // Increment the timer by the time since the last frame
        }
        else
        {
            Vector3 rbPosition = rb.position;  // Rigidbody's position in world space
            Instantiate(bullet, rbPosition + new Vector3(0, 16, 0), Quaternion.identity); // Spawn a pipe at a random height between -2.5 and 2.5 on the y-axis
            timer = 0; // Reset the timer
            spawnInterval = Random.Range(1f, 2f); // Randomize the next spawn interval between 1 and 3 seconds
        }
    }
}
