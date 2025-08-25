using UnityEngine;
using System.Collections.Generic;

public class Caneca : MonoBehaviour
{
    // ───────────────────────────────────────
    // 1. REFERENCIAS SERIALIZADAS
    // ───────────────────────────────────────
    [Header("Tipo de reciclaje permitido")]
    [SerializeField]
    private ETipoReciclaje tipoCaneca;                                          // Permite saber que tipo de reciclaje es permitido en la caneca

    [Header("Referencias")]
    [SerializeField]
    private Inventario gestorInventario;

    // ───────────────────────────────────────
    // 2. CAMPOS PRIVADOS INTERNOS
    // ───────────────────────────────────────
    private GameObject itemActual;                                              // Instancia del objeto que esta siendo procesado
    private DatosItem datosItemActual;                                          // Estructura de datos relacionados con el objeto procesado (Info DatosItem -> TiposDeDatos.cs)
    private HashSet<GameObject> itemsProcesados = new HashSet<GameObject>();

    // ───────────────────────────────────────
    // 3. MÉTODOS UNITY
    // ───────────────────────────────────────
    private void OnTriggerEnter(Collider other)
    {
        if (itemsProcesados.Contains(other.gameObject)) return;
        itemActual = other.gameObject;
        ProcesarItem(other.gameObject);
    }

    // ───────────────────────────────────────
    // 4. MÉTODOS PRIVADOS
    // ───────────────────────────────────────
    private void ProcesarItem(GameObject item)
    {
        IItem interfaceItem = item.GetComponent<IItem>();
        if (interfaceItem == null) return;

        DatosItem datosItem = interfaceItem.ObtenerDatosItem();

        if (datosItem.tipoReciclaje != tipoCaneca)
        {
            itemsProcesados.Add(itemActual);
            gestorInventario.DisminuirDineroError();
            gestorInventario.RegistrarDesacierto();
            Debug.Log("Este item no pertenece a esta caneca");
            GestorCajaTexto.Instancia?.MostrarMensajeClasificacion("Ese residuo iba en otra caneca");
            return;
        } 
            

        ValidarItem(item, datosItem);
    }

    private void ValidarItem(GameObject item, DatosItem datosItem)
    {
        switch (datosItem.tipoItem)
        {
            case ETipoItem.Residuo:
                if (itemsProcesados.Contains(item)) return;
                itemsProcesados.Add(itemActual);
                AgregarItem(datosItem);
                break;

            case ETipoItem.Bolsa:
                itemsProcesados.Add(itemActual);

                Debug.Log("Bolsa");
                if (EsBolsaNegra(datosItem))
                {
                    Debug.Log("Es bolsa negra");
                    gestorInventario.RegistrarDesacierto();
                    gestorInventario.DisminuirDineroError();
                    return;
                }
                
                AgregarContenidoBolsa(item);
                break;

            default:
                break;
        }
    }

    private bool EsBolsaNegra(DatosItem datosItem)
    {
        return datosItem.tipoReciclaje == ETipoReciclaje.NoAprovechable;
    }

    private void AgregarContenidoBolsa(GameObject bolsa)
    {
        Bolsa bolsaActual = bolsa.GetComponent<Bolsa>();
        if (bolsaActual == null) return;

        foreach (GameObject residuo in bolsaActual.ObtenerContenido())
        {
            ProcesarItem(residuo); // Reutiliza el mismo flujo
        }
    }

    private void AgregarItem(DatosItem datosItem)
    {
        if (gestorInventario != null)
        {
            gestorInventario.AgregarItemInventario(datosItem);
            gestorInventario.AgregarDinero();
            gestorInventario.RegistrarAcierto();
            GestorCajaTexto.Instancia?.MostrarMensajeClasificacion("Clasificado correctamente");
        }
        if (GestorCajaTexto.Instancia != null)
        {
            GestorCajaTexto.Instancia.OcultarMensajeActual();
        }
    }


}
