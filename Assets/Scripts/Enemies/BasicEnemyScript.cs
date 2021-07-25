using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemyScript : EnemyClass
{

    [SerializeField] private Sprite defeatedSprite;
    void Start()
    {
        enemyCollide = GetComponent<BoxCollider2D>();
        groundLayerMask = LayerMask.GetMask("Ground");
        enemyRigid = GetComponent<Rigidbody2D>();
        enemyRigid.velocity = new Vector2(-1.0f, 0.0f);
        enemyRender = GetComponent<SpriteRenderer>();
        enemyAnim = GetComponent<Animator>();

        enemySpeed = 2.0f;
        enemyHP = 1;
        enemyXDir = -1;
        enemyDamage = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyHP > 0)
        {
            BasicEnemyMovement();
        }

        else
        {
            StartCoroutine("Defeated");
        }
    }

    private IEnumerator Defeated()
    {
        enemyRigid.velocity = Vector2.zero;
        enemyRender.sprite = defeatedSprite;
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        yield return new WaitForSeconds(1.5f);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        Destroy(gameObject);
    }
}