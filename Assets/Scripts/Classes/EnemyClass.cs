using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyClass : MonoBehaviour
{
    protected BoxCollider2D enemyCollide;
    protected LayerMask groundLayerMask;
    protected Rigidbody2D enemyRigid;
    protected SpriteRenderer enemyRender;
    protected Animator enemyAnim;

    protected float enemySpeed;
    protected int enemyXDir;
    public int enemyHP, enemyDamage;

    protected void BasicEnemyMovement()
    {
        enemyRigid.velocity = new Vector2(enemySpeed * enemyXDir, enemyRigid.velocity.y);
    }

    protected void CheckTurn()
    {
        RaycastHit2D leftCast = Physics2D.Raycast(
           enemyCollide.bounds.center - new Vector3(enemyCollide.bounds.extents.x, 0)
           , Vector2.down, enemyCollide.bounds.extents.y + 0.1f, groundLayerMask);

        RaycastHit2D rightCast = Physics2D.Raycast(
            enemyCollide.bounds.center + new Vector3(enemyCollide.bounds.extents.x, 0)
            , Vector2.down, enemyCollide.bounds.extents.y + 0.1f, groundLayerMask);

        RaycastHit2D sideHit = Physics2D.Raycast(
            enemyCollide.bounds.center,
            new Vector2(enemyXDir, 0), enemyCollide.bounds.extents.x + 0.1f, groundLayerMask);

        if (leftCast.collider == null)
        {
            enemyRender.flipX = true;
            enemyXDir = 1;
        }

        else if (rightCast.collider == null)
        {
            enemyRender.flipX = false;
            enemyXDir = -1;
        }

        if (sideHit.collider !=null)
        {
            enemyRender.flipX = !enemyRender.flipX;
            enemyXDir *= -1;
        }
    }

    protected void BasicTurn()
    {
        RaycastHit2D sideHit = Physics2D.Raycast(
            enemyCollide.bounds.center,
            new Vector2(enemyXDir, 0), enemyCollide.bounds.extents.x + 0.1f, groundLayerMask);

        if (sideHit.collider != null)
        {
            enemyRender.flipX = !enemyRender.flipX;
            enemyXDir *= -1;
        }
    }
}