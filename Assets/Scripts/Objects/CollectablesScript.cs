using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectablesScript : MonoBehaviour
{
    //[HideInInspector]
    public int lives, coins;
    private TextMeshProUGUI livesText, coinText;
    [SerializeField]
    private Canvas UICanvas;
    private PlayerMovement playerMove;
    void Start()
    {
        coins = lives = 0;
        playerMove = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        livesText = UICanvas.gameObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        coinText = UICanvas.gameObject.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        livesText.text = lives.ToString();
        coinText.text = coins.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            if (collision.gameObject.name.Contains("Coin")) coins++;
            else if (collision.gameObject.name.Contains("Life")) lives++;
            else if (collision.gameObject.name.Contains("Double")) playerMove.jumpPower = true;
            else if (collision.gameObject.name.Contains("Mitt")) playerMove.firePower = true;

            Destroy(collision.gameObject);
        }
    }
}
