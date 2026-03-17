using UnityEngine;

public class GestorCajaTexto : MonoBehaviour
{
    public static GestorCajaTexto Instancia { get; private set; }

    [Header("Prefab y Padre")]
    [SerializeField] private GameObject prefabCajaTexto;
    [SerializeField] private Transform contenedorCanvas;

    [Header("Prefab mensajes de clasificacion")]
    [SerializeField] private GameObject prefabCajaClasificacion;
    [SerializeField] private Transform contenedorClasificacion;

    private CajaTextoReutilizable mensajeActual;
    private CajaTextoReutilizable mensajeClasificacionActual;

    private void Awake()
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
        CajaTextoReutilizable caja = ObtenerOCrearCaja(ref mensajeActual, prefabCajaTexto, contenedorCanvas);
        if (caja == null) return;

        if (cerrarPrevio)
        {
            caja.Ocultar();
        }

        ConfigurarDuracion(caja, duracion);
        caja.Mostrar(mensaje);
    }

    public void OcultarMensajeActual()
    {
        if (mensajeActual == null) return;
        mensajeActual.Ocultar();
    }

    public void MostrarMensajeClasificacion(string mensaje, float? duracion = null)
    {
        if (prefabCajaClasificacion == null || contenedorClasificacion == null)
        {
            MostrarMensaje(mensaje, duracion);
            return;
        }

        CajaTextoReutilizable caja = ObtenerOCrearCaja(ref mensajeClasificacionActual, prefabCajaClasificacion, contenedorClasificacion);
        if (caja == null) return;

        ConfigurarDuracion(caja, duracion);
        caja.Mostrar(mensaje);
    }

    private CajaTextoReutilizable ObtenerOCrearCaja(ref CajaTextoReutilizable cajaActual, GameObject prefab, Transform contenedor)
    {
        if (cajaActual != null) return cajaActual;
        if (prefab == null || contenedor == null) return null;

        GameObject instancia = Instantiate(prefab, contenedor, false);
        cajaActual = instancia.GetComponent<CajaTextoReutilizable>();
        if (cajaActual != null)
        {
            cajaActual.destruirAlOcultar = false;
        }

        return cajaActual;
    }

    private void ConfigurarDuracion(CajaTextoReutilizable caja, float? duracion)
    {
        if (duracion.HasValue)
        {
            caja.tiempoCierreAutomatico = duracion.Value;
            return;
        }

        caja.RestablecerDuracionPredeterminada();
    }
}
