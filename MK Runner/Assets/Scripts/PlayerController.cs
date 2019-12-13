using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Controls the player
    public float playerSpeed, playerJump;
    public LayerMask groundLayer;

    public bool canJump = true;
    private Rigidbody2D playerRB;
    private Collider2D playerCollider;

    

    private void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        playerRB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Touch Controls
         
        /* if (Input.touchCount > 0)
         {
             Jump();


         }*/

        //Can jump if touching a ground object
        

        if (Input.GetMouseButtonDown(0) && canJump )
        {
            Jump();

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (groundLayer == (groundLayer | (1 << collision.collider.gameObject.layer)))
        {
            if (!canJump && Vector2.Dot(collision.GetContact(0).point, (transform.position+Vector3.down)) >= .35f)
            {
                canJump = true;
            }
        }
    }


    private void Die()
    {
        GameEvents.InvokePlayerDeath();
    }

    private void Jump()
    {
        playerRB.velocity += new Vector2(0, playerJump);
        canJump = false;
    }
}
