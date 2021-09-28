using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleScript : EnemyClass
{
    void Start()
    {
        enemyCollide = GetComponent<BoxCollider2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
        enemyRigid = GetComponent<Rigidbody2D>();
        enemyRender = GetComponent<SpriteRenderer>();
        enemyAnim = GetComponent<Animator>();

        enemySpeed = 2.0f;
        enemyHP = 2;
        enemyXDir = -1;
        enemyDamage = 1;

        enemyRigid.velocity = new Vector2(-1, 0);
    }

    void Update()
    {
        if (enemyHP > 0)
        {
            BasicEnemyMovement();
            CheckTurn();
        }

        else
        {
            enemyRigid.velocity = Vector2.zero;
            Explosion();
        }
    }

    private void Explosion()
    {
        enemyAnim.SetBool("ShellMode", true);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);

        if (enemyAnim.GetCurrentAnimatorStateInfo(0).IsName("TurtleExplode"))
        {
            Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
            enemyDamage = 2;

            if (enemyAnim.GetCurrentAnimatorStateInfo(0).length < enemyAnim.GetCurrentAnimatorStateInfo(0).normalizedTime)
            {
                Destroy(gameObject);
            }
        }
    }
}