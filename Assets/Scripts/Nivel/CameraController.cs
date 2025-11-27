using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Objetivo a Seguir")]
    [SerializeField] private Transform objetivo;          // A que objeto va a seguir la camara

    [Header("Configuración")]
    [Range(2.0f, 8.0f)]  // Crea un slider en el editor
    [SerializeField] private float velocidadCamera = 2f;  // Que tan rapido sigue la camara. Valor bajo es muy suave, valor alto es mas rigido
    [SerializeField] private Vector3 desplazamiento;      // Distancia extra respecto al objeto

    private void LateUpdate()
    {
        if (objetivo == null) return;                                                                                    // Por seguridad
        Vector3 posicionDeseada = objetivo.position + desplazamiento;                                                    // Calcular donde deberia estar la camara
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamera * Time.deltaTime); // Suavizado (Lerp mueve la cámara un porcentaje de la distancia)
        transform.position = posicionSuavizada;                                                                          // Aplica el movimiento
    }
}
