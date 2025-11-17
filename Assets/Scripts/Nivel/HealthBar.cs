using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Image fillHealthBar;
    private PlayerControler playerControler;
    
    private void Start()
    {
      playerControler = GameObject.Find("Player").GetComponent<PlayerControler>();
        
        playerControler.vida = playerControler.vidaMaxima; //esto inicializa y sobreescribe la vida del jugador al iniciar por la vidaMaxima del jugador
    }
    void Update()
    {
        //fillHealthBar.fillAmount = playerControler.vida / playerControler.vidaMaxima;
        fillHealthBar.fillAmount = (float)playerControler.vida / playerControler.vidaMaxima;

    }
}
