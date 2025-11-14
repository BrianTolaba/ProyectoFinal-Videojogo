using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    //Public---------------------------------
    public float velocidad = 5f;
    public int vida = 3; 
    public float fuerzaSalto = 10f;
    public float fuerzaRebote = 10f;
    public float longitudRaycast = 0.1f;
    public LayerMask capaSuelo;
    public bool muerto;
    public Animator animator;
    //Private--------------------------------
    private bool enSuelo;
    private Rigidbody2D rb;
    private bool recibiendoDanio;
    private bool atacando;
    
    void Start()
    {rb = GetComponent<Rigidbody2D>();}

    void Update()
    {
        if (!muerto) //Si no esta muerto: movimiento, "salto" y ataca
        { 
          if (!atacando) //Si no esta atacando: movimiento y "salto"
          {
            Movimiento(); //Movimiento, en detalle mas abajo

            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo); //Raycast hacia abajo, mejorar su uso*********************
            enSuelo = hit.collider != null;

            if (enSuelo && Input.GetKeyDown(KeyCode.Space) && !recibiendoDanio) 
            {
            rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse); //Salto quitado, reutilizar como boton interactuar***************
            }
          }
            
            if (Input.GetKeyDown(KeyCode.Z) && !atacando)
            {
            Atacando();
            }

        }
        //Animators------------------------------------------------------------------------
        //animator.SetBool("ensuelo", enSuelo);
        animator.SetBool("RecibeDanio", recibiendoDanio);
        animator.SetBool("Atacando", atacando);
        animator.SetBool("muerto",muerto);
    }

   public void Movimiento() {

        float velocidadX = Input.GetAxis("Horizontal") * Time.deltaTime * velocidad; 

        float velocidadY = Input.GetAxis("Vertical") * Time.deltaTime * velocidad;

        animator.SetFloat("Movement", velocidadX * velocidad);
        //animator.SetFloat("Movement", velocidadY * velocidad); //Movimiento en Y aun bug***************************************

        if (velocidadX < 0)
        {transform.localScale = new Vector3(-1, 1, 1);} //Rota el sprite segun la direccion de su movimiento
        if (velocidadX > 0)
        {transform.localScale = new Vector3(1, 1, 1);}

        Vector3 posicion = transform.position; //posicion actual

        if (!recibiendoDanio) //Solo se mueve si no esta reciviendo danio
            transform.position = new Vector3(velocidadX + posicion.x, velocidadY + posicion.y, posicion.z);
    }
    
    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (!recibiendoDanio) //Evita recibir danio demasiado rapido
        {
            recibiendoDanio = true; //vuelve a posibilitar el danio
            vida -= cantDanio;
            if (vida<=0)
            {
                muerto = true; 
                if (GameManager.Instance != null) 
                {
                    GameManager.Instance.GameOver(); //pantalla de game over
                }
            }else{ //si aun tiene vida, rebota
                Vector2 rebote = new Vector2(transform.position.x - direccion.x, transform.position.y - direccion.y).normalized; // rebota en direccion opuesta al golpe //rebote sigue extranio*************************************
                rb.AddForce(rebote * fuerzaRebote,ForceMode2D.Impulse); //agrega el rebote al rigidbody del jugador
            }
                 
        }
       
    }
    public void DesactivaDanio() 
    {
        recibiendoDanio = false;
        rb.linearVelocity = Vector2.zero; //resetea la fuerza de rebote aplicada al rigidbody
    }
    public void Atacando()
    {
        atacando = true;
    }
    public void DesactivaAtacando() 
    {
        atacando = false;
    }

    private void OnDrawGizmos() //ver rango del Raycast
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}
