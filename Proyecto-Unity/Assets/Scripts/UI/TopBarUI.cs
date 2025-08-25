using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopBarUI : MonoBehaviour
{
    public static TopBarUI Instancia { get; private set; }

    private Inventario inventario;

    [SerializeField] private TextMeshProUGUI dineroText;
    [SerializeField] private TextMeshProUGUI nombreText;
    [SerializeField] private TextMeshProUGUI aciertosText;
    [SerializeField] private TextMeshProUGUI desaciertosText;
    [SerializeField] private Button ajustesButton;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);

        inventario = FindObjectOfType<Inventario>();


        ActualizarNombre();
    }

    private void Update()
    {
        if (inventario == null)
            inventario = FindObjectOfType<Inventario>();

        if (inventario != null)
        {
            if (dineroText != null)
                dineroText.text = $"$ {inventario.ObtenerDinero()}";

            if (aciertosText != null)
                aciertosText.text = $"Aciertos: {inventario.ObtenerAciertos()}";

            if (desaciertosText != null)
                desaciertosText.text = $"Desaciertos: {inventario.ObtenerDesaciertos()}";
        }
    }

    public void ActualizarNombre()
    {
        if (nombreText != null)
            nombreText.text = $"Nombre: {PlayerPrefs.GetString("NombreJugador", "Jugador")}";
    }

    
}