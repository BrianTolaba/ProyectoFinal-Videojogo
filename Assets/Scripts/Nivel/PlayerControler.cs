using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    public float velocidad = 5f;
    
    public float fuerzaSalto = 10f;
    public float fuerzaRebote = 10f;
    public float longitudRaycast = 0.1f;
    public LayerMask capaSuelo;

    private bool enSuelo;
    private Rigidbody2D rb;



    public Animator animator;

    //Danio
    private bool recibiendoDanio;
    //Ataque
    private bool atacando;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (!atacando)
        {
            Movimiento(); 
            if (enSuelo && Input.GetKeyDown(KeyCode.Space) && !recibiendoDanio)
        {

            rb.AddForce(new Vector2(0f, fuerzaSalto), ForceMode2D.Impulse);
        }
        if (Input.GetKeyDown(KeyCode.Z) && !atacando)
        {
            Atacando();
        }
        }
        

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, longitudRaycast, capaSuelo);
        enSuelo = hit.collider != null;

       
        //animator.SetBool("ensuelo", enSuelo);
        animator.SetBool("RecibeDanio", recibiendoDanio);
        animator.SetBool("Atacando", atacando);
    }

   public void Movimiento() {

        float velocidadX = Input.GetAxis("Horizontal") * Time.deltaTime * velocidad;

        float velocidadY = Input.GetAxis("Vertical") * Time.deltaTime * velocidad;

        animator.SetFloat("Movement", velocidadX * velocidad);


        if (velocidadX < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        if (velocidadX > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }


        Vector3 posicion = transform.position;


        if (!recibiendoDanio)
            transform.position = new Vector3(velocidadX + posicion.x, velocidadY + posicion.y, posicion.z);
    }
    
    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (!recibiendoDanio)
        {
            recibiendoDanio = true;
        Vector2 rebote = new Vector2(transform.position.x - direccion.x, transform.position.y - direccion.y).normalized;
        rb.AddForce(rebote * fuerzaRebote,ForceMode2D.Impulse); 
        }
       
    }
    public void DesactivaDanio() 
    {
        recibiendoDanio = false;
        rb.linearVelocity = Vector2.zero;
    }
    public void Atacando()
    {
        atacando = true;
    }
    public void DesactivaAtacando() 
    {
        atacando = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * longitudRaycast);
    }
}
