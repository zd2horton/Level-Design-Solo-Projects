using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockScriptVariable : MonoBehaviour
{
    [SerializeField] private GameObject[] blockContents;
    private bool blockHit;
    private int randNo;

    void Start()
    {
        blockHit = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && blockHit == false && collision.GetContact(0).normal.y == 1.0f)
        {
            randNo = Random.Range(0, 4);
            float posToInstan = transform.position.y + GetComponent<BoxCollider2D>().bounds.extents.y + 0.5f;
            Instantiate(blockContents[randNo],
                new Vector2(transform.position.x, posToInstan), Quaternion.identity);
            blockHit = true;
        }
    }
}
