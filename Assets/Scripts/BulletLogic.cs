using UnityEngine;

public class BulletLogic : MonoBehaviour
{
    public GameManagerScript gameManager;
    private float verticalVelocity = 0f;
    // Gravity simulation
    public float gravityForce;
    private Rigidbody2D bulletBody;
    private SpriteRenderer bulletSpriteRenderer; // Reference to the SpriteRenderer component

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bulletBody = GetComponent<Rigidbody2D>();
        bulletSpriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component

        // Rotate the sprite clockwise by 90 degrees
        bulletSpriteRenderer.transform.Rotate(0, 0, -90); // Rotate around the Z-axis (2D rotation)
    }

    // Update is called once per frame
    void Update()
    {
        verticalVelocity += gravityForce * Time.fixedDeltaTime;
        bulletBody.MovePosition(bulletBody.position - new Vector2(0, verticalVelocity * Time.fixedDeltaTime));
    }
}
