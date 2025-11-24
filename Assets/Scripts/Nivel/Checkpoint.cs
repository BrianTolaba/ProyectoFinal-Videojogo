using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public Transform puntoDeReaparicion; // El punto exacto donde reaparecerá el jugador.
    // Una variable para controlar si este checkpoint ya ha sido activado.
    private bool activado = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Asegúrate de que solo se active si el objeto que entra es el jugador
        if (other.CompareTag("Player")) // Ya no chequeamos por 'activado'
        {
            // OBTENER la instancia del GameManager
            GameManager gm = GameManager.Instance;

            // Llama al GameManager para actualizar la posición de reaparición.
            if (gm != null)
            {
                if (puntoDeReaparicion != null)
                {
                    gm.EstablecerCheckpoint(puntoDeReaparicion.position);
                    Debug.Log("Checkpoint activado. Reaparición en: " + puntoDeReaparicion.position);
                }
                else
                {
                    Debug.LogError("Error en Checkpoint: 'puntoDeReaparicion' no está asignado en el Inspector.");
                }
            }
        }
    }
}