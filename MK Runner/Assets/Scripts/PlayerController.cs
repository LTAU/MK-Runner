using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //Controls the player
    public float playerSpeed, playerJumpVelocity, playerJumpDistance;
    public LayerMask groundLayer;
    public LayerMask deathLayer;
    public LayerMask itemLayer;

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
        GameEvents.OnSpeedIncrease += OnSpeedChange;
        startPosition = transform.position;
    }

    private void OnGameStart()
    {

        
        jumpCooldown = 0;
        transform.position = startPosition;
        playerRB.simulated = true;
        playerAnim.Play("Run");
        alive = true;
    }

    private void OnSpeedChange()
    {
        playerAnim.SetFloat("Speed", GameManager.singleton.currentGameSpeed);
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
                playerAnim.SetBool("isJumping", true);


            }

            //Continue jumping for up to 1/4 a second
            if (isJumping )
            {
                if (jumpCooldown <= playerJumpDistance)
                {
                    playerRB.velocity = new Vector2(playerRB.velocity.x, playerJumpVelocity);
                    jumpCooldown += Time.deltaTime;
                }
                else
                {
                    CompleteJump();
                }
            }
           
            //Stop jumping if click is released
            if (Input.GetMouseButtonUp(0))
            {

                CompleteJump();

            }

            //if playercharacter is out of position, aim to move back into position
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

    private void CompleteJump()
    {
        isJumping = false;
        playerAnim.SetBool("isJumping", false);

        jumpCooldown = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Check if player hit ground
        if (groundLayer == (groundLayer | (1 << collision.collider.gameObject.layer)))
        {
            if (!canJump && Vector2.Dot(collision.GetContact(0).point, (transform.position + Vector3.down)) >= .8f)
            {
                canJump = true;
            }
        }

        //check if player hit something that should kill them
        if (deathLayer == (deathLayer | (1 << collision.collider.gameObject.layer)))
        {
            Die();

        }

        if(itemLayer == (itemLayer | (1 << collision.collider.gameObject.layer)))
        {
            collision.collider.GetComponent<SpawnableObjectBase>().Despawn();
            ItemPickup();
        }

    }

    //Item pickup give random effect
    private void ItemPickup()
    {
        int i = Random.Range(0, 10);
        switch (i)
        {
            case 0:
            case 1:
            case 2:
            case 3:
            case 4:
                GameEvents.InvokeSpeedIncrease();
                break;

            

            case 5:
                GameEvents.InvokeMaxSpeedChange(-1);
                GameEvents.InvokeOnInfoText("Max Speed Increased!");

                break;

            case 6:
            case 7:
                playerJumpDistance +=.1f;
                GameEvents.InvokeOnInfoText("Jump Duration Increased!");

                break;

            case 8:
            case 9:
                playerJumpVelocity += .5f;
                GameEvents.InvokeOnInfoText("Jump Velocity Increased!");

                break;

            

                

            default:
                break;
        }
    }


    //player death
    private void Die()
    {
        alive = false;
        GameEvents.InvokePlayerDeath();
       

        playerAnim.SetTrigger("Death");

        playerRB.simulated = false;
        playerRB.SetRotation(90f);
        GameEvents.InvokeOnInfoText("You Died!");



    }


}
