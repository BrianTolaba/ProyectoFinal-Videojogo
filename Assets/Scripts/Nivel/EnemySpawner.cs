using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuracion Spawn")]
    [SerializeField] GameObject goblinPrefab;
    [SerializeField] int maxGoblins = 4;
    [SerializeField] float spawnCooldown = 3f;
    [SerializeField] float spawnRadius = 5f;    // Distancia máx para contar goblins

    [Header("Area de Spawn")]
    [SerializeField] float areaX = 2f;
    [SerializeField] float areaY = 2f;

    [Header("Configuracion Vida")]
    [SerializeField] int vida = 5;
    [SerializeField] Sprite spriteDestruida;
    [SerializeField] GameObject moneyPrefab;

    [Header("Audio")]
    [SerializeField] AudioClip sonidoGolpe;
    [SerializeField] AudioClip sonidoDestruccion;

    private float spawnTimer;
    private List<GameObject> goblins = new List<GameObject>();
    private bool destruida = false;

    private Collider2D col;
    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private float variacionPitch = 0.2f;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        col = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if (destruida) return;                                // Si la choza esta destruida, no hace nada mas

        goblins.RemoveAll(g => g == null);                    // Limpiar lista de goblins muertos

        int goblinsEnRango = 0;
        foreach (var g in goblins)                            // Contar goblins dentro del radio
        {
            if (Vector2.Distance(transform.position, g.transform.position) <= spawnRadius)
            {
                goblinsEnRango++;
            }
        }
        if (goblinsEnRango < maxGoblins)                      // Generar nuevo enemigo si hay espacio
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
            spawnTimer = spawnCooldown;                      // Reset si está completo
        }
    }
    private void SpawnGoblin()
    {
        if (goblinPrefab == null) return;

        float randomX = (Random.value - 0.5f) * 2 * areaX + transform.position.x;
        float randomY = (Random.value - 0.5f) * 2 * areaY + transform.position.y;
        GameObject newGoblin = Instantiate(goblinPrefab, new Vector3(randomX, randomY, 0), Quaternion.identity);
        goblins.Add(newGoblin);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destruida) return;                               // Si ya esta destruida, ignora golpes

        if (collision.CompareTag("Espada"))
        {
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

    private void DestruirChoza()
    {
        destruida = true;
        gameObject.tag = "Untagged";

        // Cambiar el Sprite
        if (spriteRenderer != null && spriteDestruida != null)
        {
            spriteRenderer.sprite = spriteDestruida;
        }
        // Loot (Moneda)
        if (moneyPrefab != null)
        {
            Instantiate(moneyPrefab, transform.position + Vector3.down, Quaternion.identity); //new Vector3(transform.position.x, transform.position.y - 1, 0)
        }
        // Desactivar fisica
        if (col != null)
        {
            col.enabled = false;
        }
        // Sonido final
        if (audioSource != null && sonidoDestruccion != null)
        {
            audioSource.pitch = 1f;
            audioSource.PlayOneShot(sonidoDestruccion);
        }
        Debug.Log("Choza destruida");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(areaX * 2, areaY * 2, 1));
    }
}
