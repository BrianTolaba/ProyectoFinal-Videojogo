using System.Collections;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{
    //public---------------------------------
    //[SerializeField] public Transform player;
    public float detectionRadius = 5.0f;
    public float speed = 2.0f;
    public float fuerzaRebote = 2f;
    public int vida = 3;
    public float rangoAtaque = 2;
    public float delayAtaque = 0.5f; //****
    public OtherSoundController OtherSoundController;
    public int danioAntorcha = 1;

    //private----------------------------------
    private Rigidbody2D rb;
    private Transform player;
    private Vector2 movement;
    private bool playerVivo;
    private bool muerto;
    private bool enMovimiento;
    private bool recibiendoDanio;
    private Animator animator;
    private bool atacando;
    private float ataqueTimer;
   

    void Start()
    {
        playerVivo = true;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;
    }


    void Update()
    {
        if (playerVivo && !muerto) 
        {
            Movimiento();
            VerificarAtaque();
        }
        //Animators
        animator.SetBool("enMovimiento", enMovimiento);
        animator.SetBool("muerto", muerto);
        animator.SetBool("Atacando", atacando);
    }
    private void Movimiento()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);


        if (distanceToPlayer < detectionRadius)                                      // si la distancia con el jugador es menor al radio de deteccion
        {                                                                           // el enemigo se mueve en su direccion
            Vector2 direction = (player.position - transform.position).normalized; // restando sus cordenadas con las del objetivo
            if (direction.x < 0)
                
            { transform.localScale = new Vector3(-1, 1, 1); }//cambio de orientacion del sprite
            if (direction.x > 0) { transform.localScale = new Vector3(1, 1, 1); }
            if (distanceToPlayer < rangoAtaque)

            {
                movement = Vector2.zero;
                enMovimiento = false;
            }
            else
            {
                movement = direction;
                enMovimiento = true;
            }
        }
        else
        {
            movement = Vector2.zero;
            enMovimiento = false;
        }

        // Solo se mueve si no está recibiendo daño y no está atacando
        if (!recibiendoDanio && !atacando)
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direccionDanio = (player.position - transform.position).normalized;
            PlayerControler playerScript = collision.gameObject.GetComponent<PlayerControler>();
            OtherSoundController.PlayEnemigoSound(); 


            playerScript.RecibeDanio(direccionDanio, 1);
            playerVivo = !playerScript.muerto;
            if (!playerVivo) //Se detiene si el jugador muere
            { enMovimiento = false; }
        }
    }

    //*****
    private void VerificarAtaque()
    { 
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
        if (distanceToPlayer < rangoAtaque && !atacando)

        {
            ataqueTimer += Time.deltaTime;
            if (ataqueTimer >= delayAtaque)
            {
                Atacar();
                ataqueTimer = 0f;
                
            }
        }
        else 
        {
            ataqueTimer = 0f; //Funciona?
        }
    }

    private void Atacar()
    {
        atacando = true;
        movement = Vector2.zero; // detiene el movimiento
        rb.linearVelocity = Vector2.zero; // asegura que no se desplace
        // Calcular dirección del ataque
        Vector2 direccion = (player.position - transform.position).normalized;
        int dir = 3; // derecha por defecto //******

        if (Mathf.Abs(direccion.x) > Mathf.Abs(direccion.y))
        {
            dir = direccion.x < 0 ? 2 : 3; // izquierda o derecha
        }
        else
        {
            dir = direccion.y < 0 ? 1 : 0; // abajo o arriba
        }

        animator.SetInteger("DireccionAtaque", dir);
        OtherSoundController.PlayAtaqueEnemigoSound();         //sonido del ataque
        


    }
    public void AplicarDanio()
    {
        PlayerControler playerScript = player.GetComponent<PlayerControler>();
        if (playerScript != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            

            // Solo aplicar daño si sigue en rango
            if (distanceToPlayer < rangoAtaque)
            {
                Vector2 direccionDanio = (player.position - transform.position).normalized; //tambien hace falta aqui abajo
                playerScript.RecibeDanio(direccionDanio, danioAntorcha);
                

            }
        }
    }

    public void TerminaAtaque()
    {
        atacando = false;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Espada")) //recibe danio de espada
        {
            PlayerControler playerScript = collision.GetComponentInParent<PlayerControler>();
            if (playerScript != null)
            {
                Vector2 direccionDanio = new Vector2(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y);
                RecibeDanio(direccionDanio, playerScript.damage); //cantDanio = 1
            }
        }
    }
    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (!recibiendoDanio) //tambien evita recibir golpes muy seguidos 
        {
            vida -= cantDanio;
            recibiendoDanio = true;//Quizas mover al else de abajo*********************************
            OtherSoundController.PlayGritoEnemigoSound(); //sonido de recibir danio
            if (vida <= 0)
            {
                muerto = true;
                enMovimiento = false;//deja de moverse al morir
                OtherSoundController.PlayMuerteEnemigoSound(); //sonido de muerte
            }
            else 
            { 
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, transform.position.y - direccion.y).normalized;
                rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);
                StartCoroutine(DesactivaDanio()); //Llama Corrutina
            }
               
        }

    }
    IEnumerator DesactivaDanio() //Corrutina
    {
        yield return new WaitForSeconds(0.4f); //cada cuanto puede rebotar de nuevo
        recibiendoDanio=false;
        rb.linearVelocity = Vector2.zero;
    }
   

    public void DeleteBody() //Destruye el cuerpo
    {Destroy(gameObject);}
   

    private void OnDrawGizmosSelected() //Ver rango de busqueda y ataque del enemigo
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, rangoAtaque);
    }

}
