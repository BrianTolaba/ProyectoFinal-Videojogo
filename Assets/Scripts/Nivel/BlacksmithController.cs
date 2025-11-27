using UnityEngine;

public class BlacksmithController : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] float detectionRadius = 1.5f;
    [SerializeField] int cantidadMejoraDanio = 1;
    [SerializeField] int costoMejora = 2;

    [Header("Referencias")]
    [SerializeField] OtherSoundController OtherSoundController;

    private Transform player;
    private PlayerControler playerControler;
    private Animator animator;

    private bool mejorando;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (OtherSoundController == null)
        {
            OtherSoundController = FindObjectOfType<OtherSoundController>();
        }
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerControler = playerObj.GetComponent<PlayerControler>();
        }
    }

    private void Update()
    {
        if (playerControler == null || playerControler.muerto) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < detectionRadius)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                // Detectar input para mejorar
                if (Input.GetKeyDown(KeyCode.E))
                {
                    IntentarMejorar();
                }
            }
        }
        else
        {
            mejorando = false;
        }
        animator.SetBool("Mejorando", mejorando);
    }

    private void IntentarMejorar()
    {
        if (playerControler.money >= costoMejora)
        {
            // Cobrar y Mejorar
            playerControler.money -= costoMejora;
            playerControler.MejorarDanio(cantidadMejoraDanio);

            // Activar Animación y Estado
            mejorando = true;
            animator.SetBool("Mejorando", true);
        }
        else
        {
            Debug.Log("No tienes suficiente dinero.");
        }
    }

    public void TerminaAnim()
    {
        mejorando = false;
        animator.SetBool("Mejorando", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
