using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialReciclaje : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private string escenaPrincipal = "SampleScene";
    [SerializeField] private ModuloSeparacion moduloSeparacion;
    [SerializeField] private ModuloCompra moduloCompra;
    [SerializeField] private ModuloCrafteo moduloCrafteo;
    [SerializeField] private Inventario inventario;
    [SerializeField] private AudioClip[] clips;      // Asigna Audio1, Audio2, etc. en el Inspector.
    [SerializeField] private AudioSource audioSrc;


    [Header("Referencia pantalla de carga")]
    [SerializeField] private GameObject pantallaCarga;
    [SerializeField] private GameObject menuPrincipal;

    private const string PREF_TUTORIAL = "TutorialCompletado";
    private const string PREF_NOMBRE_JUGADOR = "NombreJugador";
 

    private Collider colliderModuloSeparacion;
    private Collider colliderModuloCompra;
    private Collider colliderModuloCrafteo;

    private static GameObject interaccionBlanca;
    private static GameObject interaccionVerde;
    private static GameObject interaccionNegra;

    private GameManager gameManager;
    private int indice = 0;

    private void Awake()
    {
        interaccionBlanca = GameObject.Find("InteraccionB");
        interaccionVerde = GameObject.Find("InteraccionV");
        interaccionNegra = GameObject.Find("InteraccionN");
        DesactivarInteracciones();
    }
    void Start()
    {
        if (PlayerPrefs.GetInt(PREF_TUTORIAL, 0) == 1)
        {
            PrepararEscenaPrincipal();
            //SceneManager.LoadScene(escenaPrincipal);
            return;
        }
        
        StartCoroutine(EsperarNombreEIniciar());
    }

    private IEnumerator EsperarNombreEIniciar()
    {
        
        yield return new WaitUntil(() => !string.IsNullOrEmpty(PlayerPrefs.GetString(PREF_NOMBRE_JUGADOR, "")));
      
        gameManager = FindObjectOfType<GameManager>();
        moduloSeparacion = FindObjectOfType<ModuloSeparacion>();
        moduloCompra = FindObjectOfType<ModuloCompra>();
        moduloCrafteo = FindObjectOfType<ModuloCrafteo>();
        inventario = FindObjectOfType<Inventario>();
        colliderModuloSeparacion = moduloSeparacion?.GetComponent<Collider>();
        colliderModuloCompra = moduloCompra?.GetComponent<Collider>();
        colliderModuloCrafteo = moduloCrafteo?.GetComponent<Collider>();
        
        if (gameManager != null)
        {
            //gameManager.BloquearCamara();
        }
        SetColliders(false, false, false);
        StartCoroutine(MostrarTutorial());
    }
   

    public void SaltarTutorial()
    {
        PlayerPrefs.SetInt(PREF_TUTORIAL, 1);
        PlayerPrefs.Save();
        PrepararEscenaPrincipal();
        //SceneManager.LoadScene(escenaPrincipal);
    }

    private IEnumerator MostrarTutorial()
    {
        GestorCajaTexto.Instancia.MostrarMensaje(
            "Bienvenido a Ecomisión",
            10);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitForSeconds(10);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "El reciclaje es el proceso de recoger y transformar los residuos para darles una segunda vida, reduciendo la cantidad de basura que llega a los rellenos sanitarios y el uso de recursos naturales.",
            10);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitForSeconds(10);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "Separar correctamente los residuos desde casa permite que los materiales aprovechables lleguen limpios a los centros de reciclaje y disminuye la contaminación en nuestro entorno.",
            9);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitForSeconds(9);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "La caneca verde representa los residuos orgánicos biodegradables, como restos de comida y cáscaras, que pueden convertirse en abono natural mediante compostaje.",
           10);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitForSeconds(10);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "La caneca blanca se usa para los materiales aprovechables no orgánicos como papeles limpios, plásticos, metales o vidrios que pueden reciclarse o reutilizarse.",
            10);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitForSeconds(10);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "La caneca negra se reserva para los residuos no aprovechables, aquellos que están contaminados, sucios o deteriorados y que terminarán en un relleno sanitario.",
            10);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitForSeconds(10);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "Ejemplos para la caneca verde: manzana, pan, brócoli, hamburguesa, sándwich de pollo, huevos, lechuga o arroz; todos son restos que se descomponen fácilmente.",
            10);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitForSeconds(10);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "Ejemplos para la caneca blanca: libro, botella de plástico limpia, botella de vidrio en buen estado, martillo, palanca, caja de cartón, pocillo o sartén sin óxido; son materiales que pueden reciclarse o reutilizarse.",
            13);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitForSeconds(13);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "Ejemplos para la caneca negra: botella sucia, lata de comida sucia, lata de pintura, caneca de pintura, caja de pizza sucia o lata oxidada; al estar contaminados no pueden aprovecharse.",
            9);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitForSeconds(9);
        if (moduloSeparacion != null)
        {
            SetColliders(true, false, false);
            moduloSeparacion.Interactuar();
        }
        GestorCajaTexto.Instancia.MostrarMensaje(
            "Este es el módulo de separación, aquí obtienes bolsas y decides qué hacer con su contenido.",
            4);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitForSeconds(4);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "Puedes abrir cada bolsa para revisar sus residuos o botarla directamente en la caneca correcta.",
            5);
        yield return new WaitForSeconds(5);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "Las bolsas negras siempre deben abrirse para separar lo que contengan.",
            10);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitForSeconds(10);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "Si una bolsa ya trae todos los residuos separados, puedes usar el botón 'Botar bolsa'.",
            11);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitForSeconds(11);

        if (moduloSeparacion != null)
        {
            GestorCajaTexto.Instancia.MostrarMensaje(
            "Empieza separando estas bolsas.",
            5);
            yield return new WaitUntil(() => moduloSeparacion.HaClasificadoTodasLasBolsas);
            SetColliders(false, false, false);
        }
        
        yield return new WaitForSeconds(0.5f);

        if (moduloCompra != null)
        {
            SetColliders(false, true, false);
            moduloCompra.Interactuar();
        }
        GestorCajaTexto.Instancia.MostrarMensaje(
            "Este es el módulo de compra. Aquí puedes adquirir más mesas, estilos de ropa que llegarán próximamente y accesorios para tu bodega.",
            11);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitForSeconds(11);

        inventario?.AsegurarDinero(30000);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "Te hemos entregado 30.000 monedas para que compres la mesa de crafteo.",
            17);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitForSeconds(17);

        GestorCajaTexto.Instancia.MostrarMensaje(
            "Compra la mesa de crafteo en el menú Modulos para continuar.",
            6);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitUntil(() =>
        {
            
            moduloCrafteo = FindObjectOfType<ModuloCrafteo>();
            return moduloCrafteo != null &&
                   moduloCrafteo.gameObject.activeSelf &&
                   (moduloCompra == null || !moduloCrafteo.transform.IsChildOf(moduloCompra.transform));
        });

        GestorCajaTexto.Instancia.MostrarMensaje(
            "Perfecto, ya tienes la mesa de crafteo. Ahora pasemos a aprender sobre ella.",
            7);
        audioSrc.clip = clips[indice];
        audioSrc.Play();
        indice++;
        yield return new WaitForSeconds(7);

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
        GestorCajaTexto.Instancia.MostrarMensaje(
            "En el módulo de crafteo, próximamente podrás crear objetos con los residuos reutilizables que clasificaste, aunque por ahora esta mecánica está en desarrollo.",
            7);
        yield return new WaitForSeconds(7);

        if (moduloCrafteo != null)
        {
            moduloCrafteo.SalirModuloCallback();
      
        }
        SetColliders(true, true, true);
        PlayerPrefs.SetInt(PREF_TUTORIAL, 1);
        PlayerPrefs.Save();
        yield return new WaitForSeconds(0.5f);
        PrepararEscenaPrincipal();
        //SceneManager.LoadScene(escenaPrincipal);

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

    public static void DesactivarInteracciones()
    {
        SetInteracciones(false, false, false);
    }

    private static void ActivarSoloInteraccion(GameObject interaccion)
    {
        SetInteracciones(interaccion == interaccionBlanca,
                         interaccion == interaccionVerde,
                         interaccion == interaccionNegra);
    }

    public static void MostrarMensajeResiduo(GameObject item)
    {
        if (PlayerPrefs.GetInt(PREF_TUTORIAL, 0) == 1) return;

        Item itemComp = item.GetComponent<Item>();
        if (itemComp == null)
        {
            DesactivarInteracciones();
            return;
        }

        DatosItem datos = itemComp.ObtenerDatosItem();
        string nombre = item.name.Replace("(Clone)", "").Trim();
        string mensaje;
        switch (datos.tipoReciclaje)
        {
            case ETipoReciclaje.OrganicoAprovechable:
                if (datos.tipoItem == ETipoItem.Bolsa)
                    mensaje = "Esta bolsa verde ya trae residuos orgánicos separados, bótala completa en la caneca verde sin abrirla.";
                else
                    mensaje = nombre + " es un residuo orgánico que se descompone y puede convertirse en abono, por eso debe ir en la caneca verde.";
                ActivarSoloInteraccion(interaccionVerde);
                break;
            case ETipoReciclaje.Aprovechable:
                if (datos.tipoItem == ETipoItem.Bolsa)
                    mensaje = "Esta bolsa blanca contiene materiales reciclables limpios; deposítala entera en la caneca blanca.";
                else
                    mensaje = nombre + " está limpio y en buen estado, puede reciclarse o reutilizarse; deposítalo en la caneca blanca.";
                ActivarSoloInteraccion(interaccionBlanca);
                break;
            case ETipoReciclaje.NoAprovechable:
                if (datos.tipoItem == ETipoItem.Bolsa)
                {
                    mensaje = "Esta bolsa contiene materiales de Todo tipo, no ha sido clasificada, debió abrirse y clasificarse; puede depositarse en cualquier caneca aunque algunos items no pertenezcan a ella.";
                    SetInteracciones(true, true, true);
                }

                else
                {
                    mensaje = nombre + " está sucio, contaminado u oxidado y no se recicla; bótalo en la caneca negra.";
                    ActivarSoloInteraccion(interaccionNegra);
                }
                    
                break;
            default:
                DesactivarInteracciones();
                return;
        }
        GestorCajaTexto.Instancia.MostrarMensaje(mensaje, 5f);
    }

    private void PrepararEscenaPrincipal()
    {
        if (pantallaCarga == null || menuPrincipal == null) return;
        if (!string.IsNullOrEmpty(escenaPrincipal))
        {
            pantallaCarga.SetActive(true);
            menuPrincipal.SetActive(false);
            StartCoroutine(CargarNivel());
        }
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