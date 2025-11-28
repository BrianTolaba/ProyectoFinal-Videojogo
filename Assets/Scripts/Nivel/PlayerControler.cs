using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [Header("Estadisticas del Jugador")]
    public int vida = 3;
    public int vidaMaxima = 10;
    public int money = 3;
    public int damage = 1;
    public bool muerto;

    [Header("Movimiento y Fisica")]
    public float velocidad = 5f;
    public float fuerzaRebote = 10f;
    
    [Header("Combate")]
    public float cooldown = 2f;

    [Header("Audio y Pasos")]
    public PlayerSoundController PlayerSoundController; 
    public float timeByStep = 0.3f;

    private Rigidbody2D rb;
    private Animator animator;

    private float inputX;
    private float inputY;

    private bool recibiendoDanio;
    private bool atacando;

    private float ataqueTimer;
    private float pasoTimer = 0f;
    private bool pasoAlternado = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Reducir Cooldown de ataque
        if (ataqueTimer > 0f)
        {
            ataqueTimer -= Time.deltaTime;
        }
        else if (atacando)
        {
            // Si el tiempo del cooldown ya llegó a 0, pero la variable 'atacando' sigue en true, significa que Unity se saltó el evento de animacion. Lo forzamos a false manualmente.
            DesactivaAtacando();
            Debug.Log("Bug Animacion");
        }

        // Si esta muerto o en pausa, no hace nada
        if (muerto || Time.timeScale == 0f) return;

        // Leer Inputs (Teclado)
        inputX = Input.GetAxisRaw("Horizontal");
        inputY = Input.GetAxisRaw("Vertical");

        // Logica de acciones
        if (!atacando && !recibiendoDanio)
        {
            ProcesarAtaque();
        }

        // Actualizar animaciones
        ActualizarAnimator();
    }

    private void FixedUpdate()
    {
        // El movimiento físico va aquí para evitar "temblores"
        if (!muerto && !atacando && !recibiendoDanio)
        {
            MoverPersonaje();
        }
    }

    // ------------------------------------------------ MOVIMIENTO ------------------------------------------------
    private void MoverPersonaje()
    {
        // Mover el Rigidbody en X e Y
        rb.linearVelocity = new Vector2(inputX * velocidad, inputY * velocidad);

        // Sonido de pasos 
        if (rb.linearVelocity.magnitude > 0.1f)
        {
            pasoTimer += Time.deltaTime;
            if (pasoTimer >= timeByStep)
            {
                pasoTimer = 0f;
                pasoAlternado = !pasoAlternado;
                if (PlayerSoundController != null)
                    PlayerSoundController.PlayMovementSound();
            }
        }
        else
        {
            pasoTimer = 0f;
        }

        // Girar Sprite
        if (inputX > 0) transform.localScale = new Vector3(1, 1, 1);
        else if (inputX < 0) transform.localScale = new Vector3(-1, 1, 1);
    }

    // ------------------------------------------------ COMBATE ------------------------------------------------
    private void ProcesarAtaque()
    {
        if (ataqueTimer > 0) return;

        // Detectar teclas de ataque
        if (Input.GetKeyDown(KeyCode.U)) EjecutarAtaque(0);      // Arriba
        else if (Input.GetKeyDown(KeyCode.J)) EjecutarAtaque(1); // Abajo
        else if (Input.GetKeyDown(KeyCode.H)) EjecutarAtaque(2); // Izquierda
        else if (Input.GetKeyDown(KeyCode.K)) EjecutarAtaque(3); // Derecha
    }
    private void EjecutarAtaque(int direccion)
    {
        atacando = true;
        ataqueTimer = cooldown;

        // Frena al personaje para que no deslice al pegar
        rb.linearVelocity = Vector2.zero;

        // Configurar animacion
        animator.SetInteger("DireccionAtaque", direccion);

        // Orientacion del sprite segun ataque
        if (direccion == 2) transform.localScale = new Vector3(-1, 1, 1); // Izq
        else if (direccion == 3) transform.localScale = new Vector3(1, 1, 1); // Der

        // Sonidos
        if (PlayerSoundController != null)
        {
            PlayerSoundController.PlayAttackVoiceSound();
            PlayerSoundController.PlayAttackSwordSound();
        }
    }
    public void DesactivaAtacando() // Evento de Animacion
    {
        atacando = false;
    }

    // ------------------------------------------------ DAÑO ------------------------------------------------
    public void RecibeDanio(Vector2 direccionEmpuje, int cantidad)
    {
        if (recibiendoDanio || muerto) return;

        recibiendoDanio = true;
        vida -= cantidad;

        if (PlayerSoundController != null)
        {
            PlayerSoundController.PlayDamageSound();
        }

        if (vida <= 0)
        {
            Morir();
        }
        else
        {
            // Empuje hacia atrás
            rb.linearVelocity = Vector2.zero; // Reseta velocidad para un empuje limpio
            rb.AddForce(direccionEmpuje * fuerzaRebote, ForceMode2D.Impulse);
        }
    }
    private void Morir()
    {
        muerto = true;
        if (PlayerSoundController != null) PlayerSoundController.PlayDeadSound();

        rb.linearVelocity = Vector2.zero;
        rb.isKinematic = true; // Desactivar física al morir

        if (GameManager.Instance != null) // Pantalla de GameOver
        {
            GameManager.Instance.GameOver();
        }
    }
    public void DesactivaDanio() // Evento de Animacion
    {
        recibiendoDanio = false;
    }

    // ------------------------------------------------ UTILIDADES ------------------------------------------------

    public void MejorarSalud(int cantidad)
    {
        vidaMaxima += cantidad;
        vida = vidaMaxima;
    }
    public void MejorarDanio(int cantidad)
    {
        damage += cantidad;
    }
    public void Revivir(Vector3 nuevaPosicion)
    {
        vida = vidaMaxima;
        muerto = false;
        recibiendoDanio = false;
        atacando = false;

        rb.isKinematic = false;
        rb.linearVelocity = Vector2.zero;

        transform.position = nuevaPosicion;
        transform.localScale = Vector3.one;
        
        animator.Rebind();                      // Reinicia el animator al estado base
    }
    private void ActualizarAnimator()
    {
        animator.SetFloat("MovementX", Mathf.Abs(inputX * velocidad));
        animator.SetFloat("MovementY", Mathf.Abs(inputY * velocidad));

        animator.SetBool("RecibeDanio", recibiendoDanio);
        animator.SetBool("Atacando", atacando);
        animator.SetBool("muerto", muerto);
    }
}
