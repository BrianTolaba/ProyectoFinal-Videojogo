using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    [SerializeField] private GameObject menuPause; // Objeto menuPause del canvas
    public bool juegoPausado = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))      // Si Escape fue precionado alterna entre
        {
            AlternarPausa();
        }
        if (juegoPausado == true && Input.GetKeyDown(KeyCode.JoystickButton6))
        {
            IrAlMenu();
        }
    }
    public void AlternarPausa()
    {
        if (juegoPausado)
        {
            Reanudar();
        }
        else
        {
            Pausar();
        }
    }

    public void Reanudar() 
    {
        menuPause.SetActive(false);               // Desactiva el canvas menuPause
        Time.timeScale = 1;                       // El tiempo vuelve a la normalidad
        AudioListener.pause = false;              // Reactiva el sonido
        juegoPausado = false;

    }

    public void Pausar()
    {
        menuPause.SetActive(true);               // Activa el canvas menuPause
        Time.timeScale = 0;                      // El tiempo se detiene
        AudioListener.pause = true;              // Pausa todo el sonido del juego
        juegoPausado = true;
    }

    public void IrAlMenu()
    {
        Reanudar();                              // Evita que se quede en pausa al cambiar entre escenas
        SceneManager.LoadScene("MenuPrincipal"); // Carga la escena
    }
}

