using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EdadJugadorInput : MonoBehaviour
{
    public static EdadJugadorInput Instancia { get; private set; }

    private const string PrefNombreJugador = "NombreJugador";
    private const string PrefEdadJugador = "EdadJugador";
    private const string MensajeEdadInvalida = "Ingresa una edad valida usando solo numeros.";

    [SerializeField] private TMP_InputField inputEdad;
    [SerializeField] private Button confirmarButton;
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject topBar;
    [SerializeField] private TextMeshProUGUI textoAdvertencia;

    private void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        PrepararTextoAdvertencia();

        bool tieneNombre = PlayerPrefs.HasKey(PrefNombreJugador);
        bool tieneEdad = PlayerPrefs.HasKey(PrefEdadJugador);

        if (panel != null)
        {
            panel.SetActive(tieneNombre && !tieneEdad);
        }

        EstablecerTopBarActivo(tieneNombre && tieneEdad);
    }

    private void Start()
    {
        if (confirmarButton != null)
        {
            confirmarButton.onClick.AddListener(GuardarEdad);
        }

        if (inputEdad != null)
        {
            inputEdad.onValueChanged.AddListener(OnEdadCambiada);
        }
    }

    private void OnDestroy()
    {
        if (Instancia == this)
        {
            Instancia = null;
        }

        if (confirmarButton != null)
        {
            confirmarButton.onClick.RemoveListener(GuardarEdad);
        }

        if (inputEdad != null)
        {
            inputEdad.onValueChanged.RemoveListener(OnEdadCambiada);
        }
    }

    public void MostrarPanelEdad()
    {
        if (panel == null || PlayerPrefs.HasKey(PrefEdadJugador))
        {
            return;
        }

        OcultarAdvertencia();
        panel.SetActive(true);

        if (inputEdad != null)
        {
            inputEdad.text = string.Empty;
            inputEdad.ActivateInputField();
        }
    }

    private void GuardarEdad()
    {
        if (inputEdad == null) return;

        string edadTexto = inputEdad.text.Trim();
        if (!int.TryParse(edadTexto, out int edad) || edad <= 0)
        {
            MostrarAdvertencia(MensajeEdadInvalida);
            return;
        }

        OcultarAdvertencia();
        PlayerPrefs.SetInt(PrefEdadJugador, edad);
        PlayerPrefs.Save();

        if (panel != null)
        {
            panel.SetActive(false);
        }

        EstablecerTopBarActivo(true);
        TopBarUI.Instancia?.ActualizarNombre();

        if (GestorDatos.instancia != null)
        {
            GestorDatos.instancia.RegistrarDatosJugador(
                PlayerPrefs.GetString(PrefNombreJugador, "Jugador"),
                edad
            );
        }

        Debug.Log("Edad guardada: " + edad);
    }

    private void OnEdadCambiada(string _)
    {
        OcultarAdvertencia();
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

    private void PrepararTextoAdvertencia()
    {
        if (textoAdvertencia == null && panel != null)
        {
            GameObject advertencia = new GameObject("AdvertenciaEdad", typeof(RectTransform), typeof(TextMeshProUGUI));
            RectTransform rectTransform = advertencia.GetComponent<RectTransform>();
            rectTransform.SetParent(panel.transform, false);
            rectTransform.anchorMin = new Vector2(1f, 0f);
            rectTransform.anchorMax = new Vector2(1f, 0f);
            rectTransform.pivot = new Vector2(1f, 0f);
            rectTransform.anchoredPosition = new Vector2(-72f, 36f);
            rectTransform.sizeDelta = new Vector2(540f, 90f);

            textoAdvertencia = advertencia.GetComponent<TextMeshProUGUI>();
            textoAdvertencia.fontSize = 26f;
            textoAdvertencia.alignment = TextAlignmentOptions.BottomRight;
            textoAdvertencia.enableWordWrapping = true;
            textoAdvertencia.color = new Color32(92, 19, 15, 255);
            textoAdvertencia.raycastTarget = false;
        }

        OcultarAdvertencia();
    }

    private void MostrarAdvertencia(string mensaje)
    {
        if (textoAdvertencia == null)
        {
            return;
        }

        textoAdvertencia.text = mensaje;
        textoAdvertencia.gameObject.SetActive(true);
    }

    private void OcultarAdvertencia()
    {
        if (textoAdvertencia == null)
        {
            return;
        }

        textoAdvertencia.text = string.Empty;
        textoAdvertencia.gameObject.SetActive(false);
    }
}
