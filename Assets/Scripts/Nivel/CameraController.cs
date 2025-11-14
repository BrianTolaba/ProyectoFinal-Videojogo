using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform objetivo;
    public float velocidadCamera = 0.025f;
    public Vector3 desplazamiento;

    private void LateUpdate()
    {
         Vector3 posicionDeseada = objetivo.position + desplazamiento;
        Vector3 posicionSuavizada = Vector3.Lerp(transform.position, posicionDeseada, velocidadCamera);
        transform.position = posicionSuavizada;
    }
}
