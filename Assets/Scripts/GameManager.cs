using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;        // Permite acceder a este script desde cualquier otro con GameManager.Instance

    [Header("Referencias al Jugador")]
    [SerializeField] private PlayerControler playerControler;

    [Header("Interfaz de Usuario (UI)")]
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button reiniciarButton;
    [SerializeField] private Button menuButton;

    public bool gameOverActivo = false;
    public bool winningActivo = false;
    private Vector3 posicionReaparicion;

    private void Awake()
    {
        if(Instance == null)     // Configuración del patrón Singleton
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    private void Start()
    {
        InicializarJugador();
        InicializarUI();
    }

    private void Update()
    {
        if (gameOverActivo)                                                          // Solo leer imputs si es GameOver
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReiniciarEscena();
            }
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.M))
            {
                IrAlMenu();   
            }
            if (winningActivo)
            {
                if (Input.GetKeyDown(KeyCode.Escape)) IrAlMenu();
            }
        }
    }

    private void InicializarJugador()
    {
        if (playerControler == null)                                                // Si no se asigno el jugador manualmnete, se lo busca
        {
            playerControler = FindObjectOfType<PlayerControler>();
        }
        if (playerControler != null && posicionReaparicion == Vector3.zero)         // Establece el primer checkpoint en la posición inicial del jugador
        {
            posicionReaparicion = playerControler.transform.position;
        }
        else
        {
            Debug.LogError("Error en GameManager: No se encontro al PlayerControler en la escena");
        }
    }

    private void InicializarUI()
    {
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
        if (winScreen != null)
            winScreen.SetActive(false);
        if (reiniciarButton != null)
            reiniciarButton.onClick.AddListener(ReiniciarEscena);
        if (menuButton != null)
            menuButton.onClick.AddListener(IrAlMenu);
    }

    public void EstablecerCheckpoint(Vector3 nuevaPosicion)
    {
        posicionReaparicion = nuevaPosicion;
    }

    public void Winning()
    {
        if (winningActivo || gameOverActivo) return;

        winningActivo = true;
        Time.timeScale = 0f;
        if (winScreen != null)
        {
            winScreen.SetActive(true);       // Activa el canvas winScreen
        }
        Debug.Log("¡VICTORIA!");
    }

    public void GameOver()
    {
        if (winningActivo || gameOverActivo) return;              // Evita que se llame múltiples veces

        gameOverActivo = true;
        Time.timeScale = 0f;                     // Pausa el juego
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);       // Activa el canvas gameOverPanel
        }
        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER\n\nR - Revivir \nESC - Menu Principal";
        }
    }

    public void ReiniciarEscena()
    {
        if (playerControler == null)
        {
            Debug.LogError("El PlayerControler no está asignado en GameManager. ¡No se puede revivir!");
            return;
        }
        Time.timeScale = 1f;                          // El tiempo vuelve a la normalidad

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);           // Ocultar el panel de Game Over
        }
        gameOverActivo = false;
        playerControler.Revivir(posicionReaparicion); // Revivir al personaje
    }

    public void IrAlMenu()
    {
        Time.timeScale = 1f;                          // El tiempo vuelve a la normalidad
        SceneManager.LoadScene("MenuPrincipal");      // Carga la escena
    }
}
