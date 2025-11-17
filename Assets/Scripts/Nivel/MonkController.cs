using UnityEngine;

public class MonkController : MonoBehaviour
{ 
    public Transform player;
    public float detectionRadius = 5.0f;
    public int cantidadCuracion = 1; // cuánto cura cada vez

    //private bool playerVivo;
    //private bool muerto;
    private bool curando;
    private Animator animator;
    private PlayerControler playerControler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        playerControler = player.GetComponent<PlayerControler>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerControler.vida < playerControler.vidaMaxima)
        { 
        Curar();
        }
                
        //Animators
        animator.SetBool("Curando", curando);
        
    }
    private void Curar()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);
       
        if (distanceToPlayer < detectionRadius)                                   
        {                                                                        
             Vector2 direction = (player.position - transform.position).normalized;
            
            curando = true;
        }
        else { curando = false; }
        
    }

    public void IniciaCuracion()
    {
        if (playerControler != null && !playerControler.muerto)
        {
            playerControler.vida += cantidadCuracion;

            // Opcional: limitar la vida máxima
            if (playerControler.vida >= playerControler.vidaMaxima) // ejemplo: vida máxima = 10
               curando = !true;
        }
    }

    public void TerminaAnim()
    { 
        curando = false;
        animator.SetBool("Curando", false);
    }

    private void OnDrawGizmosSelected() //Ver rango de busqueda del player
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}
