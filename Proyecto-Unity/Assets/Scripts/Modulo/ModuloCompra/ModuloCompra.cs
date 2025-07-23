using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ModuloCompra : Modulo
{
    [Space]
    [Header("Referencias Modulo Compra")]
    [SerializeField]
    private Inventario gestorInventario;
    [SerializeField]
    private AnimacionesRecicladora recicladora;
    [SerializeField]
    private List<ObjetoComprable> decoracionesDisponibles = new List<ObjetoComprable>();
    [SerializeField]
    private List<ObjetoComprable> modulosDisponibles = new List<ObjetoComprable>();


    protected override void Start()
    {
        base.Start();
        
    }

    public override void Interactuar()
    {
        base.Interactuar();
        if(recicladora != null)
        {
            recicladora.SaludarModulo();
        }
    }

    public List<ObjetoComprable> ObtenerDecoracionesDisponibles() { return decoracionesDisponibles; }
    public List<ObjetoComprable> ObtenerModulosDisponibles() { return modulosDisponibles; }
    public void EstablecerDecoracionesDisponibles(List<ObjetoComprable> lista) { decoracionesDisponibles = lista; }
    public void EstablecerModulosDisponibles(List<ObjetoComprable> lista) { modulosDisponibles = lista; }

    /* Callback boton */
    public void ComprarObjeto(Button botonOprimido, List<ObjetoComprable> listaAsociada)
    {
        if (gestorInventario == null) return;

        ObjetoComprable objetoAComprar = ObtenerEstructuraObjeto(botonOprimido, listaAsociada);
        int cantidadDineroActual = gestorInventario.ObtenerDinero();
        ProcesarCompra(objetoAComprar, cantidadDineroActual);
    }

    private void ProcesarCompra(ObjetoComprable objetoAComprar, int cantidadDineroActual)
    {
        if (PuedeComprar(objetoAComprar, cantidadDineroActual))
        {
            Debug.Log("Puedes comprar este objeto");
            recicladora.CompraExitosa();
            gestorInventario.DisminuirDineroCompra(objetoAComprar.precio);
            ActivarObjeto(objetoAComprar);
        }
        else
        {
            recicladora.CompraFallida();
            Debug.Log("No puedes comprar este objeto");
        }
    }

    private ObjetoComprable ObtenerEstructuraObjeto(Button botonOprimido, List<ObjetoComprable> lista)
    {
        int index = HallarBotonEnLista(botonOprimido, lista);
        return lista[index];
    }


    private static void ActivarObjeto(ObjetoComprable objetosAComprar)
    {
        if (objetosAComprar.objetoAVenta != null)
        {
            objetosAComprar.objetoAVenta.SetActive(true);
        }
        Destroy(objetosAComprar.botonObjeto.gameObject);
    }



    private int HallarBotonEnLista(Button botonOprimido, List<ObjetoComprable> lista)
    {
        return lista.FindIndex(boton => boton.botonObjeto == botonOprimido);
    }

    private bool PuedeComprar(ObjetoComprable objetoAComprar, int cantidadDineroActual)
    {
        return cantidadDineroActual >= objetoAComprar.precio;
    }

}

