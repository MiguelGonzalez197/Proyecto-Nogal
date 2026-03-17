using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModuloCrafteo : Modulo
{
    [Space]
    [Header("Referencias Modulo Crafteo")]
    [SerializeField]
    private Inventario gestorInventario;
    [SerializeField]
    private GameObject mostradorObjetoCrafteable;
    [SerializeField]
    private TextMeshProUGUI aprovechablesNecesarios;
    [SerializeField]
    private TextMeshProUGUI organicosAprovechablesNecesarios;
    [SerializeField]
    private Image imagenObjeto;
    [SerializeField]
    private Button botonCraftear;

    [Header("Prefabs Modulo Crafteo")]
    [SerializeField]
    private List<ObjetoCrafteable> crafteablesDisponibles = new List<ObjetoCrafteable>();

    private ObjetoCrafteable objetoCrafteable;
    private GameObject instanciaObjetoCrafteable;
    private TextMeshProUGUI tituloObjetoTexto;
    private TextMeshProUGUI detalleEstadoTexto;
    private TextMeshProUGUI resumenMaterialesTexto;
    private TextMeshProUGUI textoBotonCraftear;
    private TextMeshProUGUI inicialesImagenTexto;

    protected override void Start()
    {
        base.Start();
        InicializarMostrador();

        if (gestorInventario != null)
        {
            gestorInventario.EnInventarioActualizado += ActualizarMostradorDesdeInventario;
        }
    }

    private void OnDestroy()
    {
        if (gestorInventario != null)
        {
            gestorInventario.EnInventarioActualizado -= ActualizarMostradorDesdeInventario;
        }
    }

    public List<ObjetoCrafteable> ObtenerCrafteablesDisponibles() { return crafteablesDisponibles; }
    public void EstablecerCrafteablesDisponibles(List<ObjetoCrafteable> lista) { crafteablesDisponibles = lista; }
    public Inventario ObtenerGestorInventario() { return gestorInventario; }

    public bool PuedeCraftear(ObjetoCrafteable objeto)
    {
        return CrafteoVisualHelper.PuedeCraftear(objeto, gestorInventario);
    }

    public override void Interactuar()
    {
        base.Interactuar();

        if (objetoCrafteable != null)
        {
            CambiarUIMostrador();
            ActivarElementosMostrador();
            SpawnearObjeto();
            return;
        }

        MostrarPrimerCrafteableDisponible();
    }

    public override void SalirModuloCallback()
    {
        base.SalirModuloCallback();

        if (mostradorObjetoCrafteable != null)
        {
            mostradorObjetoCrafteable.SetActive(false);
        }

        DestruirObjetoEnEscena();
    }

    public void MostrarCrafteable(Button botonOprimido, List<ObjetoCrafteable> listaAsociada)
    {
        if (aprovechablesNecesarios == null || organicosAprovechablesNecesarios == null)
        {
            return;
        }

        objetoCrafteable = ObtenerEstructuraObjeto(botonOprimido, listaAsociada);
        if (objetoCrafteable == null)
        {
            return;
        }

        CambiarUIMostrador();
        ActivarElementosMostrador();
        SpawnearObjeto();
    }

    public void Craftear()
    {
        if (objetoCrafteable == null || !PuedeComprar())
        {
            ActivarElementosMostrador();
            return;
        }

        gestorInventario.DisminuirAprovechables(objetoCrafteable.aprovechablesNecesarios);
        gestorInventario.DisminuirOrganicosAprovechables(objetoCrafteable.organicosAprovechablesNecesarios);

        if (mostradorObjetoCrafteable != null)
        {
            mostradorObjetoCrafteable.SetActive(false);
        }

        DestruirObjetoEnEscena();
    }

    private void CambiarUIMostrador()
    {
        if (PuedeCambiarElementos())
        {
            return;
        }

        ActualizarContadoresMateriales();
        ActualizarImagenMostrador();
        ActualizarTextosMostrador();
        ConfigurarBotonCraftear();
    }

    private void ActivarElementosMostrador()
    {
        if (mostradorObjetoCrafteable != null && !mostradorObjetoCrafteable.activeInHierarchy)
        {
            mostradorObjetoCrafteable.SetActive(true);
        }

        ConfigurarBotonCraftear();
    }

    private void SpawnearObjeto()
    {
        if (objetoCrafteable == null || objetoCrafteable.objetoACraftear == null || posicionesItems.Count < 1)
        {
            return;
        }

        DestruirObjetoEnEscena();
        instanciaObjetoCrafteable = Instantiate(objetoCrafteable.objetoACraftear, posicionesItems[0].position, Quaternion.identity);
    }

    private void DestruirObjetoEnEscena()
    {
        if (instanciaObjetoCrafteable != null)
        {
            Destroy(instanciaObjetoCrafteable);
            instanciaObjetoCrafteable = null;
        }
    }

    private void ActualizarContadoresMateriales()
    {
        int disponiblesAprovechables = gestorInventario != null ? gestorInventario.ObtenerAprovechables() : 0;
        int disponiblesOrganicos = gestorInventario != null ? gestorInventario.ObtenerOrganicos() : 0;

        aprovechablesNecesarios.text = string.Format("{0}/{1}", Mathf.Min(disponiblesAprovechables, objetoCrafteable.aprovechablesNecesarios), objetoCrafteable.aprovechablesNecesarios);
        organicosAprovechablesNecesarios.text = string.Format("{0}/{1}", Mathf.Min(disponiblesOrganicos, objetoCrafteable.organicosAprovechablesNecesarios), objetoCrafteable.organicosAprovechablesNecesarios);

        aprovechablesNecesarios.color = disponiblesAprovechables >= objetoCrafteable.aprovechablesNecesarios
            ? new Color(0.20f, 0.52f, 0.29f, 1f)
            : new Color(0.73f, 0.45f, 0.12f, 1f);
        organicosAprovechablesNecesarios.color = disponiblesOrganicos >= objetoCrafteable.organicosAprovechablesNecesarios
            ? new Color(0.20f, 0.52f, 0.29f, 1f)
            : new Color(0.73f, 0.45f, 0.12f, 1f);
    }

    private void ActualizarImagenMostrador()
    {
        if (imagenObjeto == null)
        {
            return;
        }

        bool tieneImagenValida = CrafteoVisualHelper.TieneImagenValida(objetoCrafteable.imagenCrafteable);
        imagenObjeto.sprite = tieneImagenValida
            ? objetoCrafteable.imagenCrafteable
            : CrafteoVisualHelper.ObtenerSpritePlano();
        imagenObjeto.color = tieneImagenValida
            ? Color.white
            : CrafteoVisualHelper.ObtenerColorBase(objetoCrafteable);
        imagenObjeto.preserveAspect = tieneImagenValida;

        if (inicialesImagenTexto != null)
        {
            inicialesImagenTexto.gameObject.SetActive(!tieneImagenValida);
            inicialesImagenTexto.text = CrafteoVisualHelper.ObtenerIniciales(objetoCrafteable);
        }
    }

    private void ActualizarTextosMostrador()
    {
        AsegurarElementosMostrador();

        if (tituloObjetoTexto != null)
        {
            tituloObjetoTexto.text = CrafteoVisualHelper.ObtenerNombre(objetoCrafteable);
        }

        if (resumenMaterialesTexto != null)
        {
            resumenMaterialesTexto.text = CrafteoVisualHelper.ObtenerResumenMateriales(objetoCrafteable);
        }

        if (detalleEstadoTexto != null)
        {
            string mensajeEstado = CrafteoVisualHelper.ObtenerEstadoMateriales(objetoCrafteable, gestorInventario);

            if (!CrafteoVisualHelper.TieneImagenValida(objetoCrafteable.imagenCrafteable))
            {
                mensajeEstado += "\nVista conceptual del resultado.";
            }

            if (objetoCrafteable.objetoACraftear == null)
            {
                mensajeEstado += "\nSin modelo 3D disponible.";
            }

            detalleEstadoTexto.text = mensajeEstado;
            detalleEstadoTexto.color = CrafteoVisualHelper.ObtenerColorEstado(PuedeComprar());
        }
    }

    private void ConfigurarBotonCraftear()
    {
        if (botonCraftear == null)
        {
            return;
        }

        bool puedeCraftear = objetoCrafteable != null && PuedeComprar();
        botonCraftear.interactable = puedeCraftear;

        if (textoBotonCraftear != null)
        {
            textoBotonCraftear.text = puedeCraftear ? "Craftear ahora" : "Reune materiales";
        }
    }

    private void MostrarPrimerCrafteableDisponible()
    {
        if (crafteablesDisponibles == null || crafteablesDisponibles.Count == 0)
        {
            return;
        }

        for (int i = 0; i < crafteablesDisponibles.Count; i++)
        {
            ObjetoCrafteable crafteable = crafteablesDisponibles[i];
            if (crafteable != null && crafteable.botonObjeto != null)
            {
                MostrarCrafteable(crafteable.botonObjeto, crafteablesDisponibles);
                return;
            }
        }
    }

    private void InicializarMostrador()
    {
        AsegurarElementosMostrador();

        if (mostradorObjetoCrafteable != null)
        {
            mostradorObjetoCrafteable.SetActive(false);
        }

        if (imagenObjeto != null)
        {
            imagenObjeto.sprite = CrafteoVisualHelper.ObtenerSpritePlano();
            imagenObjeto.color = new Color(0.88f, 0.88f, 0.88f, 1f);
            imagenObjeto.preserveAspect = false;
        }

        ConfigurarBotonCraftear();
    }

    private void AsegurarElementosMostrador()
    {
        if (mostradorObjetoCrafteable == null)
        {
            return;
        }

        RectTransform rectTransformMostrador = mostradorObjetoCrafteable.GetComponent<RectTransform>();
        if (rectTransformMostrador == null)
        {
            return;
        }

        tituloObjetoTexto = ObtenerOCrearTexto(
            rectTransformMostrador,
            "TituloObjeto",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, 247f),
            new Vector2(330f, 42f),
            new Vector2(0.5f, 0.5f),
            24,
            FontStyles.Bold,
            new Color(0.12f, 0.14f, 0.12f, 1f),
            TextAlignmentOptions.Center);
        resumenMaterialesTexto = ObtenerOCrearTexto(
            rectTransformMostrador,
            "ResumenMateriales",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, 92f),
            new Vector2(250f, 24f),
            new Vector2(0.5f, 0.5f),
            14,
            FontStyles.Bold,
            new Color(0.21f, 0.24f, 0.21f, 1f),
            TextAlignmentOptions.Center);
        detalleEstadoTexto = ObtenerOCrearTexto(
            rectTransformMostrador,
            "DetalleEstado",
            new Vector2(0.5f, 0.5f),
            new Vector2(0.5f, 0.5f),
            new Vector2(0f, 54f),
            new Vector2(330f, 44f),
            new Vector2(0.5f, 0.5f),
            13,
            FontStyles.Normal,
            new Color(0.20f, 0.20f, 0.20f, 1f),
            TextAlignmentOptions.Center);

        if (imagenObjeto != null)
        {
            inicialesImagenTexto = ObtenerOCrearTexto(
                imagenObjeto.rectTransform,
                "InicialesImagen",
                new Vector2(0f, 0f),
                new Vector2(1f, 1f),
                Vector2.zero,
                Vector2.zero,
                Vector2.zero,
                32,
                FontStyles.Bold,
                Color.white,
                TextAlignmentOptions.Center);

            Outline contornoImagen = imagenObjeto.GetComponent<Outline>();
            if (contornoImagen == null)
            {
                contornoImagen = imagenObjeto.gameObject.AddComponent<Outline>();
            }

            contornoImagen.effectColor = new Color(0.08f, 0.08f, 0.08f, 0.25f);
            contornoImagen.effectDistance = new Vector2(1f, -1f);
            contornoImagen.useGraphicAlpha = true;
        }

        if (botonCraftear != null)
        {
            textoBotonCraftear = botonCraftear.GetComponentInChildren<TextMeshProUGUI>(true);
        }
    }

    private void ActualizarMostradorDesdeInventario()
    {
        if (objetoCrafteable == null)
        {
            return;
        }

        CambiarUIMostrador();
    }

    private static TextMeshProUGUI ObtenerOCrearTexto(
        RectTransform padre,
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
        texto.enableWordWrapping = true;
        texto.raycastTarget = false;
        texto.text = string.Empty;

        return texto;
    }

    private ObjetoCrafteable ObtenerEstructuraObjeto(Button botonOprimido, List<ObjetoCrafteable> lista)
    {
        int index = HallarBotonEnLista(botonOprimido, lista);
        if (index < 0 || index >= lista.Count)
        {
            return null;
        }

        return lista[index];
    }

    private int HallarBotonEnLista(Button botonOprimido, List<ObjetoCrafteable> lista)
    {
        return lista.FindIndex(boton => boton.botonObjeto == botonOprimido);
    }

    private bool PuedeCambiarElementos()
    {
        return objetoCrafteable == null
            || aprovechablesNecesarios == null
            || organicosAprovechablesNecesarios == null
            || imagenObjeto == null;
    }

    private bool PuedeComprar()
    {
        return PuedeCraftear(objetoCrafteable);
    }
}
