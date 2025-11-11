using UnityEngine;

public class Moneda : MonoBehaviour
{

    public Animator animator;

   
    private void Update()
    {
        //animator.SetBool("Spawning", true);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }
}
