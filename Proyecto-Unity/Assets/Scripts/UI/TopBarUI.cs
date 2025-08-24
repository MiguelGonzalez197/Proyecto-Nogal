using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TopBarUI : MonoBehaviour
{
    private Inventario inventario;

    [SerializeField] private TextMeshProUGUI dineroText;
    [SerializeField] private TextMeshProUGUI nombreText;
    [SerializeField] private TextMeshProUGUI aciertosText;
    [SerializeField] private TextMeshProUGUI desaciertosText;
    [SerializeField] private Button ajustesButton;

    private void Awake()
    {
        inventario = FindObjectOfType<Inventario>();

        if (ajustesButton != null)
            ajustesButton.onClick.AddListener(AbrirAjustes);

        ActualizarNombre();
    }

    private void Update()
    {
        if (inventario == null)
            inventario = FindObjectOfType<Inventario>();

        if (inventario != null)
        {
            if (dineroText != null)
                dineroText.text = $"Dinero: {inventario.ObtenerDinero()}";

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

    private void AbrirAjustes()
    {
        AjustesKeep ajustes = FindObjectOfType<AjustesKeep>();
        if (ajustes != null)
            ajustes.ToggleAjustes();
    }
}
