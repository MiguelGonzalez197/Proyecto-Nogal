using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TopBarUI : MonoBehaviour
{
    public static TopBarUI Instancia { get; private set; }

    private const string PrefNombreJugador = "NombreJugador";
    private const string PrefEdadJugador = "EdadJugador";

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

        SceneManager.sceneLoaded += OnSceneLoaded;
        ResolverInventario();
        ActualizarNombre();
        ActualizarInventarioUI();
    }

    private void OnDestroy()
    {
        if (Instancia != this) return;

        SceneManager.sceneLoaded -= OnSceneLoaded;
        DesvincularInventario();
        Instancia = null;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ResolverInventario();
        ActualizarNombre();
    }

    public void ActualizarNombre()
    {
        string nombreJugador = PlayerPrefs.GetString(PrefNombreJugador, "Jugador");

        if (nombreText != null)
        {
            nombreText.text = "Nombre: " + nombreJugador;
        }

        if (GestorDatos.instancia != null)
        {
            GestorDatos.instancia.RegistrarDatosJugador(
                nombreJugador,
                PlayerPrefs.GetInt(PrefEdadJugador, 0)
            );
        }
    }

    private void ResolverInventario()
    {
        Inventario nuevoInventario = FindObjectOfType<Inventario>();
        if (nuevoInventario == inventario) return;

        DesvincularInventario();
        inventario = nuevoInventario;

        if (inventario != null)
        {
            inventario.EnInventarioActualizado += ActualizarInventarioUI;
        }

        ActualizarInventarioUI();
    }

    private void DesvincularInventario()
    {
        if (inventario == null) return;

        inventario.EnInventarioActualizado -= ActualizarInventarioUI;
        inventario = null;
    }

    private void ActualizarInventarioUI()
    {
        if (dineroText != null)
        {
            dineroText.text = inventario != null ? "$ " + inventario.ObtenerDinero() : "$ 0";
        }

        if (aciertosText != null)
        {
            aciertosText.text = inventario != null ? "Aciertos: " + inventario.ObtenerAciertos() : "Aciertos: 0";
        }

        if (desaciertosText != null)
        {
            desaciertosText.text = inventario != null ? "Desaciertos: " + inventario.ObtenerDesaciertos() : "Desaciertos: 0";
        }
    }
}
