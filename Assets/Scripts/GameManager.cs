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
     
    private bool gameOverActivo = false;

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

        if (gameOverText != null)
        {
            gameOverText.text = "GAME OVER\n\nR - Reiniciar \nESC - Menu Principal";
        }
    }

    public void ReiniciarEscena() //reinicia la escena
    {
        Time.timeScale = 1f;
        //playerControler.vida = 5;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //(SceneManager.LoadScene) = carga la Escena y (SceneManger.GetActiveScene().name) = obtine el nombre de la escena actual
    }
    public void IrAlMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal");
    }
}
