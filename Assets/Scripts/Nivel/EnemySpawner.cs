using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject goblinPrefab;
    [SerializeField] GameObject moneyPrefab;
    [SerializeField] int maxGoblins = 4;
    [SerializeField] float spawnCooldown = 3f;
    [SerializeField] float spawnRadius = 5f;   // radio máximo permitido alrededor de la casa
    [SerializeField] float areaX = 2f;         // ancho del área de spawn
    [SerializeField] float areaY = 2f;         // alto del área de spawn
    [SerializeField] int vida = 5;             // vida de la choza
    [SerializeField] Sprite spriteDestruida;   // imagen de la choza rota
    [Header("Audio")]
    [SerializeField] AudioClip sonidoGolpe;
    [SerializeField] AudioClip sonidoDestruccion;

    private float spawnTimer;
    private List<GameObject> goblins = new List<GameObject>();
    private List<GameObject> money = new List<GameObject>();
    private bool destruida = false;
    private Collider2D col;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private float variacionPitch = 0.2f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        col = GetComponent<Collider2D>();
    }

    void Update()
    {
        // Si la choza está destruida, no hacemos nada más.
        if (destruida) return;

        // limpiar lista de goblins muertos (Destroy los deja en null)
        goblins.RemoveAll(g => g == null);

        // contar goblins dentro del radio
        int goblinsEnRango = 0;
        foreach (var g in goblins)
        {
            if (Vector2.Distance(transform.position, g.transform.position) <= spawnRadius)
                goblinsEnRango++;
        }
        // si hay menos goblins en rango que el máximo, iniciar cooldown
        if (goblinsEnRango < maxGoblins)
        {
            spawnTimer -= Time.deltaTime;
            if (spawnTimer <= 0f)
            {
                SpawnGoblin();
                spawnTimer = spawnCooldown;
            }
        }
        else
        {
            spawnTimer = spawnCooldown; // reset si está completo
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si ya está destruida, ignoramos golpes
        if (destruida) return;

        if (collision.CompareTag("Espada"))
        {
            // Buscamos el script del jugador para saber cuánto daño hace
            PlayerControler playerScript = collision.GetComponentInParent<PlayerControler>();

            if (playerScript != null)
            {
                RecibirDanio(playerScript.damage);
            }
        }
    }

    public void RecibirDanio(int cantidad)
    {
        vida -= cantidad;

        if (audioSource != null && sonidoGolpe != null)
        {
            audioSource.pitch = Random.Range(1f - variacionPitch, 1f + variacionPitch);
            audioSource.PlayOneShot(sonidoGolpe);
        }

        if (vida <= 0)
        {
            DestruirChoza();
        }
    }

    void DestruirChoza()
    {
        destruida = true;

        // Cambiar el Sprite
        if (spriteRenderer != null && spriteDestruida != null)
        {
            spriteRenderer.sprite = spriteDestruida;
            GameObject newMoney = Instantiate(moneyPrefab, new Vector3(transform.position.x, transform.position.y - 1, 0), Quaternion.identity);
            money.Add(newMoney);
        }
        if (col != null)
        {
            col.enabled = false;
        }
        if (audioSource != null && sonidoDestruccion != null)
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(sonidoDestruccion);
        }
        Debug.Log("¡La choza ha sido destruida!");
    }
    
    void SpawnGoblin()
    {
        float xPos = (Random.value - 0.5f) * 2 * areaX + transform.position.x;
        float yPos = (Random.value - 0.5f) * 2 * areaY + transform.position.y;

        GameObject newGoblin = Instantiate(goblinPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
        goblins.Add(newGoblin);
    }

    private void OnDrawGizmosSelected() //Ver rango de busqueda del player
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaX * 2, areaY * 2, 1));
    }
}
