using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed;
    private float xVelocity = 0;
    private float yVelocity = 0;
    Rigidbody2D rb2d;
    bool isMoving = false;


    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        isMoving = false;
        MoveObject();
    }

    private void MoveObject()
    {
        if (speed == 0)
        {
            Debug.Log("Speed set to 0 for NPC: " + this.name);
            return;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            xVelocity = -speed;
        }else if (Input.GetKey(KeyCode.RightArrow))
        {
            xVelocity = speed;
        } else { xVelocity = 0; }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            yVelocity = -speed;
        } else if (Input.GetKey(KeyCode.UpArrow))
        {
            yVelocity = speed;
        }
        else
        {
            yVelocity = 0;
        }

        rb2d.velocity = new Vector2(xVelocity, yVelocity);
    }
}
