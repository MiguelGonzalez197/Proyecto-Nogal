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
    private GameObject mensajeActual;

    void Awake()
    {
        if (Instancia != null && Instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        Instancia = this;
        transform.SetParent(null);
        DontDestroyOnLoad(gameObject);
    }
   
    


   
    public void MostrarMensaje(string mensaje, float? duracion = null, bool cerrarPrevio = false)
    {
        if (cerrarPrevio && mensajeActual != null)
        {
            Destroy(mensajeActual);
            mensajeActual = null;
        }

        mensajeActual = Instantiate(prefabCajaTexto, contenedorCanvas, worldPositionStays: false);
        CajaTextoReutilizable caja = mensajeActual.GetComponent<CajaTextoReutilizable>();
        if (duracion.HasValue)
        {
            caja.tiempoCierreAutomatico = duracion.Value;
            caja.Mostrar(mensaje);
        }
    }
        
        
    
    /// <summary>
    /// Oculta el mensaje actualmente visible si existe.
    /// </summary>
  public void OcultarMensajeActual()
    {
    if (mensajeActual == null) return;

    CajaTextoReutilizable caja = mensajeActual.GetComponent<CajaTextoReutilizable>();
    if (caja != null)
        caja.Ocultar();

    Destroy(mensajeActual);
    mensajeActual = null;
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

