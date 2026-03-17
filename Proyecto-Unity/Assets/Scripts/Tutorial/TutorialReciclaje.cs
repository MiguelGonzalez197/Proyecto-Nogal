using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialReciclaje : MonoBehaviour
{
    private static readonly WaitForSeconds EsperaBusquedaModulo = new WaitForSeconds(0.25f);
    private const int ClipsInicialesOmitidos = 3;
    private const int BolsasSeparacionTutorial = 2;

    [Header("General")]
    [SerializeField] private string escenaPrincipal = "SampleScene";
    [SerializeField] private ModuloSeparacion moduloSeparacion;
    [SerializeField] private ModuloCompra moduloCompra;
    [SerializeField] private ModuloCrafteo moduloCrafteo;
    [SerializeField] private Inventario inventario;
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource audioSrc;

    [Header("Referencia pantalla de carga")]
    [SerializeField] private GameObject pantallaCarga;
    [SerializeField] private GameObject menuPrincipal;

    private const string PrefTutorial = "TutorialCompletado";
    private const string PrefNombreJugador = "NombreJugador";
    private const string PrefEdadJugador = "EdadJugador";

    private Collider colliderModuloSeparacion;
    private Collider colliderModuloCompra;
    private Collider colliderModuloCrafteo;

    private static GameObject interaccionBlanca;
    private static GameObject interaccionVerde;
    private static GameObject interaccionNegra;

    private GameManager gameManager;
    private int indiceClipActual;

    private void Awake()
    {
        ResolverInteracciones();
        DesactivarInteracciones();
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt(PrefTutorial, 0) == 1)
        {
            PrepararEscenaPrincipal();
            return;
        }

        StartCoroutine(EsperarNombreEIniciar());
    }

    public void SaltarTutorial()
    {
        PlayerPrefs.SetInt(PrefTutorial, 1);
        PlayerPrefs.Save();
        PrepararEscenaPrincipal();
    }

    public static void DesactivarInteracciones()
    {
        SetInteracciones(false, false, false);
    }

    public static void MostrarMensajeResiduo(GameObject item)
    {
        if (PlayerPrefs.GetInt(PrefTutorial, 0) == 1) return;

        Item itemComp = item.GetComponent<Item>();
        if (itemComp == null)
        {
            DesactivarInteracciones();
            return;
        }

        DatosItem datos = itemComp.ObtenerDatosItem();
        string nombre = item.name.Replace("(Clone)", string.Empty).Trim();
        string mensaje;

        switch (datos.tipoReciclaje)
        {
            case ETipoReciclaje.OrganicoAprovechable:
                if (datos.tipoItem == ETipoItem.Bolsa)
                {
                    mensaje = "Esta bolsa verde ya trae residuos orgánicos separados. Bótala completa en la caneca verde sin abrirla.";
                }
                else
                {
                    mensaje = nombre + " es un residuo orgánico que puede convertirse en abono. Debe ir en la caneca verde.";
                }

                ActivarSoloInteraccion(interaccionVerde);
                break;

            case ETipoReciclaje.Aprovechable:
                if (datos.tipoItem == ETipoItem.Bolsa)
                {
                    mensaje = "Esta bolsa blanca contiene materiales reciclables limpios. Deposítala entera en la caneca blanca.";
                }
                else
                {
                    mensaje = nombre + " está limpio y en buen estado. Puede reciclarse o reutilizarse, así que va en la caneca blanca.";
                }

                ActivarSoloInteraccion(interaccionBlanca);
                break;

            case ETipoReciclaje.NoAprovechable:
                if (datos.tipoItem == ETipoItem.Bolsa)
                {
                    mensaje = "Esta bolsa no ha sido clasificada. Debías abrirla y separar su contenido antes de botarla.";
                    SetInteracciones(true, true, true);
                }
                else
                {
                    mensaje = nombre + " está sucio, contaminado o deteriorado. Debe ir en la caneca negra.";
                    ActivarSoloInteraccion(interaccionNegra);
                }
                break;

            default:
                DesactivarInteracciones();
                return;
        }

        GestorCajaTexto.Instancia?.MostrarMensaje(mensaje, 5f, true);
    }

    private IEnumerator EsperarNombreEIniciar()
    {
        yield return new WaitUntil(() =>
            !string.IsNullOrEmpty(PlayerPrefs.GetString(PrefNombreJugador, string.Empty))
            && PlayerPrefs.GetInt(PrefEdadJugador, 0) > 0
        );

        ResolverReferenciasEscena();
        indiceClipActual = clips != null ? Mathf.Clamp(ClipsInicialesOmitidos, 0, clips.Length) : 0;
        SetColliders(false, false, false);
        StartCoroutine(MostrarTutorial());
    }

    private IEnumerator MostrarTutorial()
    {
        yield return MostrarPasoTutorial(
            "La caneca verde representa los residuos orgánicos biodegradables, como restos de comida y cáscaras, que pueden convertirse en abono natural mediante compostaje.",
            10f
        );

        yield return MostrarPasoTutorial(
            "La caneca blanca se usa para los materiales aprovechables no orgánicos, como papel limpio, plástico, metal o vidrio, que pueden reciclarse o reutilizarse.",
            10f
        );

        yield return MostrarPasoTutorial(
            "La caneca negra se reserva para los residuos no aprovechables, es decir, los que están contaminados o deteriorados y terminarán en un relleno sanitario.",
            10f
        );

        if (moduloSeparacion != null)
        {
            SetColliders(true, false, false);
            moduloSeparacion.Interactuar();
            moduloSeparacion.ConfigurarNumeroBolsasParaSesion(BolsasSeparacionTutorial);
        }

        yield return MostrarPasoTutorial(
            "Este es el módulo de separación. Aquí obtienes bolsas y decides qué hacer con su contenido.",
            4f
        );

        yield return MostrarPasoTutorial(
            "Puedes abrir cada bolsa para revisar sus residuos o botarla directamente en la caneca correcta.",
            5f,
            false
        );

        yield return MostrarPasoTutorial(
            "Las bolsas negras siempre deben abrirse para separar lo que contengan.",
            10f
        );

        yield return MostrarPasoTutorial(
            "Si una bolsa ya trae todos los residuos separados, puedes usar el botón 'Botar bolsa'.",
            11f
        );

        if (moduloSeparacion != null)
        {
            GestorCajaTexto.Instancia?.MostrarMensaje("Empieza separando estas bolsas.", 5f, true);
            yield return new WaitUntil(() => moduloSeparacion.HaClasificadoTodasLasBolsas);
            SetColliders(false, false, false);
        }

        yield return new WaitForSeconds(0.5f);

        if (moduloCompra != null)
        {
            SetColliders(false, true, false);
            moduloCompra.Interactuar();
        }

        yield return MostrarPasoTutorial(
            "Este es el módulo de compra. Aquí puedes adquirir más mesas, accesorios y futuras mejoras para tu bodega.",
            11f
        );

        inventario?.AsegurarDinero(30000);

        yield return MostrarPasoTutorial(
            "Te hemos entregado 30.000 monedas para que compres la mesa de crafteo.",
            17f
        );

        yield return MostrarPasoTutorial(
            "Compra la mesa de crafteo en el menú Módulos para continuar.",
            6f
        );

        yield return EsperarModuloCrafteoComprado();

        yield return MostrarPasoTutorial(
            "Perfecto, ya tienes la mesa de crafteo. Ahora pasemos a aprender sobre ella.",
            7f
        );

        if (moduloCompra != null)
        {
            moduloCompra.SalirModuloCallback();
            SetColliders(false, false, false);
        }

        yield return new WaitForSeconds(0.5f);

        if (moduloCrafteo != null)
        {
            SetColliders(false, false, true);
            moduloCrafteo.Interactuar();
        }

        yield return MostrarPasoTutorial(
            "En el módulo de crafteo podrás crear objetos con los residuos reutilizables que clasificaste. Por ahora esta mecánica sigue en desarrollo.",
            7f,
            false
        );

        if (moduloCrafteo != null)
        {
            moduloCrafteo.SalirModuloCallback();
        }

        SetColliders(true, true, true);
        PlayerPrefs.SetInt(PrefTutorial, 1);
        PlayerPrefs.Save();

        yield return new WaitForSeconds(0.5f);
        PrepararEscenaPrincipal();
    }

    private IEnumerator MostrarPasoTutorial(string mensaje, float duracion, bool reproducirAudio = true)
    {
        GestorCajaTexto.Instancia?.MostrarMensaje(mensaje, duracion, true);
        ReproducirClipActual(reproducirAudio);
        yield return new WaitForSeconds(duracion);
    }

    private IEnumerator EsperarModuloCrafteoComprado()
    {
        while (true)
        {
            if (moduloCrafteo == null)
            {
                moduloCrafteo = FindObjectOfType<ModuloCrafteo>();
            }

            if (ModuloCrafteoDisponible())
            {
                yield break;
            }

            yield return EsperaBusquedaModulo;
        }
    }

    private void ResolverReferenciasEscena()
    {
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }

        if (moduloSeparacion == null)
        {
            moduloSeparacion = FindObjectOfType<ModuloSeparacion>();
        }

        if (moduloCompra == null)
        {
            moduloCompra = FindObjectOfType<ModuloCompra>();
        }

        if (moduloCrafteo == null)
        {
            moduloCrafteo = FindObjectOfType<ModuloCrafteo>();
        }

        if (inventario == null)
        {
            inventario = FindObjectOfType<Inventario>();
        }

        colliderModuloSeparacion = moduloSeparacion != null ? moduloSeparacion.GetComponent<Collider>() : null;
        colliderModuloCompra = moduloCompra != null ? moduloCompra.GetComponent<Collider>() : null;
        colliderModuloCrafteo = moduloCrafteo != null ? moduloCrafteo.GetComponent<Collider>() : null;

        if (GestorDatos.instancia != null)
        {
            GestorDatos.instancia.RegistrarDatosJugador(
                PlayerPrefs.GetString(PrefNombreJugador, "Jugador"),
                PlayerPrefs.GetInt(PrefEdadJugador, 0)
            );
        }
    }

    private void ResolverInteracciones()
    {
        if (interaccionBlanca == null)
        {
            interaccionBlanca = GameObject.Find("InteraccionB");
        }

        if (interaccionVerde == null)
        {
            interaccionVerde = GameObject.Find("InteraccionV");
        }

        if (interaccionNegra == null)
        {
            interaccionNegra = GameObject.Find("InteraccionN");
        }
    }

    private bool ModuloCrafteoDisponible()
    {
        return moduloCrafteo != null
            && moduloCrafteo.gameObject.activeSelf
            && (moduloCompra == null || !moduloCrafteo.transform.IsChildOf(moduloCompra.transform));
    }

    private void ReproducirClipActual(bool reproducirAudio)
    {
        if (!reproducirAudio || audioSrc == null || clips == null || indiceClipActual >= clips.Length) return;

        audioSrc.clip = clips[indiceClipActual];
        audioSrc.Play();
        indiceClipActual++;
    }

    private void SetColliders(bool separacion, bool compra, bool crafteo)
    {
        if (colliderModuloSeparacion != null) colliderModuloSeparacion.enabled = separacion;
        if (colliderModuloCompra != null) colliderModuloCompra.enabled = compra;
        if (colliderModuloCrafteo != null) colliderModuloCrafteo.enabled = crafteo;
    }

    private static void SetInteracciones(bool blanca, bool verde, bool negra)
    {
        if (interaccionBlanca != null) interaccionBlanca.SetActive(blanca);
        if (interaccionVerde != null) interaccionVerde.SetActive(verde);
        if (interaccionNegra != null) interaccionNegra.SetActive(negra);
    }

    private static void ActivarSoloInteraccion(GameObject interaccion)
    {
        SetInteracciones(
            interaccion == interaccionBlanca,
            interaccion == interaccionVerde,
            interaccion == interaccionNegra
        );
    }

    private void PrepararEscenaPrincipal()
    {
        if (pantallaCarga == null || menuPrincipal == null || string.IsNullOrEmpty(escenaPrincipal)) return;

        if (GestorDatos.instancia != null)
        {
            GestorDatos.instancia.RegistrarExitoTutorial(true);
        }

        pantallaCarga.SetActive(true);
        menuPrincipal.SetActive(false);
        StartCoroutine(CargarNivel());
    }

    private IEnumerator CargarNivel()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(escenaPrincipal);

        while (!loadOperation.isDone)
        {
            yield return null;
        }
    }
}
