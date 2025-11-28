using UnityEngine;
using TMPro;

public class MoneyDamageBar : MonoBehaviour
{
    [Header("Referencias a Textos")]
    [SerializeField] private TextMeshProUGUI textoMonedas;
    [SerializeField] private TextMeshProUGUI textoDanio;

    [Header("Referencia al Jugador")]
    [SerializeField] private PlayerControler player;

    private int ultimoDinero = -1;
    private int ultimoDanio = -1;

    void Start()
    {
        if (player == null)
        {
            player = FindObjectOfType<PlayerControler>();
        }
    }

    void Update()
    {
        if (player == null) return;

        if (player.money != ultimoDinero) // Verifica si el dinero cambio
        {
            ultimoDinero = player.money;
            textoMonedas.text = $"x {ultimoDinero}";
        }

        if (player.damage != ultimoDanio)
        {
            ultimoDanio = player.damage;
            textoDanio.text = $"x {ultimoDanio}";
        }
    }
}
