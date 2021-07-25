using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerMovement : MonoBehaviour
{
    private float speed, horizontal, vertical, jumpModifier, fireTimer, wallJumpTimer, previousPlayerDir;
    private bool canDoubleJump, isWallSliding, canWallJump;

    private Rigidbody2D playerRigid;
    private BoxCollider2D playerBox;
    private LayerMask groundLayerMask;
    private SpriteRenderer playerRender;

    private Animator anim;

    [SerializeField]private GameObject fireBall;


    void Start()
    {
        speed = 4;
        jumpModifier = 9.85f;
        previousPlayerDir = 0;
        canDoubleJump = true;
        isWallSliding = false;

        playerRigid = this.GetComponent<Rigidbody2D>();
        playerBox = this.GetComponent<BoxCollider2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
        playerRender = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        fireTimer = 0.85f;
        wallJumpTimer = 0.3f;
    }

    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        WallMovement();
        jumpMovement();
        groundedMovement();
        FireCheck();

        if (fireTimer < 0.85f)
        {
            fireTimer += Time.deltaTime;
        }

        if (wallJumpTimer < 0.35f)
        {
            wallJumpTimer += Time.deltaTime;
        }
    }

    private void jumpMovement()
    {

        if (Input.GetButtonDown("Jump") && 
            (PlayerGrounded() == true ||
            (PlayerGrounded() == false && canDoubleJump == true) || 
            canWallJump == true))
        {
            playerJump();
        }

        if (PlayerGrounded() == true)
        {
            canDoubleJump = true;
        }

        if (canWallJump == true)
        {
            canWallJump = false;
            wallJumpTimer = 0.0f;
        }
    }

    public void playerJump()
    {
        Vector2 jumpMovement = new Vector2(playerRigid.velocity.x, jumpModifier);
        playerRigid.velocity = jumpMovement;
        ToggleAnimState("Jumping");

        if (canDoubleJump == true)
        {
            canDoubleJump = false;
        }
    }

    private bool PlayerGrounded()
    {
        RaycastHit2D checkIfGrounded = Physics2D.BoxCast(playerBox.bounds.center, playerBox.bounds.size,
            0, Vector2.down, 0.1f, groundLayerMask);
        return (checkIfGrounded.collider != null);
    }

    private void groundedMovement()
    {
        if (Mathf.Abs(horizontal) > 0)
        {
            switch (Mathf.Sign(horizontal))
            {
                case 1:
                    playerRender.flipX = false;
                    break;

                case -1:
                    playerRender.flipX = true;
                    break;

            }

            if (PlayerGrounded() == true)
            {
                ToggleAnimState("Running");
            }
            else ToggleAnimState("Jumping");
        }



        else if (PlayerGrounded() == true && horizontal == 0) //if stationary and grounded
        {
            if (vertical < 0) //check for duck state
            {
                ToggleAnimState("Ducking");
            }

            else if (Input.GetButtonDown("Fire1")) //check for firing state
            {
                ToggleAnimState("Firing");
            }

            else if (vertical == 0 && Input.GetButtonDown("Fire1") == false)
            {
                ToggleAnimState("Idle");
            }
        }
        
        switch (Input.GetButton("Run"))
        {
            case true:
                speed = 8.5f;
                anim.speed = 1.5f;
                break;

            case false:
                speed = 5.0f;
                anim.speed = 1.0f;
                break;
        }

        Vector2 verticalMovement = new Vector2(horizontal * speed, playerRigid.velocity.y);
        playerRigid.velocity = verticalMovement;
    }

    private void WallMovement()
    {
        int playerXDir = 0;

        switch (Convert.ToInt32(GetComponent<SpriteRenderer>().flipX))
        {
            case 0:
                playerXDir = 1;
                break;
            case 1:
                playerXDir = -1;
                break;
        }

        RaycastHit2D sideHit = Physics2D.Raycast(
            playerBox.bounds.center,
            new Vector2(playerXDir, 0), playerBox.bounds.extents.x + 0.025f, groundLayerMask);


        if (PlayerGrounded() == false)
        {
            if (sideHit.collider != null)
            {
                playerRigid.velocity = new Vector2(playerRigid.velocity.x, Mathf.Clamp(playerRigid.velocity.y, -1.5f, float.MaxValue));


                if (Input.GetButtonDown("Jump"))
                {
                    float currentDir = playerXDir;
                    if (previousPlayerDir != currentDir && wallJumpTimer > 0.35f)
                    {
                        canWallJump = true;
                    }
                    else canWallJump = false;

                    previousPlayerDir = currentDir;
                }
            }
        }

        else previousPlayerDir = 0;
    }

    private void FireCheck()
    {
        if (fireTimer >= 0.85f && Input.GetButtonDown("Fire1"))
        {
            int fireXDir = 0;

            switch (Convert.ToInt32(playerRender.flipX))
            {
                case 0:
                    fireXDir = 1;
                    break;

                case 1:
                    fireXDir = -1;
                    break;
            }

            Vector2 instanPos = new Vector2
                (transform.position.x + (0.33f * fireXDir), transform.position.y);
            Instantiate(fireBall, instanPos, Quaternion.Euler(0,0,0));
            fireTimer = 0.0f;
        }
    }

    private void ToggleAnimState(string animState)
    {
        anim.SetBool("Idle", false);
        anim.SetBool("Jumping", false);
        anim.SetBool("Running", false);
        anim.SetBool("Halting", false);
        anim.SetBool("Ducking", false);
        anim.SetBool("Firing", false);
        anim.SetBool(animState, true);
    }
}