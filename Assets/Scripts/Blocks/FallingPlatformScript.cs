using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FallingPlatformScript : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D platformRigid;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        platformRigid = GetComponent<Rigidbody2D>();
        platformRigid.gravityScale = 0.0f;
        platformRigid.freezeRotation = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.GetContact(0).normal.y == 1.0f)
            {
                platformRigid.gravityScale = 0.5f;
            }
        }

    }
}
