using System.Collections;
using UnityEngine;

public class EnemyControler : MonoBehaviour
{

    public Transform player;
    public float detectionRadius = 5.0f;
    public float speed = 2.0f;
    public float fuerzaRebote = 2f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool enMovimiento;
    private bool recibiendoDanio;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer < detectionRadius)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            if (direction.x <0)
            {
                transform.localScale = new Vector3(-1, 1, 1);

            }
            if (direction.x > 0)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            movement = new Vector2(direction.x, direction.y);
            enMovimiento = true;
        }
        else {
            movement = Vector2.zero;
            enMovimiento = false;
        }
        if (!recibiendoDanio)
            rb.MovePosition(rb.position + movement * speed * Time.deltaTime);
        
        
        animator.SetBool("enMovimiento", enMovimiento);
        }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 direccionDanio = new Vector2(transform.position.x, 0);
            collision.gameObject.GetComponent<PlayerControler>().RecibeDanio(direccionDanio, 1);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Espada"))
        {
            Vector2 direccionDanio = new Vector2(collision.gameObject.transform.position.x, 0);
            RecibeDanio(direccionDanio, 1);
        }
    }
    public void RecibeDanio(Vector2 direccion, int cantDanio)
    {
        if (!recibiendoDanio)
        {
            recibiendoDanio = true;
            Vector2 rebote = new Vector2(transform.position.x - direccion.x, transform.position.y - direccion.y).normalized;
            rb.AddForce(rebote * fuerzaRebote, ForceMode2D.Impulse);
            StartCoroutine(DesactivaDanio());
        }

    }
    IEnumerator DesactivaDanio() 
    {
        yield return new WaitForSeconds(0.4f);
        recibiendoDanio=false;
        rb.linearVelocity = Vector2.zero;
    }
    //Ver rango de busqueda del enemigo

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}
