using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIModuloCompra : MonoBehaviour
{
    private ModuloCompra moduloCompra;

    [Header("Referencias UI Modulo Compra")]
    [SerializeField]
    private GameObject ContenedorDecoraciones;
    [SerializeField]
    private GameObject ContenedorModulos;


    [Header("Prefabs UI Modulo Compra")]
    [SerializeField]
    private GameObject prefabBoton;

    private List<DatosCompra> decoracionesDisponibles = new List<DatosCompra>();
    private List<DatosCompra> modulosDisponibles = new List<DatosCompra>();

    void Start()
    {
        moduloCompra = GetComponent<ModuloCompra>();
        GenerarUIDecoraciones();
        GenerarUIModulos();
    }

    public void CambiarAContenidoDecoraciones()
    {
        ContenedorDecoraciones.SetActive(true);
        ContenedorModulos.SetActive(false);
    }

    public void CambiarAContenidoModulos()
    {
        ContenedorModulos.SetActive(true);
        ContenedorDecoraciones.SetActive(false);
    }

    private void GenerarUIDecoraciones()
    {
        decoracionesDisponibles = moduloCompra.ObtenerDecoracionesDisponibles();
        GenerarBotones(ref decoracionesDisponibles, ContenedorDecoraciones);
        InicializarBotones(ref decoracionesDisponibles);
        moduloCompra.EstablecerDecoracionesDisponibles(decoracionesDisponibles);
    }
    private void GenerarUIModulos()
    {
        modulosDisponibles = moduloCompra.ObtenerModulosDisponibles();
        GenerarBotones(ref modulosDisponibles, ContenedorModulos);
        InicializarBotones(ref modulosDisponibles);
        moduloCompra.EstablecerModulosDisponibles(modulosDisponibles);
    }


    private void GenerarBotones(ref List<DatosCompra> lista, GameObject contenedor)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            GameObject instanciaBoton = Instantiate(prefabBoton, contenedor.transform.position, Quaternion.identity);
            instanciaBoton.transform.SetParent(contenedor.transform, false);

            DatosCompra copiaDatos = lista[i];
            copiaDatos.botonObjeto = instanciaBoton.GetComponent<Button>();

            lista[i] = copiaDatos;

            EstablecerTextoPrecioBotones(copiaDatos);

        }
    }

    private static void EstablecerTextoPrecioBotones(DatosCompra copiaDatosInstancia)
    {
        TextMeshProUGUI precio = copiaDatosInstancia.botonObjeto.GetComponentInChildren<TextMeshProUGUI>();
        precio.text = (copiaDatosInstancia.precio).ToString() + (" $");
    }

    private void InicializarBotones(ref List<DatosCompra> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            int indexCapturado = i; // Captura el índice en una variable local (clave para evitar bugs de cierre)
            AsignarCallbackCompra(lista, i, indexCapturado);
        }

        
    }

    private void AsignarCallbackCompra(List<DatosCompra> lista, int i, int indexCapturado)
    {
        lista[i].botonObjeto.onClick.AddListener(() =>
        {
            moduloCompra.ComprarObjeto(lista[indexCapturado].botonObjeto, lista);
        });
    }
}
