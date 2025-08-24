using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModuloCrafteo : Modulo
{
    // ───────────────────────────────────────
    // 1. REFERENCIAS SERIALIZADAS
    // ───────────────────────────────────────
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
    private Image imagenObjeto;                                                                 // Imagen que representa al objeto en el modulo
    [SerializeField]
    private Button botonCraftear;

    [Header("Prefabs Modulo Crafteo")]
    [SerializeField]
    private List<ObjetoCrafteable> crafteablesDisponibles = new List<ObjetoCrafteable>();


    // ───────────────────────────────────────
    // 2. CAMPOS PRIVADOS INTERNOS
    // ───────────────────────────────────────
    private ObjetoCrafteable objetoCrafteable;
    private GameObject instanciaObjetoCrafteable;

    // ───────────────────────────────────────
    // 3. MÉTODOS UNITY
    // ───────────────────────────────────────
    protected override void Start()
    {
        base.Start();

    }

    // ───────────────────────────────────────
    // 4. MÉTODOS PÚBLICOS
    // ───────────────────────────────────────

    /** <Getters> */ 
    public List<ObjetoCrafteable> ObtenerCrafteablesDisponibles() { return crafteablesDisponibles; }
    /** </Getters> */
    /** <setters> */
    public void EstablecerCrafteablesDisponibles(List<ObjetoCrafteable> lista) { crafteablesDisponibles = lista; }
    /** </Setters> */

    public override void Interactuar()
    {
        
        base.Interactuar();

    }

    public override void SalirModuloCallback()
    {
        base.SalirModuloCallback();

        mostradorObjetoCrafteable.SetActive(false);
        DestruirObjetoEnEscena();
    }

    /* Callback boton */
    public void MostrarCrafteable(Button botonOprimido, List<ObjetoCrafteable> listaAsociada)
    {
        if (gestorInventario == null || aprovechablesNecesarios == null || organicosAprovechablesNecesarios == null) return;

        objetoCrafteable = ObtenerEstructuraObjeto(botonOprimido, listaAsociada);
        int cantidadDineroActual = gestorInventario.ObtenerDinero();

        CambiarUIMostrador();
        ActivarElementosMostrador();
        SpawnearObjeto();
    }

    /// <summary>
    /// Callback boton
    /// Gestiona los elementos necesarios para construir el objeto seleccionado
    /// </summary>
    public void Craftear()
    {
        Debug.Log("Objeto Crafteable");
        gestorInventario.DisminuirAprovechables(objetoCrafteable.aprovechablesNecesarios);
        gestorInventario.DisminuirOrganicosAprovechables(objetoCrafteable.organicosAprovechablesNecesarios);
        mostradorObjetoCrafteable.SetActive(false);
        DestruirObjetoEnEscena();
    }

    // ───────────────────────────────────────
    // 5. MÉTODOS PRIVADOS
    // ───────────────────────────────────────
    private void CambiarUIMostrador()
    {
        if (PuedeCambiarElementos()) return;
        aprovechablesNecesarios.text = (objetoCrafteable.aprovechablesNecesarios).ToString();
        organicosAprovechablesNecesarios.text = (objetoCrafteable.organicosAprovechablesNecesarios).ToString();
        imagenObjeto.sprite = objetoCrafteable.imagenCrafteable;

    }

    private void ActivarElementosMostrador()
    {
        if (mostradorObjetoCrafteable == null || botonCraftear == null) return;

        if (PuedeComprar())
        {
            botonCraftear.interactable = true;
        }
        else
        {
            botonCraftear.interactable = false;
        }

        if (mostradorObjetoCrafteable.activeInHierarchy == false)
        {
            mostradorObjetoCrafteable.SetActive(true);
        }


    }

    private void SpawnearObjeto()
    {
        if (posicionesItems.Count < 1) return;
        DestruirObjetoEnEscena();

        instanciaObjetoCrafteable = Instantiate(objetoCrafteable.objetoACraftear, posicionesItems[0].position, Quaternion.identity);

    }

    private void DestruirObjetoEnEscena()
    {
        if (instanciaObjetoCrafteable != null)
        {
            Destroy(instanciaObjetoCrafteable.gameObject);
        }
    }

    private ObjetoCrafteable ObtenerEstructuraObjeto(Button botonOprimido, List<ObjetoCrafteable> lista)
    {
        int index = HallarBotonEnLista(botonOprimido, lista);
        return lista[index];
    }

    private int HallarBotonEnLista(Button botonOprimido, List<ObjetoCrafteable> lista)
    {
        return lista.FindIndex(boton => boton.botonObjeto == botonOprimido);
    }

    /** Metodos Booleanos */
    private bool PuedeCambiarElementos()
    {
        return objetoCrafteable == null || aprovechablesNecesarios == null || organicosAprovechablesNecesarios == null || imagenObjeto == null;
    }

    private bool PuedeComprar()
    {
        return gestorInventario.ObtenerAprovechables() >= objetoCrafteable.aprovechablesNecesarios && gestorInventario.ObtenerOrganicos() >= objetoCrafteable.organicosAprovechablesNecesarios;
    }
}
