using UnityEngine;
using System.Collections.Generic;

public class Caneca : MonoBehaviour
{
    [Header("Tipo de reciclaje permitido")]
    [SerializeField]
    private ETipoReciclaje tipoCaneca;

    [Header("Referencias")]
    [SerializeField]
    private Inventario gestorInventario;

    private GameObject itemActual;

    private IItem interfaceItemActual;
    private DatosItem datosItemActual;
    private HashSet<GameObject> itemsProcesados = new HashSet<GameObject>();


    private void OnTriggerEnter(Collider other)
    {
        if (itemsProcesados.Contains(other.gameObject)) return;
        itemActual = other.gameObject;
        ProcesarItem(other.gameObject);
    }

    private void ProcesarItem(GameObject item)
    {
        IItem interfaceItem = item.GetComponent<IItem>();
        if (interfaceItem == null) return;

        DatosItem datosItem = interfaceItem.ObtenerDatosItem();

        if (datosItem.tipoReciclaje != tipoCaneca)
        {
            itemsProcesados.Add(itemActual);
            gestorInventario.DisminuirDineroError();
            Debug.Log("Este item no pertenece a esta caneca");
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
        }
    }


}
