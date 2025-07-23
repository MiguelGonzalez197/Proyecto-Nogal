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
    private List<DatosCompra> decoracionesDisponibles = new List<DatosCompra>();
    [SerializeField]
    private List<DatosCompra> modulosDisponibles = new List<DatosCompra>();


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

    public List<DatosCompra> ObtenerDecoracionesDisponibles() { return decoracionesDisponibles; }
    public List<DatosCompra> ObtenerModulosDisponibles() { return modulosDisponibles; }
    public void EstablecerDecoracionesDisponibles(List<DatosCompra> lista) { decoracionesDisponibles = lista; }
    public void EstablecerModulosDisponibles(List<DatosCompra> lista) { decoracionesDisponibles = lista; }

    /* Callback boton */
    public void ComprarObjeto(Button botonOprimido, List<DatosCompra> listaAsociada)
    {
        if (gestorInventario == null) return;

        DatosCompra objetoAComprar = ObtenerEstructuraObjeto(botonOprimido, listaAsociada);
        int cantidadDineroActual = gestorInventario.ObtenerDinero();

        if (cantidadDineroActual >= objetoAComprar.precio)
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


    private DatosCompra ObtenerEstructuraObjeto(Button botonOprimido, List<DatosCompra> lista)
    {
        int index = HallarBotonEnLista(botonOprimido, lista);
        return lista[index];
    }


    private static void ActivarObjeto(DatosCompra objetosAComprar)
    {
        if (objetosAComprar.objetoAVenta != null)
        {
            objetosAComprar.objetoAVenta.SetActive(true);
        }
        Destroy(objetosAComprar.botonObjeto.gameObject);
    }



    private int HallarBotonEnLista(Button botonOprimido, List<DatosCompra> lista)
    {
        return lista.FindIndex(boton => boton.botonObjeto == botonOprimido);
    }


}

