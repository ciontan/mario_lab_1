using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 70;
    public float maxSpeed = 80;
    public float jumpForce = 30f;
    public float holdForce = 10f;
    public float maxJumpVelocity = 20f;
    private bool isJumping = false;
    private bool onGroundState = true;
    private Rigidbody2D marioBody;
    private SpriteRenderer marioSprite;
    private bool faceRightState = true;
    public TextMeshProUGUI scoreText;
    public GameObject enemies;
    public JumpOverGoomba jumpOverGoomba;

    public GameManagerScript gameManager;

    private bool isDead;

    // Dash Variables
    public float dashSpeed = 50f;
    public float dashDuration = 0.2f;
    private bool isDashing = false;
    private float dashTime = 0f;
    private Vector2 dashDirection;

    // Start is called before the first frame update
    void Start()
    {
        // Set to be 30 FPS
        Application.targetFrameRate = 30;
        marioBody = GetComponent<Rigidbody2D>();
        marioSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        // Flip character sprite when moving left or right
        if (moveHorizontal < 0 && faceRightState)
        {
            faceRightState = false;
            marioSprite.flipX = true;
        }

        if (moveHorizontal > 0 && !faceRightState)
        {
            faceRightState = true;
            marioSprite.flipX = false;
        }

        // Space key action for dashing
        if (Input.GetKeyDown(KeyCode.Space) && !isDashing)
        {
            marioBody.linearVelocity = new Vector2(0, 0); // Stop vertical velocity
            isDashing = true;
            dashTime = 0f;

            // Set dash direction based on current movement direction
            dashDirection = new Vector2(marioSprite.flipX ? -1 : 1, 0); // Dash left or right

            // Start dash
            marioBody.linearVelocity = new Vector2(dashDirection.x * dashSpeed, marioBody.linearVelocity.y);
        }

        // W key for jump
        if (Input.GetKeyDown(KeyCode.W) && onGroundState)
        {
            marioBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            onGroundState = false;
            isJumping = true;
        }

        // Hold jump: apply smaller force while rising
        if (Input.GetKey(KeyCode.W) && isJumping)
        {
            if (marioBody.linearVelocity.y > 0 && marioBody.linearVelocity.y < maxJumpVelocity)
            {
                marioBody.AddForce(Vector2.up * holdForce, ForceMode2D.Force);
            }
        }

        // Short hop: if released early, cut upward velocity
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (marioBody.linearVelocity.y > 0)
            {
                marioBody.linearVelocity = new Vector2(marioBody.linearVelocity.x, marioBody.linearVelocity.y * 0.25f);
            }
            isJumping = false;
        }
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            dashTime += Time.fixedDeltaTime;
            if (dashTime >= dashDuration)
            {
                // End dash after duration
                isDashing = false;
                marioBody.linearVelocity = new Vector2(0, marioBody.linearVelocity.y); // Stop dash movement
            }
        }

        float moveHorizontal = Input.GetAxisRaw("Horizontal");

        // Move player horizontally (when not dashing)
        if (!isDashing && Mathf.Abs(moveHorizontal) > 0)
        {
            Vector2 movement = new Vector2(moveHorizontal, 0);
            // check if it doesn't go beyond maxSpeed
            if (marioBody.linearVelocity.magnitude < maxSpeed)
                marioBody.AddForce(movement * speed);
        }

        // Stop player when keys are released
        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            marioBody.linearVelocity = new Vector2(0, marioBody.linearVelocity.y);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Collided with goomba!");
        }
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) onGroundState = true;
    }

    public void RestartButtonCallback(int input)
    {
        Debug.Log("Restart!");
        // reset everything
        ResetGame();
        // resume time
        Time.timeScale = 1.0f;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
