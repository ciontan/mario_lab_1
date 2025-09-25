using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Unity.VisualScripting;

public class JumpOverGoomba : MonoBehaviour
{
    public Transform enemyLocation;
    public TextMeshProUGUI scoreText;
    private bool onGroundState;

    [System.NonSerialized]
    public int score = 0; // we don't want this to show up in the inspector

    private bool countScoreState = false;
    public Vector3 boxSize;
    public float maxDistance;
    public LayerMask layerMask;
    private Boolean canCheckGoomba = true;
    private Boolean isOverlapped;
    private Boolean lastOverlapped;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isOverlapped = checkGoombaAbove();
        if (!lastOverlapped && isOverlapped)
        {
            score++;
            scoreText.text = "Score: " + score.ToString();
        }
        lastOverlapped = isOverlapped;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Ground")) onGroundState = true;
    }


    private bool checkGoombaAbove()
    {
        if (Physics2D.BoxCast(transform.position + new Vector3(0,1,0), boxSize, 0, transform.up, maxDistance, layerMask))
        {
            Debug.Log("goomba touched");
            return true;
        }
        else
        {
            Debug.Log("goommba not touched");
            return false;
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(transform.position - transform.up * maxDistance, boxSize);
    }

}
