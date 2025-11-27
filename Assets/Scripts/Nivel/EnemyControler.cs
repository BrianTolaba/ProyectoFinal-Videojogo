using System.Collections;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    [Header("Configuracion de Estadísticas")]
    [SerializeField] private int vida = 3;
    [SerializeField] private float speed = 2.0f;
    [SerializeField] private int danioAntorcha = 1;
    [SerializeField] private float fuerzaRebote = 2f;

    [Header("Combate e IA")]
    [SerializeField] private float detectionRadius = 5.0f;
    [SerializeField] private float rangoAtaque = 2;
    [SerializeField] private float delayAtaque = 0.5f;

    [Header("Referencias")]
    [SerializeField] private OtherSoundController OtherSoundController;

    private Rigidbody2D rb;
    private Animator animator;
    private Transform player;         // Referencia a la posición del jugador

    private Vector2 movement;         // Direccion hacia donde se movera
    private float ataqueTimer;

    private bool playerVivo;
    private bool muerto;
    private bool enMovimiento;
    private bool recibiendoDanio;
    private bool atacando;

    void Start()
    {
        playerVivo = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");               // Buscar al jugador en la escena
        if (playerObj != null)
            player = playerObj.transform;
    }

    void Update()
    {
        if (playerVivo && !muerto)                       // Si el jugador existe y el enemigo no está muerto
        {
            Movimiento();                                // Calcular movimiento
            VerificarAtaque();                           // Chequear si puede atacar
        }
        //Animators
        animator.SetBool("enMovimiento", enMovimiento);
        animator.SetBool("muerto", muerto);
        animator.SetBool("Atacando", atacando);
    }

    private void Movimiento()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position); // Distancia entre jugador y el enemigo

        if (distanceToPlayer < detectionRadius)                                         // Si la distancia con el jugador es menor al radio de deteccion
        {
            Vector2 direction = (player.position - transform.position).normalized;      // El enemigo se mueve en su direccion, restando sus cordenadas con las del objetivo

            if (direction.x < 0)                                                        // Si la direccion es negativa (izquierda)
            { 
                transform.localScale = new Vector3(-1, 1, 1);                           // Cambio de orientacion del sprite
            }                           
            if (direction.x > 0)                                                        // Si la direccion es possitiva (derecha)
            { 
                transform.localScale = new Vector3(1, 1, 1);                            // Cambio de orientacion del sprite
            }

            if (distanceToPlayer < rangoAtaque)                                         // Si el enemigo llega a su rango de ataque, deja de moverse
            {
                movement = Vector2.zero;
                enMovimiento = false;
            }
            else                                                                        // Si an no llego al rango de ataque, sigue moviendose
            {
                movement = direction;
                enMovimiento = true;
            }
        }
        else                                                                            // Si no detecto jugador, deja de movese
        {
            movement = Vector2.zero;
            enMovimiento = false;
        }
        // Solo se mueve si no está recibiendo daño y no está atacando
        if (!recibiendoDanio && !atacando)
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision) // Si el jugador toca al enemigo caminando
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direccionDanio = (player.position - transform.position).normalized;             // Calcular dirección para empujar al jugador
            OtherSoundController.PlayEnemigoSound();

            PlayerControler playerScript = collision.gameObject.GetComponent<PlayerControler>();
            playerScript.RecibeDanio(direccionDanio, 1);                                            // Daniar al jugador
            playerVivo = !playerScript.muerto;                                                      // Actualizar estado si el jugador murio

            if (!playerVivo) // Se detiene si el jugador muere
            { 
                enMovimiento = false; 
            }
        }
    }

    private void VerificarAtaque() // Si ya paso suficiente tiempo para nuevo ataque
    { 
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < rangoAtaque && !atacando)                                            // Si esta en rango y no esta atacando
        {
            ataqueTimer += Time.deltaTime;
            if (ataqueTimer >= delayAtaque)                                                         // Si el contador supera el delay
            {
                Atacar();                                                                           // Iniciar ataque
                ataqueTimer = 0f;
            }
        }
        else 
        {
            ataqueTimer = 0f;    // Si el jugador se aleja, reseteamos el contador para que no ataque instantáneamente al volver
        }
    }

    private void Atacar()
    {
        atacando = true;
        movement = Vector2.zero;                                                                // Detiene el movimiento
        rb.linearVelocity = Vector2.zero;                                                       // Asegura que no se desplace
        
        Vector2 direccion = (player.position - transform.position).normalized;                  // Calcular dirección del ataque
        int dir = 3; // Derecha por defecto
        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y))
        {
            dir = direccion.x < 0 ? 2 : 3;                       // izquierda o derecha
        }
        else
        {
            dir = direccion.y < 0 ? 1 : 0;                       // abajo o arriba
        }

        animator.SetInteger("DireccionAtaque", dir);             // Animacion
        OtherSoundController.PlayAtaqueEnemigoSound();
    }
    public void AplicarDanio()
    {
        PlayerControler playerScript = player.GetComponent<PlayerControler>();
        if (playerScript != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer < rangoAtaque)                                                 // Solo aplicar daño si jugador sigue en rango
            {
                Vector2 direccionDanio = (player.position - transform.position).normalized;
                playerScript.RecibeDanio(direccionDanio, danioAntorcha);                        // Jugador recibe danio
            }
        }
    }

    public void TerminaAtaque() // Libera al enemigo para que pueda volver a moverse
    {
        atacando = false;
    }

    private void OnTriggerEnter2D(Collider2D collision) // Detecta cuando espada del jugador entra en el Trigger
    {
        if (collision.CompareTag("Espada"))                                                                                                  // Recibe danio de espada
        {
            PlayerControler playerScript = collision.GetComponentInParent<PlayerControler>();
            if (playerScript != null)
            {
                Vector2 direccionDanio = new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y);
                RecibeDanio(direccionDanio, playerScript.damage);                                                                            // Aplicamos daño al enemigo
            }
        }
    }

    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (!recibiendoDanio)                                                                   // Tambien evita recibir golpes muy seguidos 
        {
            vida -= cantDanio;
            recibiendoDanio = true;                                                             // Quizas mover al else de abajo*********************************
            OtherSoundController.PlayGritoEnemigoSound();
            if (vida <= 0)                                                                      // Muerte del enemigo
            {
                muerto = true;
                enMovimiento = false;                          // Deja de moverse al morir
                OtherSoundController.PlayMuerteEnemigoSound();
                rb.linearVelocity = Vector2.zero;
            }
            else                                                                                // Rebote
            { 
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, transform.position.y - direccion.y).normalized;    // Calcula vector opuesto al golpe
                rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);                                                            // Impulso
                StartCoroutine(DesactivaDanio());                                                                                   // cuenta regresiva para recuperarse
            }
        }
    }

    IEnumerator DesactivaDanio() // Corrutina
    {
        yield return new WaitForSeconds(0.4f);                                                  // Cada cuanto puede rebotar de nuevo
        recibiendoDanio=false;
        rb.linearVelocity = Vector2.zero;                                                       // Frena el empuje del rebote
    }
   
    public void DeleteBody() // Destruye el cuerpo
    {
        Destroy(gameObject);
    }
   
    private void OnDrawGizmosSelected() // Ver rango de busqueda y ataque del enemigo
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }
}
