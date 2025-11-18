using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] GameObject goblinPrefab;
    [SerializeField] int maxGoblins = 4;
    [SerializeField] float spawnCooldown = 3f;
    [SerializeField] float spawnRadius = 5f;   // radio máximo permitido alrededor de la casa
    [SerializeField] float areaX = 2f;         // ancho del área de spawn
    [SerializeField] float areaY = 2f;         // alto del área de spawn

    private float spawnTimer;
    private List<GameObject> goblins = new List<GameObject>();


    // Update is called once per frame
    void Update()
    {

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
    private void OnDrawGizmosSelected() //Ver rango de busqueda del player
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, spawnRadius);
    }
    
    void SpawnGoblin()
    {
        float xPos = (Random.value - 0.5f) * 2 * areaX + transform.position.x;
        float yPos = (Random.value - 0.5f) * 2 * areaY + transform.position.y;

        GameObject newGoblin = Instantiate(goblinPrefab, new Vector3(xPos, yPos, 0), Quaternion.identity);
        goblins.Add(newGoblin);
    }

}
