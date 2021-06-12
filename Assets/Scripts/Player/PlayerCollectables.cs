using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCollectables : MonoBehaviour
{
    private int lives, coins;
    private TextMeshProUGUI livesText, coinText;
    [SerializeField]
    private Canvas UICanvas;
    void Start()
    {
        coins = lives = 0;
        livesText = UICanvas.gameObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
        coinText = UICanvas.gameObject.transform.GetChild(5).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            switch (collision.transform.parent.gameObject.name)
            {
                case "Lives":
                    lives++;
                    livesText.text = lives.ToString();
                    break;

                case "Coins":
                    coins++;
                    coinText.text = coins.ToString();
                    break;
            }
            Destroy(collision.gameObject);
        }
    }

    private void OnGUI()
    {

    }
}
