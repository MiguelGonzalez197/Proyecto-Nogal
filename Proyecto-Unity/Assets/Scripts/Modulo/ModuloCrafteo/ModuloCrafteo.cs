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


    ObjetoCrafteable objetoCrafteable;
    GameObject instanciaObjetoCrafteable;

    protected override void Start()
    {
        base.Start();

    }

    public override void Interactuar()
    {
        base.Interactuar();

    }

    public List<ObjetoCrafteable> ObtenerCrafteablesDisponibles() { return crafteablesDisponibles; }
    public void EstablecerCrafteablesDisponibles(List<ObjetoCrafteable> lista) { crafteablesDisponibles = lista; }

    public override void SalirModuloCallback()
    {
        base.SalirModuloCallback();

        mostradorObjetoCrafteable.SetActive(false);
        DestruirObjetoAnterior();
    }

    /* Callback boton */
    public void MostrarCrafteable(Button botonOprimido, List<ObjetoCrafteable> listaAsociada)
    {
        if (gestorInventario == null || aprovechablesNecesarios == null || organicosAprovechablesNecesarios == null) return;

        objetoCrafteable = ObtenerEstructuraObjeto(botonOprimido, listaAsociada);
        int cantidadDineroActual = gestorInventario.ObtenerDinero();

        CambiarUIMostrador();
        ActivarMostrador();
        SpawnearObjeto();
    }

    /* Callback boton */
    public void Craftear()
    {
        Debug.Log("Objeto Crafteable");
        gestorInventario.DisminuirAprovechables(objetoCrafteable.aprovechablesNecesarios);
        gestorInventario.DisminuirOrganicosAprovechables(objetoCrafteable.organicosAprovechablesNecesarios);
        mostradorObjetoCrafteable.SetActive(false);
        DestruirObjetoAnterior();
    }

    private void CambiarUIMostrador()
    {
        if (PuedeCambiarElementos()) return;
        aprovechablesNecesarios.text = (objetoCrafteable.aprovechablesNecesarios).ToString();
        organicosAprovechablesNecesarios.text = (objetoCrafteable.organicosAprovechablesNecesarios).ToString();
        imagenObjeto.sprite = objetoCrafteable.imagenCrafteable;

    }

    private void ActivarMostrador()
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
        DestruirObjetoAnterior();

        instanciaObjetoCrafteable = Instantiate(objetoCrafteable.objetoACraftear, posicionesItems[0].position, Quaternion.identity);

    }

    private void DestruirObjetoAnterior()
    {
        if (instanciaObjetoCrafteable != null)
        {
            Destroy(instanciaObjetoCrafteable.gameObject);
        }
    }

    private bool PuedeCambiarElementos()
    {
        return objetoCrafteable == null || aprovechablesNecesarios == null || organicosAprovechablesNecesarios == null || imagenObjeto == null;
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

    private bool PuedeComprar()
    {
        return gestorInventario.ObtenerAprovechables() >= objetoCrafteable.aprovechablesNecesarios && gestorInventario.ObtenerOrganicos() >= objetoCrafteable.organicosAprovechablesNecesarios;
    }
}
