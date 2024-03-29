﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private int health;
    private float invincibleTimer;
    private bool invinciblePeriod;
    public int score, lives, coins;

    private Vector2 initialPos;
    private SpriteRenderer playerRender;
    private Animator anim;

    void Start()
    {
        health = 3;
        invincibleTimer = 3;
        invinciblePeriod = false;
        score = 0;
        lives = 0;
        coins = 0;


        initialPos = transform.position;
        playerRender = GetComponent<SpriteRenderer>();
        playerRender.color = Color.green;
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (invinciblePeriod)
        {
            invincibleTimer += Time.deltaTime;
        }

        if (invincibleTimer >= 3)
        {
            invinciblePeriod = false;
            invincibleTimer = 0;
            anim.SetLayerWeight(1, 0);
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        }
        //Debug.Log(health);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MenuScene");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Hurtbox")
        {
            Destroy(collision.gameObject.transform.parent);
        }

        else if (collision.gameObject.tag == "Enemy" && invinciblePeriod == false)
        {
            Debug.DrawLine(collision.GetContact(0).point, collision.GetContact(0).point + collision.GetContact(0).normal, Color.black, 10);
            //Debug.Log(collision.GetContact(0).normal);

            if (collision.GetContact(0).normal.y != 1.0f)
            {
                health -= collision.gameObject.GetComponent<EnemyClass>().enemyDamage;
                HealthHandler();
                invinciblePeriod = true;
                anim.SetLayerWeight(1, 1);
                Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
            }

            else
            {
                collision.gameObject.GetComponent<EnemyClass>().enemyHP--;
                GetComponent<PlayerMovement>().playerJump();
                score += 250;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "DeathPlane")
        {
            health = 0;
            HealthHandler();
        }
    }

    private void HealthHandler()
    {
        switch (health)
        {

            case -1:
                transform.position = initialPos;
                lives--;
                health = 3;
                playerRender.color = Color.green;
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;

            case 0:
                transform.position = initialPos;
                lives--;
                health = 3;
                playerRender.color = Color.green;
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;

            case 1:
                playerRender.color = Color.red;
                break;

            case 2:
                playerRender.color = Color.yellow;
                break;
        }
    }
}