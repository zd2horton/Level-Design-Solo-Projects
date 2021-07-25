using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScript : MonoBehaviour
{
    [SerializeField] private GameObject blockContents;
    private bool blockHit;

    void Start()
    {
        blockHit = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && blockHit == false)
        {
            float posToInstan = transform.position.y + GetComponent<BoxCollider2D>().bounds.extents.y + 0.5f;
            Instantiate(blockContents,
                new Vector2(transform.position.x, posToInstan), Quaternion.identity);
            blockHit = true;
        }
    }
}
