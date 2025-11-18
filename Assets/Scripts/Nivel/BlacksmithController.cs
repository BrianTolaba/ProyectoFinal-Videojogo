using UnityEngine;

public class BlacksmithController : MonoBehaviour
{
    public float detectionRadius = 1.5f;
    public int cantidadMejoraDanio = 1;
    public int costoMejora = 2; // Aqui o mas tarde valor fijo

    private Transform player;
    private PlayerControler playerControler;
    private Animator animator;
    private bool mejorando;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
            playerControler = playerObj.GetComponent<PlayerControler>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerControler == null || playerControler.muerto) return;

        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            if (Input.GetKeyDown(KeyCode.E) && playerControler.money >= costoMejora)
            {
                playerControler.MejorarDanio(cantidadMejoraDanio);
                playerControler.money -= costoMejora;
                mejorando = true;
            }
        }
        else
        {
            mejorando = false;
        }

        animator.SetBool("Mejorando", mejorando);
    }

    public void TerminaAnim()
    {
        mejorando = false;
        animator.SetBool("Mejorando", false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
