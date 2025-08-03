using UnityEngine;

public class GestorCajaTexto : MonoBehaviour
{
    public static GestorCajaTexto Instancia { get; private set; }

    [Header("Prefab y Padre")]
    [SerializeField]
    private GameObject prefabCajaTexto;    // prefab con CajaTextoReutilizable
    [SerializeField]
    private Transform contenedorCanvas;    

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
}
