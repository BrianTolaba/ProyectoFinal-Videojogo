using UnityEngine;

public class MonkController : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] float detectionRadius = 5.0f;
    [SerializeField] int cantidadCuracion = 1;

    [Header("Mejora de Vida")]
    [SerializeField] int cantidadMejoraSalud = 1;
    [SerializeField] int costoMejora = 4;
    [SerializeField] int maximaMejora = 20;

    [Header("Referencias")]
    [SerializeField] OtherSoundController OtherSoundController;

    private Transform player;
    private PlayerControler playerControler;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Collider2D monkCollider;

    private bool curando;
    private bool hayEnemigosCerca;
    private float timerBusquedaEnemigos;

    void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        monkCollider = GetComponent<Collider2D>();

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

    void Update()
    {
        if (playerControler == null || playerControler.muerto) return;
        timerBusquedaEnemigos += Time.deltaTime;
        if (timerBusquedaEnemigos >= 0.25f)
        {
            BuscarEnemigos();
            timerBusquedaEnemigos = 0f;
        }

        // Comportamiento segun entorno
        if (hayEnemigosCerca)
        {
            CambiarEstadoVisible(false); // Esconderse
        }
        else
        {
            CambiarEstadoVisible(true);  // Aparecer
            GestionarInteraccionJugador();
        }

        // Animators
        animator.SetBool("Curando", curando);
    }

    private void BuscarEnemigos()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);
        hayEnemigosCerca = false;

        foreach (var col in colliders)
        {
            if (col.CompareTag("Enemy"))
            {
                hayEnemigosCerca = true;
                break;
            }
        }
    }

    private void CambiarEstadoVisible(bool visible)
    {
        if (spriteRenderer.enabled != visible)
        {
            spriteRenderer.enabled = visible;
            monkCollider.enabled = visible;

            if (!visible)
            {
                curando = false;                    // Si se esconde y deja de curar
            }
        }
    }

    private void GestionarInteraccionJugador()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            // Curacion automatica
            if (playerControler.vida < playerControler.vidaMaxima)
            {
                curando = true;
            }
            else
            {
                curando = false;
            }

            // Mejora
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (playerControler.money >= costoMejora && playerControler.vidaMaxima < maximaMejora)
                {
                    playerControler.money -= costoMejora;
                    playerControler.MejorarSalud(cantidadMejoraSalud);
                    curando = true;
                    if (OtherSoundController != null)
                    {
                        OtherSoundController.PlayUpVidaSound();
                    }
                }
                else
                {
                    OtherSoundController.PlayNoDineroSound();
                    Debug.Log("No tienes suficiente dinero");
                }
            }
        }
        else
        {
            curando = false;
        }
    }

    public void IniciaCuracion()
    {
        if (playerControler != null && !playerControler.muerto)
        {
            // Solo aplicamos la cura si realmente le falta vida
            if (playerControler.vida < playerControler.vidaMaxima)
            {
                playerControler.vida += cantidadCuracion;
                // Aseguramos que no se pase del máximo
                playerControler.vida = Mathf.Clamp(playerControler.vida, 0, playerControler.vidaMaxima);
                if (OtherSoundController != null)
                {
                    OtherSoundController.PlayCuraSound();
                }
            }
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

