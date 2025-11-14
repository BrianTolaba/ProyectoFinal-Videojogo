using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Image fillHealthBar;
    private PlayerControler playerControler;
    private float vidaMaxima;
    private void Start()
    {
      playerControler = GameObject.Find("Player").GetComponent<PlayerControler>();
        vidaMaxima = playerControler.vida;
    }
    void Update()
    {
        fillHealthBar.fillAmount = playerControler.vida / vidaMaxima;
    }
}
