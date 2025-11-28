using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image[] hearts;
    [SerializeField] private Sprite fullHeart;
    [SerializeField] private Sprite emptyHeart;

    private PlayerControler playerControler;

    private void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerControler = playerObj.GetComponent<PlayerControler>();
        }
    }

    private void Update()
    {
        if (playerControler.vida > playerControler.vidaMaxima)
        {
            playerControler.vida = playerControler.vidaMaxima;
        }

        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < playerControler.vida)
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = emptyHeart;
            }
            if (i < playerControler.vidaMaxima)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }
        }
    }
}
