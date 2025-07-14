using UnityEngine;

public class GestorCajaTexto : MonoBehaviour
{
    public static GestorCajaTexto Instancia { get; private set; }

    [Header("Prefab y Padre")]
    public GameObject prefabCajaTexto;    // prefab con CajaTextoReutilizable
    public Transform contenedorCanvas;    

    void Awake()
    {
        if (Instancia == null)
            Instancia = this;
        else
            Destroy(gameObject);
    }

    /// <summary>
    /// API global para mostrar un mensaje.
    /// </summary>
    public void MostrarMensaje(string mensaje, float? duracion = null)
    {
        GameObject go = Instantiate(prefabCajaTexto, contenedorCanvas, worldPositionStays: false);
        CajaTextoReutilizable caja = go.GetComponent<CajaTextoReutilizable>();
        if (duracion.HasValue)
            caja.tiempoCierreAutomatico = duracion.Value;
        caja.Mostrar(mensaje);
    }
}
