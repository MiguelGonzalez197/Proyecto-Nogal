using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ModuloCompra : Modulo
{
    // ───────────────────────────────────────
    // 1. REFERENCIAS SERIALIZADAS
    // ───────────────────────────────────────
    [Space]
    [Header("Referencias Modulo Compra")]
    [SerializeField]
    private Inventario gestorInventario;                                                        // Permite obtener y disminuir la cantidad de dinero del jugador
    [SerializeField]
    private AnimacionesRecicladora recicladora;                                                 // Permite decidir que animacion realizar la recicladora
    [SerializeField]
    private List<ObjetoComprable> decoracionesDisponibles = new List<ObjetoComprable>();        // Lista de todos los game objects disponibles como decoraciones
    [SerializeField]
    private List<ObjetoComprable> modulosDisponibles = new List<ObjetoComprable>();             // Lista de modulos que se comprar agregar

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
    public List<ObjetoComprable> ObtenerDecoracionesDisponibles() { return decoracionesDisponibles; }
    public List<ObjetoComprable> ObtenerModulosDisponibles() { return modulosDisponibles; }
    /** </Getters */
    /** <Setters> */
    public void EstablecerDecoracionesDisponibles(List<ObjetoComprable> lista) { decoracionesDisponibles = lista; }
    public void EstablecerModulosDisponibles(List<ObjetoComprable> lista) { modulosDisponibles = lista; }
    /** </Setters> */
    public override void Interactuar()
    {
        base.Interactuar();
        if (recicladora != null)
        {
            recicladora.SaludarModulo();
        }
    }

    public override void SalirModuloCallback()
    {
        base.SalirModuloCallback();
    }

    /// <summary>
    /// Callback Boton
    /// </summary>
    public void ComprarObjeto(Button botonOprimido, List<ObjetoComprable> listaAsociada)
    {
        if (gestorInventario == null) return;

        ObjetoComprable objetoAComprar = ObtenerEstructuraObjeto(botonOprimido, listaAsociada);
        int cantidadDineroActual = gestorInventario.ObtenerDinero();
        ProcesarCompra(objetoAComprar, cantidadDineroActual);
    }

    // ───────────────────────────────────────
    // 5. MÉTODOS PRIVADOS
    // ───────────────────────────────────────
    private void ProcesarCompra(ObjetoComprable objetoAComprar, int cantidadDineroActual)
    {
        if (PuedeComprar(objetoAComprar, cantidadDineroActual))
        {
            Debug.Log("Puedes comprar este objeto");
            recicladora.CompraExitosa();
            gestorInventario.DisminuirDinero(objetoAComprar.precio);
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

