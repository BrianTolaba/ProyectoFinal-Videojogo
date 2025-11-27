using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    [Header("Configuración")]
    [SerializeField] private Transform puntoDeReaparicion; // El punto exacto donde reaparecera el jugador

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))                                                      // Verificar si es el jugador
        {
            if (GameManager.Instance != null)                                                // Verificar si existe el GameManager
            {
                if (puntoDeReaparicion != null)                                              // Verificar si existe puntoDeReaparicion
                {
                    GameManager.Instance.EstablecerCheckpoint(puntoDeReaparicion.position);  // Utiliza el metodo EstablecerCheckpoint de GameManager
                }
                else
                {
                    Debug.LogError("Error en Checkpoint: 'puntoDeReaparicion' no está asignado en el Inspector");
                }
            }
            else
            {
                Debug.LogWarning("Error en Checkpoint: No se encontro el GameManager en la escena");
            }
        }
    }
}