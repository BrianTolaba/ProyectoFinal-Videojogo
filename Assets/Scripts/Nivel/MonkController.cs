using UnityEngine;

public class MonkController : MonoBehaviour
{
    [SerializeField] float detectionRadius = 5.0f;
    [SerializeField] int cantidadCuracion = 1;       // cuánto cura cada vez
    [SerializeField] int cantidadMejoraSalud = 5;    // cuánto aumenta la vida máxima
    [SerializeField] int costoMejora = 2;            // costo de monedas para el aumenta de vida máxima
    [SerializeField] OtherSoundController OtherSoundController;

    private Transform player;
    private bool curando;
    private Animator animator;
    private PlayerControler playerControler;
    private SpriteRenderer spriteRenderer;
    private Collider2D monkCollider;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        monkCollider = GetComponent<Collider2D>();

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
        // Buscar enemigos en rango
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        bool enemyNearby = false;
        foreach (var col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                enemyNearby = true;
                break;
            }
        }

        if (enemyNearby)
        {
            // Monk se "apaga" visualmente y deja de curar
            curando = false;
            animator.SetBool("Curando", false);
            spriteRenderer.enabled = false;   // oculta sprite
            monkCollider.enabled = false;     // desactiva colisiones si quieres
            return;
        }
        else
        {
            // Monk reaparece cuando no hay enemigos
            spriteRenderer.enabled = true;
            monkCollider.enabled = true;
        }

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
                playerControler.money -= costoMejora;
                OtherSoundController.PlayUpVidaSound();  //sonido del upgrade de vida
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
            OtherSoundController.PlayCuraSound();  // sonido de la cura
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

