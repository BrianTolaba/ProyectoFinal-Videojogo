using UnityEngine;

public class MonkController : MonoBehaviour
{
    public float detectionRadius = 5.0f;
    public int cantidadCuracion = 1;       // cuánto cura cada vez
    public int cantidadMejoraSalud = 5;    // cuánto aumenta la vida máxima

    private Transform player;
    private bool curando;
    private Animator animator;
    private PlayerControler playerControler;

    void Start()
    {
        animator = GetComponent<Animator>();

        // Buscar al Player por tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerControler = playerObj.GetComponent<PlayerControler>();
        }
    }

    void Update()
    {
        if (playerControler == null || playerControler.muerto) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        // Si el jugador está en rango
        if (distanceToPlayer < detectionRadius)
        {
            // Si tiene menos vida que el máximo, activar curación
            if (playerControler.vida < playerControler.vidaMaxima)
                curando = true;
            else
                curando = false;

            // Si presiona E, dar mejora de salud
            if (Input.GetKeyDown(KeyCode.E)&&playerControler.money >=1)
            {
                playerControler.MejorarSalud(cantidadMejoraSalud);
                curando = true; // Inicia la animación de curar
                playerControler.money -= 1;

            }
        }
        else
        {
            curando = false;
        }

        // Animators
        animator.SetBool("Curando", curando);
    }

    // Animation Event al inicio del clip de curar
    public void IniciaCuracion()
    {
        if (playerControler != null && !playerControler.muerto)
        {
            playerControler.vida += cantidadCuracion;
            playerControler.vida = Mathf.Clamp(playerControler.vida, 0, playerControler.vidaMaxima);
        }
    }

    // Animation Event al final del clip
    public void TerminaAnim()
    {
        curando = false;
        animator.SetBool("Curando", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

