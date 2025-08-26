using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NombreJugadorInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputNombre;
    [SerializeField] private Button confirmarButton;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject topBar;

    private void Awake()
    {
        bool tieneNombre = PlayerPrefs.HasKey("NombreJugador");
        if (panel != null)
            panel.SetActive(!tieneNombre);
        if (topBar != null)
            topBar.SetActive(tieneNombre);
    }

    private void Start()
    {
        if (confirmarButton != null)
            confirmarButton.onClick.AddListener(GuardarNombre);
    }

    private void GuardarNombre()
    {
        if (inputNombre == null) return;

        string nombre = string.IsNullOrWhiteSpace(inputNombre.text) ? "Jugador" : inputNombre.text;
        PlayerPrefs.SetString("NombreJugador", nombre);
        PlayerPrefs.Save();

        if (panel != null)
            panel.SetActive(false);

        if (topBar != null)
            topBar.SetActive(true);

        TopBarUI barra = FindObjectOfType<TopBarUI>();
        if (barra != null)
            barra.ActualizarNombre();
    }
}