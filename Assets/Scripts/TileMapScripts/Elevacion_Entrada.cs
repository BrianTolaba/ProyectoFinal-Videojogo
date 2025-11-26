using UnityEngine;

public class Elevacion_Entrada : MonoBehaviour
{
    public Collider2D[] mountainColliders;
    public Collider2D[] murosInvisiblesNivel1;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            foreach (Collider2D mountain in mountainColliders)
            {
                mountain.enabled = false;
            }
            foreach (Collider2D muros in murosInvisiblesNivel1)
            {
                muros.enabled = true;
            }

            collision.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 20;
        }
    }
}
