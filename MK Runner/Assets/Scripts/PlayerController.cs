﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Controls the player
    public float playerSpeed, playerJumpVelocity;
    public LayerMask groundLayer;
    public LayerMask deathLayer;

    public bool canJump = false;
    public bool alive = false;
    public bool isJumping = false;


    private Rigidbody2D playerRB;
    private Collider2D playerCollider;
    private Animator playerAnim;
    private float jumpCooldown = 0;
    private Vector3 startPosition;


    private void Start()
    {
        playerCollider = GetComponent<Collider2D>();
        playerRB = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        GameEvents.OnGameStart += OnGameStart;
        startPosition = transform.position;
    }

    private void OnGameStart()
    {

        
        jumpCooldown = 0;
        transform.position = startPosition;
        playerRB.simulated = true;

        alive = true;
    }


    private void Update()
    {
        if (alive)
        {
            //Touch Controls

            /* if (Input.touchCount > 0)
             {
                 Jump();


             }*/

            //Jump on mouse click and if able
            if (Input.GetMouseButtonDown(0) && canJump)
            {
                isJumping = true;
                canJump = false;
                playerRB.velocity = new Vector2(0, playerJumpVelocity);



            }

            if (isJumping )
            {
                if (jumpCooldown <= .25f)
                {
                    playerRB.velocity = new Vector2(playerRB.velocity.x, playerJumpVelocity);
                    jumpCooldown += Time.deltaTime;
                }
                else
                {
                    isJumping = false;
                    jumpCooldown = 0;
                }
            }
           

            if (Input.GetMouseButtonUp(0))
            {
               
                isJumping = false;
                jumpCooldown = 0;

            }

            if (transform.position.x != startPosition.x)
            {
               
                playerRB.velocity = new Vector2((startPosition.x-transform.position.x), playerRB.velocity.y);
            }
            

            //If player out of bounds, die
            if (transform.position.y<-5 || transform.position.x < -5)
            {
                Die();
            }
        }
       
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if player hit ground
        if (groundLayer == (groundLayer | (1 << collision.collider.gameObject.layer)))
        {
            if (!canJump && Vector2.Dot(collision.GetContact(0).point, (transform.position + Vector3.down)) >= .35f)
            {
                canJump = true;
            }
        }

        //check if player hit something that should kill them
        if (deathLayer == (deathLayer | (1 << collision.collider.gameObject.layer)))
        {
            Die();

        }

    }

    //player death
    private void Die()
    {
        alive = false;
        GameEvents.InvokePlayerDeath();
        playerRB.SetRotation(90f);
        playerRB.simulated = false;
        playerAnim.SetTrigger("Death");
       

    }

   
}
