using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float originalX;
    public float maxOffset;
    public float enemyPatroltime;
    private int moveRight = -1;
    private Vector2 velocity;
    private Rigidbody2D enemyBody;
    public Vector3 startPosition = new Vector3(0.0f, 0.0f, 0.0f);
    private Vector2 resultantMovementVector;

    private bool isDescending = false;
    private float descentTime = 3f;
    private float targetY;

    // Gravity simulation
    public float gravityForce;
    private float verticalVelocity = 0f;

    public LayerMask groundLayer; // Assign Ground layer here in Inspector
    private Boolean isGrounded;

    public int checkMultiplier;
    private Vector3 offsetVector;
    public GameManagerScript gameManager;
    public int score = 0;

    void Start()
    {
        enemyBody = GetComponent<Rigidbody2D>();
        originalX = transform.position.x;
        ComputeVelocity();

        // Start the descent
        StartCoroutine(DescendAndActivateGravity());
        offsetVector = new Vector3(0,-1,0)/2;
    }

    void ComputeVelocity()
    {
        velocity = new Vector2((moveRight) * maxOffset / enemyPatroltime, 0);
    }

    void Movegoomba()
    {
        resultantMovementVector = new Vector2(0, 0);
        if (!isDescending)
        {
            resultantMovementVector += velocity * Time.fixedDeltaTime;

            // Apply gravity force if not grounded
            if (isGrounded)
            {
                verticalVelocity = 0;
            }
            else
            {
                verticalVelocity += gravityForce * Time.fixedDeltaTime;
            }
            

            // Add vertical velocity to resultant movement
            resultantMovementVector += new Vector2(0, verticalVelocity * Time.fixedDeltaTime);
            enemyBody.MovePosition(enemyBody.position + resultantMovementVector);
        }
    }

    void Update()
    {
        CheckGroundCollision();
    }
    void FixedUpdate()
    {
        CheckGroundCollision();
        if (Mathf.Abs(enemyBody.position.x - originalX) < maxOffset)
        {
            Movegoomba();
        }
        else
        {
            moveRight *= -1;
            ComputeVelocity();
            Movegoomba();
        }
    }

    // Coroutine to handle the descending and then activate gravity
    private IEnumerator DescendAndActivateGravity()
    {
        isDescending = true;
        float startY = transform.position.y;
        targetY = startY - 1f;
        float elapsedTime = 0f;

        while (elapsedTime < descentTime)
        {
            float newY = Mathf.Lerp(startY, targetY, elapsedTime / descentTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        isDescending = false;
    }

    // Continuous check for collisions with the ground
    void CheckGroundCollision()
    {
        // Perform a Raycast downwards to check if Goomba is touching the ground
        new Vector2(0, verticalVelocity * Time.fixedDeltaTime);
        RaycastHit2D hit = Physics2D.Raycast(transform.position+offsetVector, Vector2.down, 0.1f*checkMultiplier, groundLayer);

        // If we hit something and it's grounded, stop falling
        if (hit.collider != null)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Set vertical velocity to 0.25 when colliding with the player
            verticalVelocity = 9f;
            score++;
        }
        else if (other.CompareTag("Ground"))
        {
            isGrounded = true;
            // Check if the collision is on the side (left or right)
            if (Mathf.Abs(other.transform.position.y - transform.position.y) < 0.5f)
            {
                // Colliding on the side, reverse horizontal movement
                moveRight *= -1;
                ComputeVelocity();
            }

            // Check if the ground layer is water
            if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                gameManager.gameOver();

            }
        }
    }
}
