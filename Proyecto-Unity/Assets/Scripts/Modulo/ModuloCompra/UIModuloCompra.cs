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

    private List<ObjetoComprable> decoracionesDisponibles = new List<ObjetoComprable>();
    private List<ObjetoComprable> modulosDisponibles = new List<ObjetoComprable>();

    void Start()
    {
        moduloCompra = GetComponent<ModuloCompra>();
        ObtenerListas();
        GenerarUI(ref decoracionesDisponibles, ContenedorDecoraciones);
        GenerarUI(ref modulosDisponibles, ContenedorModulos);
        EstablecerListas();
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


    private void ObtenerListas()
    {
        if (moduloCompra == null) return;
        decoracionesDisponibles = moduloCompra.ObtenerDecoracionesDisponibles();
        modulosDisponibles = moduloCompra.ObtenerModulosDisponibles();
    }

    private void GenerarUI(ref List<ObjetoComprable> lista, GameObject contenedor)
    {
        GenerarBotones(ref lista, contenedor);
        InicializarBotones(ref lista);
    }

    private void EstablecerListas()
    {
        moduloCompra.EstablecerDecoracionesDisponibles(decoracionesDisponibles);
        moduloCompra.EstablecerModulosDisponibles(modulosDisponibles);
    }

    private void GenerarBotones(ref List<ObjetoComprable> lista, GameObject contenedor)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            GameObject instanciaBoton = Instantiate(prefabBoton, contenedor.transform.position, Quaternion.identity);
            instanciaBoton.transform.SetParent(contenedor.transform, false);

            lista[i].botonObjeto = instanciaBoton.GetComponent<Button>();


            EstablecerTextoPrecioBotones(lista[i]);
            EstablecerImagen(lista[i]);

        }
    }

    private static void EstablecerTextoPrecioBotones(ObjetoComprable copiaDatosInstancia)
    {
        TextMeshProUGUI precio = copiaDatosInstancia.botonObjeto.GetComponentInChildren<TextMeshProUGUI>();
        precio.text = (copiaDatosInstancia.precio).ToString() + (" $");
    }

    private void EstablecerImagen(ObjetoComprable objeto)
    {
        if (objeto.imagenCrafteable != null)
        {
            Image imagenHija = objeto.botonObjeto.transform.Find("Image").GetComponent<Image>();
            imagenHija.sprite = objeto.imagenCrafteable;
        }
    }

    private void InicializarBotones(ref List<ObjetoComprable> lista)
    {
        for (int i = 0; i < lista.Count; i++)
        {
            int indexCapturado = i; // Captura el índice en una variable local (clave para evitar bugs de cierre)
            AsignarCallbackCompra(lista, i, indexCapturado);
        }

        
    }

    private void AsignarCallbackCompra(List<ObjetoComprable> lista, int i, int indexCapturado)
    {
        lista[i].botonObjeto.onClick.AddListener(() =>
        {
            moduloCompra.ComprarObjeto(lista[indexCapturado].botonObjeto, lista);
        });
    }
}
