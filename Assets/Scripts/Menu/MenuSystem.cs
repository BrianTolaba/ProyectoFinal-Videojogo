using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void Jugar() 
    {
        SceneManager.LoadScene("Nivel_1");                // Carga la escena
    }

    public void Salir()
    {
        Debug.Log("JuegoCerrado...");
        Application.Quit();                               // Cierra la aplicacion
        UnityEditor.EditorApplication.isPlaying = false;  // Detiene el juego si se esta dentro del editor de Unity
    }

}
