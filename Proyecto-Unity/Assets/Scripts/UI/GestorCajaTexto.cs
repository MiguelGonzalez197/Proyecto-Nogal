using UnityEngine;

public class GestorCajaTexto : MonoBehaviour
{
    public static GestorCajaTexto Instancia { get; private set; }

    [Header("Prefab y Padre")]
    [SerializeField]
    private GameObject prefabCajaTexto;    // prefab con CajaTextoReutilizable
    [SerializeField]
    private Transform contenedorCanvas;

    [Header("Prefab mensajes de clasificacion")]
    [SerializeField]
    private GameObject prefabCajaClasificacion;   // prefab anclado a la derecha
    [SerializeField]
    private Transform contenedorClasificacion;    // contenedor lateral

    void Awake()
    {
        if (Instancia == null)
            Instancia = this;
        else
            Destroy(gameObject);
    }

   
    public void MostrarMensaje(string mensaje, float? duracion = null)
    {
        GameObject go = Instantiate(prefabCajaTexto, contenedorCanvas, worldPositionStays: false);
        CajaTextoReutilizable caja = go.GetComponent<CajaTextoReutilizable>();
        if (duracion.HasValue)
            caja.tiempoCierreAutomatico = duracion.Value;
        caja.Mostrar(mensaje);
    }

    /// <summary>
    /// Mensajes que aparecen en el costado derecho al clasificar items.
    /// </summary>
    public void MostrarMensajeClasificacion(string mensaje, float? duracion = null)
    {
        if (prefabCajaClasificacion == null || contenedorClasificacion == null)
        {
            MostrarMensaje(mensaje, duracion);
            return;
        }

        GameObject go = Instantiate(prefabCajaClasificacion, contenedorClasificacion, false);
        CajaTextoReutilizable caja = go.GetComponent<CajaTextoReutilizable>();
        if (duracion.HasValue)
            caja.tiempoCierreAutomatico = duracion.Value;
        caja.Mostrar(mensaje);
    }
}

