using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuSystem : MonoBehaviour
{
    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "MenuPrincipal" && Input.GetKeyDown(KeyCode.JoystickButton7))      // Si Escape fue precionado alterna entre
        {
            Jugar();
        }
        if (SceneManager.GetActiveScene().name == "MenuPrincipal" && Input.GetKeyDown(KeyCode.JoystickButton6))      // Si Escape fue precionado alterna entre
        {
            Salir();
        }

    }
    public void Jugar() 
    {
        SceneManager.LoadScene("Nivel_1");                // Carga la escena
    }

    public void Salir()
    {
        Debug.Log("JuegoCerrado...");
        Application.Quit();                               // Cierra la aplicacion
        //UnityEditor.EditorApplication.isPlaying = false;  // Detiene el juego si se esta dentro del editor de Unity
    }

}
