using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FireballTravel : MonoBehaviour
{
    private float speed, HPDamage, ballLife;
    private Rigidbody2D ballRigid;
    private GameObject player;
    Vector2 currentVeloVector, newVeloVector, 
        normalisedCurrent, normalisedSurface;

    private void OnEnable()
    {
        speed = 6.5f;
        HPDamage = 3.0f;
        ballLife = 0.0f;
        ballRigid = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");


        switch (Convert.ToInt32(player.GetComponent<SpriteRenderer>().flipX))
        {
            case 0:
                ballRigid.velocity = new Vector2(1, -1);
                break;

            case 1:
                ballRigid.velocity = new Vector2(-1, -1);
                break;
        }
    }

    void Update()
    {
        currentVeloVector = ballRigid.velocity;
        ballLife += Time.deltaTime;

        if (ballLife > 4.13f)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            normalisedCurrent = currentVeloVector.normalized;
            normalisedSurface = collision.GetContact(0).normal;
            newVeloVector = Vector2.Reflect(normalisedCurrent, normalisedSurface);
            ballRigid.velocity = newVeloVector * speed;
        }
        //Debug.Log(collision.gameObject.layer);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
    }
}
