using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;
    public Button reiniciarButton;
    public Button menuButton;
    //private PlayerControler playerControler;
    public PlayerControler playerControler;
    private bool gameOverActivo = false;

    private Vector3 posicionReaparicion;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }
    void Start()
    {

        //playerControler = player.GetComponent<PlayerControler>();
        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        if (reiniciarButton != null)
            reiniciarButton.onClick.AddListener(ReiniciarEscena);

        if (menuButton != null)
            menuButton.onClick.AddListener(IrAlMenu);
        if (playerControler == null)
        {
            playerControler = FindObjectOfType<PlayerControler>();
        }

        if (playerControler != null && posicionReaparicion == Vector3.zero) // Solo si no se ha establecido
        {
            posicionReaparicion = playerControler.transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOverActivo)
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                ReiniciarEscena();
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.M))
            {
                IrAlMenu();   
            }
        }
    }

    public void GameOver()
    {
        if (gameOverActivo) return;

        gameOverActivo = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
        Time.timeScale = 0f;
        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER\n\nR - Reiniciar \nESC - Menu Principal";
        }
    }
    public void EstablecerCheckpoint(Vector3 nuevaPosicion)
    {
        posicionReaparicion = nuevaPosicion;
    }

    public void ReiniciarEscena() //reinicia la escena
    {
        /*
        Time.timeScale = 1f;
        //playerControler.vida = 5;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //(SceneManager.LoadScene) = carga la Escena y (SceneManger.GetActiveScene().name) = obtine el nombre de la escena actual
        */
        if (playerControler == null)
        {
            Debug.LogError("El PlayerControler no está asignado en GameManager. ¡No se puede revivir!");
            return;
        }

        // 1. Revivir al personaje
        playerControler.Revivir(posicionReaparicion);

        // 2. Ocultar el panel de Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
        gameOverActivo = false;

        // 3. Reanudar el tiempo (si se detuvo)
        Time.timeScale = 1f;
    }
    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
