using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    // -------------------- Public --------------------
    public float velocidad = 5f;
    public int vida = 3;
    public float fuerzaSalto = 10f;
    public float fuerzaRebote = 10f;
    public float longitudRaycast = 0.1f;
    public LayerMask capaSuelo;
    public bool muerto;
    public Animator animator;
    public float cooldown = 2f;
    public int vidaMaxima = 10;
    public int money = 3;
    public int damage = 1;
    public PlayerSoundController PlayerSoundController;
    public bool step1 = false;  
    public float timeByStep = 0.3f;
    public float cont = 0f;

    // -------------------- Private -------------------
    private bool enSuelo;
    private float timer;
    private Rigidbody2D rb;
    private bool recibiendoDanio;
    private bool atacando;
    private Vector3 posicionInicial;
    // -------------------- Unity Methods -------------
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        posicionInicial = transform.position;
    }

    private void Update()
    {
        if (timer > 0f)
        {timer -= Time.deltaTime;}

        if (!muerto) // Si no está muerto: movimiento, salto y ataque
        {
            if (!atacando) // Si no está atacando: movimiento y salto
            {
                Movimiento();

                // Raycast hacia abajo para detectar suelo
                RaycastHit2D hit = Physics2D.Raycast(
                    transform.position,
                    Vector2.down,
                    longitudRaycast,
                    capaSuelo
                );
                enSuelo = hit.collider != null;

                // Salto
                if (enSuelo && Input.GetKeyDown(KeyCode.Space) && !recibiendoDanio)
                {
                    rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
                }
            }

            // Ataque
            //if (Input.GetKeyDown(KeyCode.U) && !atacando){Atacando();}

            if (Input.GetKeyDown(KeyCode.U) && !atacando) // Arriba
                Atacar(0);
            if (Input.GetKeyDown(KeyCode.J) && !atacando) // Abajo
                Atacar(1);
            if (Input.GetKeyDown(KeyCode.H) && !atacando) // Izquierda
                Atacar(2);
            if (Input.GetKeyDown(KeyCode.K) && !atacando) // Derecha
                Atacar(3);

        }

        // ---------------- Animator --------
        // animator.SetBool("ensuelo", enSuelo);
        animator.SetBool("RecibeDanio", recibiendoDanio);
        animator.SetBool("Atacando", atacando);
        animator.SetBool("muerto", muerto);
    }

    // ------------- Movimiento ---------------
    public void Movimiento()
    {
       
        float velocidadX = Input.GetAxis("Horizontal") * Time.deltaTime * velocidad;
        float velocidadY = Input.GetAxis("Vertical") * Time.deltaTime * velocidad;

        animator.SetFloat("MovementX", Mathf.Abs(velocidadX * velocidad));
        animator.SetFloat("MovementY", Mathf.Abs(velocidadY * velocidad));

        if (velocidadX != 0 || velocidadY != 0 && !recibiendoDanio && !atacando)
        {
            cont += Time.deltaTime;
            if (cont >= timeByStep)
            {
                cont = 0f;
                if (!step1)
                {
                    PlayerSoundController.PlayMovementSound();
                    step1 = true;
                }
                else
                {
                    step1 = false;
                }
            }
        }
        // Flip horizontal
        if (velocidadX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (velocidadX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        // Movimiento directo (ojo: esto ignora física)
        Vector3 posicion = transform.position;
        if (!recibiendoDanio)
        {
            transform.position = new Vector3(
                velocidadX + posicion.x,
                velocidadY + posicion.y,
                posicion.z
            );
        }
    }

    // ------------------- Daño ----------------
    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (!recibiendoDanio) // Evita recibir daño demasiado rápido
        {
            PlayerSoundController.PlayDamageSound(); // sonido de recibir danio
            recibiendoDanio = true;
            vida -= cantDanio;

            if (vida <= 0)
            {
                PlayerSoundController.PlayDeadSound();
                muerto = true;
                
                if (GameManager.Instance != null)
                {
                    GameManager.Instance.GameOver(); // Pantalla de game over
                }
            }
            else
            {
                rb.AddForce(direccion * fuerzaRebote, ForceMode2D.Impulse);
            }
        }
    }

    public void DesactivaDanio()
    {
        recibiendoDanio = false;
        rb.linearVelocity = Vector2.zero; // Resetea la fuerza de rebote
    }

    // -------------- Ataque ---------------
  
    public void Atacar(int direccion)
    {
        if (timer <= 0f)
        {
            atacando = true;
            timer = cooldown;
            PlayerSoundController.PlayAttackVoiceSound();
            PlayerSoundController.PlayAttackSwordSound();

            // Dirección del ataque para el Animator
            animator.SetInteger("DireccionAtaque", direccion);

            if (direccion == 2) // izquierda
                transform.localScale = new Vector3(-1, 1, 1);
            else if (direccion == 3) // derecha
                transform.localScale = new Vector3(1, 1, 1);
            // Detiene al jugador durante el ataque
            rb.linearVelocity = Vector2.zero;
        }
    }


    public void DesactivaAtacando()
    {
        atacando = false;
    }
    // --------------Mejoras---------------
    public void MejorarSalud(int cantidad)
    {
        vidaMaxima += cantidad;          // aumenta el límite
        vida = vidaMaxima;               // opcional: llenar la vida al máximo
    }
    public void MejorarDanio(int cantidad)
    {
        damage += cantidad; // aumenta el daño
    }
    public void Revivir(Vector3 nuevaPosicion)
    {
        // 1. Restaurar vida al máximo
        vida = vidaMaxima;

        // 2. Resetear estado
        muerto = false;
        recibiendoDanio = false;

        // 3. Teletransportar a la posición inicial
        transform.position = nuevaPosicion;

        // 4. Asegurar que el Rigidbody esté en reposo (opcional, pero útil)
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        // 5. Opcional: Resetear la escala si se había volteado
        transform.localScale = new Vector3(1, 1, 1);
    }
    //------------- Gizmos ----------------
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(
            transform.position,
            transform.position + Vector3.down * longitudRaycast
        );
    }
}
