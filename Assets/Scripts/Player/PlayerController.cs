using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float speed, horizontal, vertical, jumpModifier;
    private bool canDoubleJump;

    private Rigidbody2D playerRigid;
    private BoxCollider2D playerBox;
    private LayerMask groundLayerMask;
    private SpriteRenderer playerRender;

    private Animator anim;

    void Start()
    {
        speed = 4;
        jumpModifier = 9.85f;
        canDoubleJump = true;

        playerRigid = this.GetComponent<Rigidbody2D>();
        playerBox = this.GetComponent<BoxCollider2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
        playerRender = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        vertical = Input.GetAxis("Vertical");
        horizontal = Input.GetAxis("Horizontal");
        jumpMovement();
        groundedMovement();
    }

    private void jumpMovement()
    {

        if ((Input.GetButtonDown("Jump") && PlayerGrounded() == true) ||
            (Input.GetButtonDown("Jump") && PlayerGrounded() == false && canDoubleJump == true))
        {
            Vector2 jumpMovement = new Vector2(playerRigid.velocity.x, jumpModifier);
            playerRigid.velocity = jumpMovement;
            ToggleAnimState("Jumping");

            if (canDoubleJump == true)
            {
                canDoubleJump = false;
            }
        }

        if (PlayerGrounded() == true)
        {
            canDoubleJump = true;
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

            else if (Input.GetButtonDown("Fire1") == true) //check for firing state
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