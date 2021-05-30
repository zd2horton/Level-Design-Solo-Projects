using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private enum AnimState
    {
        IDLE,
        JUMPING,
        RUNNING,
        HALTING,
        DUCKING,
        FIRING,
    }

    private float speed, horizontal, vertical, jumpModifier;
    private bool canDoubleJump;

    private Rigidbody2D playerRigid;
    private BoxCollider2D playerBox;
    private LayerMask groundLayerMask;
    private SpriteRenderer playerRender;

    private Animator anim;
    private AnimState playerState;

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
        //CheckState();
        Debug.Log(anim.GetBool("Running"));
    }

    private void jumpMovement()
    {

        if ((Input.GetButtonDown("Jump") && PlayerGrounded() == true) ||
            (Input.GetButtonDown("Jump") && PlayerGrounded() == false && canDoubleJump == true))
        {
            Vector2 jumpMovement = new Vector2(playerRigid.velocity.x, jumpModifier);
            playerRigid.velocity = jumpMovement;
            //playerState = AnimState.JUMPING;
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
        //RaycastHit2D checkIfGrounded = Physics2D.Raycast(playerBox.bounds.center, Vector2.down,
        //    playerBox.bounds.extents.y + 0.1f, groundLayerMask);
        RaycastHit2D checkIfGrounded = Physics2D.BoxCast(playerBox.bounds.center, playerBox.bounds.size,
            0, Vector2.down, 0.1f, groundLayerMask);
        return (checkIfGrounded.collider != null);
    }

    private void groundedMovement()
    {
        if (horizontal > 0)
        {
            playerRender.flipX = false;

            if (PlayerGrounded() == true)
            {
                //playerState = AnimState.RUNNING;
                ToggleAnimState("Running");
            }
            else ToggleAnimState("Jumping");
        }

        else if (horizontal < 0)
        {
            playerRender.flipX = true;

            if (PlayerGrounded() == true)
            {
                //playerState = AnimState.RUNNING;
                ToggleAnimState("Running");
            }
            else ToggleAnimState("Jumping");
        }

        if (Input.GetButton("Run"))
        {
            speed = 8.5f;

            //if (playerState == AnimState.RUNNING)
            //{
            //    anim.speed = 2.0f;
            //}
        }

        else
        {
            speed = 5.0f;
            anim.speed = 1.0f;
        }
        
        Vector2 verticalMovement = new Vector2(horizontal * speed, playerRigid.velocity.y);
        playerRigid.velocity = verticalMovement;
    }

    //private void CheckState()
    //{
    //    switch (playerState)
    //    {
    //        case AnimState.IDLE:
    //            ToggleAnimState("Idle");
    //            break;

    //        case AnimState.JUMPING:
    //            ToggleAnimState("Jumping");
    //            break;

    //        case AnimState.RUNNING:
    //            ToggleAnimState("Running");
    //            break;

    //        case AnimState.HALTING:
    //            ToggleAnimState("Halting");
    //            break;

    //        case AnimState.DUCKING:
    //            ToggleAnimState("Ducking");
    //            break;

    //        case AnimState.FIRING:
    //            ToggleAnimState("Firing");
    //            break;
    //    }
    //}

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