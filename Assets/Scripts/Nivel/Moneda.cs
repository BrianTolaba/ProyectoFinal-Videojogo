using UnityEngine;

public class Moneda : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private OtherSoundController OtherSoundController;
    [SerializeField] private Animator animator;
    [SerializeField] private int valor = 1;
    private bool recogido;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (recogido) return;                  // Evita si el jugador pasa muy rápido, cobre doble
        if (collision.CompareTag("Player"))
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            if (playerControler != null)
            {
                RecogerMoneda(playerControler);
            }
            
        }
    }

    private void RecogerMoneda(PlayerControler player)
    {
        recogido = true;        // Bloqueamos la moneda para que no se pueda volver a tocar
        player.money += valor;  // Suma dinero
        // Sonido
        if (OtherSoundController != null)
        {
            OtherSoundController.PlayDineroSound();
        }
        // Animación
        if (animator != null)
        {
            animator.SetBool("Recogido", true);
        }
    }

    public void DeleteObj()
    {
        Destroy(gameObject);
    }
}
