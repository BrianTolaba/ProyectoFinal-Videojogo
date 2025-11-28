using UnityEngine;

public class WinCondition : MonoBehaviour
{
    private BoxCollider2D miCollider;
    private bool jugadorAdentro;

    private void Awake()
    {
        miCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        // Solo revisamos si el jugador está dentro. Si no, no hacemos nada.
        if (jugadorAdentro && ZonaDespejada())
        {
            GameManager.Instance?.Winning();
            this.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)  // Encendemos el interruptor
    {
        if (other.CompareTag("Player"))
        {
            jugadorAdentro = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)  // Apagamos el interruptor
    {
        if (other.CompareTag("Player"))
        {
            jugadorAdentro = false;
        }
    }

    private bool ZonaDespejada()
    {
        var objetos = Physics2D.OverlapBoxAll((Vector2)transform.position + miCollider.offset, miCollider.size, 0f);

        // Si se encontra al menos un enemigo, retorna false
        foreach (var obj in objetos)
            if (obj.CompareTag("Enemy")) return false;

        return true; // Si termino el bucle sin encontrar enemigos, está limpio
    }

    private void OnDrawGizmos()
    {
        if (miCollider == null)
        {
            miCollider = GetComponent<BoxCollider2D>();
        }
        if (miCollider != null)
        {
            Gizmos.color = new Color(0, 1, 0, 0.3f);
            Gizmos.DrawCube(transform.position + (Vector3)miCollider.offset, miCollider.size);
        }
    }
}