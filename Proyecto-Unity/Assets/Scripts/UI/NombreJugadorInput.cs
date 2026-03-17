using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NombreJugadorInput : MonoBehaviour
{
    private const string PrefNombreJugador = "NombreJugador";
    private const string PrefEdadJugador = "EdadJugador";

    [SerializeField] private TMP_InputField inputNombre;
    [SerializeField] private Button confirmarButton;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject topBar;

    private void Awake()
    {
        bool tieneNombre = PlayerPrefs.HasKey(PrefNombreJugador);
        bool tieneEdad = PlayerPrefs.HasKey(PrefEdadJugador);

        if (panel != null)
        {
            panel.SetActive(!tieneNombre);
        }

        EstablecerTopBarActivo(tieneNombre && tieneEdad);
    }

    private void Start()
    {
        if (confirmarButton != null)
        {
            confirmarButton.onClick.AddListener(GuardarNombre);
        }
    }

    private void OnDestroy()
    {
        if (confirmarButton != null)
        {
            confirmarButton.onClick.RemoveListener(GuardarNombre);
        }
    }

    private void GuardarNombre()
    {
        if (inputNombre == null) return;

        string nombre = string.IsNullOrWhiteSpace(inputNombre.text) ? "Jugador" : inputNombre.text.Trim();
        PlayerPrefs.SetString(PrefNombreJugador, nombre);
        PlayerPrefs.Save();

        if (panel != null)
        {
            panel.SetActive(false);
        }

        TopBarUI.Instancia?.ActualizarNombre();
        EdadJugadorInput.Instancia?.MostrarPanelEdad();

        if (GestorDatos.instancia != null)
        {
            GestorDatos.instancia.RegistrarDatosJugador(
                nombre,
                PlayerPrefs.GetInt(PrefEdadJugador, 0)
            );
        }
    }

    private void EstablecerTopBarActivo(bool activo)
    {
        if (topBar != null)
        {
            topBar.SetActive(activo);
        }

        if (TopBarUI.Instancia != null)
        {
            TopBarUI.Instancia.gameObject.SetActive(activo);
        }
    }
}
