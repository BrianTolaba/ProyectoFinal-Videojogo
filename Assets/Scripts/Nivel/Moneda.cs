using UnityEngine;

public class Moneda : MonoBehaviour
{
    public OtherSoundController OtherSoundController;
    private bool spawning;
    private bool recogido;
    public Animator animator;
    


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        animator.SetBool("Recogido", recogido);
        animator.SetBool("Spawning", spawning);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            
        {
            PlayerControler playerControler = collision.GetComponent<PlayerControler>();
            
            if (playerControler != null)
                
            { 
            playerControler.money += 1;
                recogido = true;
                OtherSoundController.PlayDineroSound(); // sonido recoger moneda

            }
            
        }
    }

    public void DeleteObj()
    {
        
        Destroy(gameObject);
    }
}
