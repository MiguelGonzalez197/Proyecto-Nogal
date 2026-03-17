using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIModuloCrafteo : MonoBehaviour
{
    [Header("Referencias UI Modulo Compra")]
    [SerializeField]
    private GameObject ContenedorCrafteables;

    [Header("Prefabs UI Modulo Compra")]
    [SerializeField]
    private GameObject prefabBoton;

    private readonly Dictionary<Button, ReferenciasTarjetaCrafteo> referenciasTarjetas = new Dictionary<Button, ReferenciasTarjetaCrafteo>();

    private List<ObjetoCrafteable> crafteablesDisponibles = new List<ObjetoCrafteable>();
    private ModuloCrafteo moduloCrafteo;
    private Inventario gestorInventario;
    private Button botonSeleccionado;
    private TextMeshProUGUI textoEstadoVacio;

    private sealed class ReferenciasTarjetaCrafteo
    {
        public Image fondo;
        public Outline contorno;
        public Image portada;
        public TextMeshProUGUI iniciales;
        public TextMeshProUGUI titulo;
        public TextMeshProUGUI materiales;
        public Image badgeEstado;
        public TextMeshProUGUI textoEstado;
    }

    private void Start()
    {
        moduloCrafteo = GetComponent<ModuloCrafteo>();
        gestorInventario = moduloCrafteo != null ? moduloCrafteo.ObtenerGestorInventario() : null;

        if (gestorInventario != null)
        {
            gestorInventario.EnInventarioActualizado += ActualizarEstadosVisuales;
        }

        ObtenerListas();
        GenerarUI(ref crafteablesDisponibles, ContenedorCrafteables);
        EstablecerListas();

        if (crafteablesDisponibles.Count > 0 && crafteablesDisponibles[0].botonObjeto != null)
        {
            botonSeleccionado = crafteablesDisponibles[0].botonObjeto;
        }

        ActualizarEstadosVisuales();
    }

    private void OnDestroy()
    {
        if (gestorInventario != null)
        {
            gestorInventario.EnInventarioActualizado -= ActualizarEstadosVisuales;
        }
    }

    private void ObtenerListas()
    {
        if (moduloCrafteo == null)
        {
            return;
        }

        crafteablesDisponibles = moduloCrafteo.ObtenerCrafteablesDisponibles();
    }

    private void GenerarUI(ref List<ObjetoCrafteable> lista, GameObject contenedor)
    {
        if (contenedor == null)
        {
            return;
        }

        if (lista == null || lista.Count == 0)
        {
            MostrarEstadoVacio(contenedor);
            return;
        }

        OcultarEstadoVacio();
        GenerarBotones(ref lista, contenedor);
        InicializarBotones(ref lista);
    }

    private void EstablecerListas()
    {
        if (moduloCrafteo == null)
        {
            return;
        }

        moduloCrafteo.EstablecerCrafteablesDisponibles(crafteablesDisponibles);
    }

    private void GenerarBotones(ref List<ObjetoCrafteable> lista, GameObject contenedor)
    {
        if (prefabBoton == null)
        {
            return;
        }

        for (int i = 0; i < lista.Count; i++)
        {
            GameObject instanciaBoton = Instantiate(prefabBoton, contenedor.transform.position, Quaternion.identity);
            instanciaBoton.transform.SetParent(contenedor.transform, false);

            Button boton = instanciaBoton.GetComponent<Button>();
            lista[i].botonObjeto = boton;

            if (boton == null)
            {
                continue;
            }

            referenciasTarjetas[boton] = ConstruirTarjetaVisual(instanciaBoton, lista[i]);
        }
    }

    private void InicializarBotones(ref List<ObjetoCrafteable> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            int indexCapturado = i;
            AsignarCallbackCompra(lista, i, indexCapturado);
        }
    }

    private void AsignarCallbackCompra(List<ObjetoCrafteable> lista, int i, int indexCapturado)
    {
        if (lista[i].botonObjeto == null)
        {
            return;
        }

        lista[i].botonObjeto.onClick.AddListener(() =>
        {
            botonSeleccionado = lista[indexCapturado].botonObjeto;
            moduloCrafteo.MostrarCrafteable(lista[indexCapturado].botonObjeto, lista);
            ActualizarEstadosVisuales();
        });
    }

    private ReferenciasTarjetaCrafteo ConstruirTarjetaVisual(GameObject botonInstanciado, ObjetoCrafteable objeto)
    {
        ReferenciasTarjetaCrafteo referencias = new ReferenciasTarjetaCrafteo();

        referencias.fondo = botonInstanciado.GetComponent<Image>();
        referencias.contorno = AsegurarContorno(botonInstanciado);
        referencias.portada = ObtenerOCrearPortada(botonInstanciado.transform);
        referencias.iniciales = ObtenerOCrearTexto(
            referencias.portada.transform,
            "Iniciales",
            new Vector2(0f, 0f),
            new Vector2(1f, 1f),
            Vector2.zero,
            Vector2.zero,
            Vector2.zero,
            26,
            FontStyles.Bold,
            Color.white,
            TextAlignmentOptions.Center);
        referencias.titulo = ObtenerOCrearTexto(
            botonInstanciado.transform,
            "Titulo",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, -47f),
            new Vector2(150f, 40f),
            new Vector2(0.5f, 0.5f),
            13,
            FontStyles.Bold,
            new Color(0.14f, 0.18f, 0.14f, 1f),
            TextAlignmentOptions.Center);
        referencias.materiales = ObtenerOCrearTexto(
            botonInstanciado.transform,
            "Materiales",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, -71f),
            new Vector2(150f, 24f),
            new Vector2(0.5f, 0.5f),
            11,
            FontStyles.Normal,
            new Color(0.29f, 0.29f, 0.29f, 1f),
            TextAlignmentOptions.Center);

        referencias.badgeEstado = ObtenerOCrearImagen(
            botonInstanciado.transform,
            "BadgeEstado",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, 64f),
            new Vector2(116f, 22f),
            new Vector2(0.5f, 0.5f));
        referencias.textoEstado = ObtenerOCrearTexto(
            referencias.badgeEstado.transform,
            "TextoEstado",
            new Vector2(0f, 0f),
            new Vector2(1f, 1f),
            Vector2.zero,
            Vector2.zero,
            Vector2.zero,
            10,
            FontStyles.Bold,
            Color.white,
            TextAlignmentOptions.Center);

        referencias.titulo.enableWordWrapping = true;
        referencias.textoEstado.enableWordWrapping = false;
        referencias.materiales.enableWordWrapping = false;

        ActualizarVisualTarjeta(objeto);
        return referencias;
    }

    private void ActualizarEstadosVisuales()
    {
        for (int i = 0; i < crafteablesDisponibles.Count; i++)
        {
            ActualizarVisualTarjeta(crafteablesDisponibles[i]);
        }
    }

    private void ActualizarVisualTarjeta(ObjetoCrafteable objeto)
    {
        if (objeto == null || objeto.botonObjeto == null || !referenciasTarjetas.ContainsKey(objeto.botonObjeto))
        {
            return;
        }

        ReferenciasTarjetaCrafteo referencias = referenciasTarjetas[objeto.botonObjeto];
        bool puedeCraftear = moduloCrafteo != null && moduloCrafteo.PuedeCraftear(objeto);
        bool estaSeleccionado = botonSeleccionado == objeto.botonObjeto;

        referencias.fondo.color = estaSeleccionado
            ? new Color(0.99f, 0.96f, 0.88f, 1f)
            : new Color(0.96f, 0.96f, 0.96f, 1f);

        if (referencias.contorno != null)
        {
            referencias.contorno.effectColor = estaSeleccionado
                ? new Color(0.16f, 0.39f, 0.28f, 0.75f)
                : new Color(0.12f, 0.12f, 0.12f, 0.22f);
        }

        referencias.titulo.text = CrafteoVisualHelper.ObtenerNombre(objeto);
        referencias.materiales.text = CrafteoVisualHelper.ObtenerResumenMateriales(objeto);
        referencias.materiales.color = puedeCraftear
            ? new Color(0.21f, 0.42f, 0.24f, 1f)
            : new Color(0.56f, 0.35f, 0.14f, 1f);

        referencias.badgeEstado.sprite = CrafteoVisualHelper.ObtenerSpritePlano();
        referencias.badgeEstado.color = CrafteoVisualHelper.ObtenerColorEstado(puedeCraftear);
        referencias.textoEstado.text = puedeCraftear ? "Listo" : "Recolecta mas";

        ConfigurarPortada(objeto, referencias.portada, referencias.iniciales);
    }

    private static void ConfigurarPortada(ObjetoCrafteable objeto, Image portada, TextMeshProUGUI iniciales)
    {
        if (portada == null)
        {
            return;
        }

        bool tieneImagenValida = CrafteoVisualHelper.TieneImagenValida(objeto != null ? objeto.imagenCrafteable : null);
        portada.sprite = tieneImagenValida
            ? objeto.imagenCrafteable
            : CrafteoVisualHelper.ObtenerSpritePlano();
        portada.color = tieneImagenValida
            ? Color.white
            : CrafteoVisualHelper.ObtenerColorBase(objeto);
        portada.preserveAspect = tieneImagenValida;

        if (iniciales != null)
        {
            iniciales.gameObject.SetActive(!tieneImagenValida);
            iniciales.text = CrafteoVisualHelper.ObtenerIniciales(objeto);
        }
    }

    private void MostrarEstadoVacio(GameObject contenedor)
    {
        textoEstadoVacio = ObtenerOCrearTexto(
            contenedor.transform,
            "TextoEstadoVacio",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, -20f),
            new Vector2(260f, 60f),
            new Vector2(0.5f, 0.5f),
            18,
            FontStyles.Bold,
            new Color(0.2f, 0.2f, 0.2f, 1f),
            TextAlignmentOptions.Center);
        textoEstadoVacio.text = "No hay recetas disponibles por ahora.";
        textoEstadoVacio.gameObject.SetActive(true);
    }

    private void OcultarEstadoVacio()
    {
        if (textoEstadoVacio != null)
        {
            textoEstadoVacio.gameObject.SetActive(false);
        }
    }

    private static Image ObtenerOCrearPortada(Transform padre)
    {
        Transform transformPortada = padre.Find("Image");
        Image portada = transformPortada != null ? transformPortada.GetComponent<Image>() : null;

        if (portada == null)
        {
            portada = ObtenerOCrearImagen(
                padre,
                "Image",
                new Vector2(0.5f, 0.5f),
                new Vector2(0.5f, 0.5f),
                new Vector2(0f, 8f),
                new Vector2(92f, 92f),
                new Vector2(0.5f, 0.5f));
        }

        RectTransform rectTransform = portada.rectTransform;
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.anchoredPosition = new Vector2(0f, 8f);
        rectTransform.sizeDelta = new Vector2(92f, 92f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);

        portada.sprite = CrafteoVisualHelper.ObtenerSpritePlano();
        portada.type = Image.Type.Simple;
        portada.raycastTarget = false;

        Outline contornoPortada = portada.GetComponent<Outline>();
        if (contornoPortada == null)
        {
            contornoPortada = portada.gameObject.AddComponent<Outline>();
        }

        contornoPortada.effectColor = new Color(0.08f, 0.08f, 0.08f, 0.25f);
        contornoPortada.effectDistance = new Vector2(1f, -1f);
        contornoPortada.useGraphicAlpha = true;

        return portada;
    }

    private static Outline AsegurarContorno(GameObject objeto)
    {
        Outline contorno = objeto.GetComponent<Outline>();
        if (contorno == null)
        {
            contorno = objeto.AddComponent<Outline>();
        }

        contorno.effectDistance = new Vector2(2f, -2f);
        contorno.useGraphicAlpha = true;

        return contorno;
    }

    private static Image ObtenerOCrearImagen(
        Transform padre,
        string nombre,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 anchoredPosition,
        Vector2 sizeDelta,
        Vector2 pivot)
    {
        Transform transformExistente = padre.Find(nombre);
        Image imagen = transformExistente != null ? transformExistente.GetComponent<Image>() : null;

        if (imagen == null)
        {
            GameObject objetoImagen = new GameObject(nombre, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
            objetoImagen.layer = padre.gameObject.layer;
            objetoImagen.transform.SetParent(padre, false);
            imagen = objetoImagen.GetComponent<Image>();
        }

        RectTransform rectTransform = imagen.rectTransform;
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = sizeDelta;
        rectTransform.pivot = pivot;

        imagen.sprite = CrafteoVisualHelper.ObtenerSpritePlano();
        imagen.type = Image.Type.Simple;
        imagen.raycastTarget = false;

        return imagen;
    }

    private static TextMeshProUGUI ObtenerOCrearTexto(
        Transform padre,
        string nombre,
        Vector2 anchorMin,
        Vector2 anchorMax,
        Vector2 anchoredPosition,
        Vector2 sizeDelta,
        Vector2 pivot,
        float fontSize,
        FontStyles estilo,
        Color color,
        TextAlignmentOptions alineacion)
    {
        Transform transformExistente = padre.Find(nombre);
        TextMeshProUGUI texto = transformExistente != null ? transformExistente.GetComponent<TextMeshProUGUI>() : null;

        if (texto == null)
        {
            GameObject objetoTexto = new GameObject(nombre, typeof(RectTransform), typeof(CanvasRenderer), typeof(TextMeshProUGUI));
            objetoTexto.layer = padre.gameObject.layer;
            objetoTexto.transform.SetParent(padre, false);
            texto = objetoTexto.GetComponent<TextMeshProUGUI>();
        }

        RectTransform rectTransform = texto.rectTransform;
        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
        rectTransform.anchoredPosition = anchoredPosition;
        rectTransform.sizeDelta = sizeDelta;
        rectTransform.pivot = pivot;

        texto.fontSize = fontSize;
        texto.fontStyle = estilo;
        texto.color = color;
        texto.alignment = alineacion;
        texto.enableWordWrapping = false;
        texto.raycastTarget = false;
        texto.text = string.Empty;

        return texto;
    }
}
