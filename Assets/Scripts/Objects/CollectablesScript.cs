using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CollectablesScript : MonoBehaviour
{
    private TextMeshProUGUI livesText, coinText, scoreText;
    [SerializeField]
    private Canvas UICanvas;
    private GameObject player;
    private PlayerMovement playerMove;
    private PlayerController playerCont;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMove = player.GetComponent<PlayerMovement>();
        playerCont = player.GetComponent<PlayerController>();
        livesText = UICanvas.gameObject.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        coinText = UICanvas.gameObject.transform.GetChild(3).GetComponent<TextMeshProUGUI>();
        scoreText = UICanvas.gameObject.transform.GetChild(4).GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        livesText.text = playerCont.lives.ToString();
        coinText.text = playerCont.coins.ToString();
        scoreText.text = playerCont.score.ToString();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Collectable")
        {
            if (collision.gameObject.name.Contains("Coin")) playerCont.coins++;
            else if (collision.gameObject.name.Contains("Life")) playerCont.lives++;
            else if (collision.gameObject.name.Contains("Double")) playerMove.jumpPower = true;
            else if (collision.gameObject.name.Contains("Mitt")) playerMove.firePower = true;
            playerCont.score += 200;



            Destroy(collision.gameObject);
        }
    }
}
