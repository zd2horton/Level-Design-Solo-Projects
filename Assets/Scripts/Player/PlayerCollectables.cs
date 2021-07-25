using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerCollectables : MonoBehaviour
{
    //[HideInInspector]
    public int lives, coins;
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
        livesText.text = lives.ToString();
        coinText.text = coins.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            if (collision.gameObject.name.Contains("Coin"))
            {
                coins++;
                Destroy(collision.gameObject);
            }

            else if (collision.gameObject.name.Contains("Life"))
            {
                lives++;
                Destroy(collision.gameObject);
            }

            else
            {
                switch (collision.transform.parent.gameObject.name)
                {
                    case "Lives":
                        lives++;
                        break;

                    case "Coins":
                        coins++;
                        break;

                    default:
                        break;
                }
                Destroy(collision.gameObject);
            }
        }
    }

    private void OnGUI()
    {

    }
}
