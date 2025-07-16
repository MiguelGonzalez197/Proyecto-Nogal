using UnityEngine;
using System.Collections.Generic;

public class Caneca : MonoBehaviour
{
    [Header("Tipo de reciclaje permitido")]
    [SerializeField]
    private ETipoReciclaje tipoCaneca;

    [Header("Contenido")]
    [SerializeField]
    private int cantidad = 0;
    [SerializeField]
    private List<GameObject> residuosRecolectados = new List<GameObject>();

    private GameObject itemActual;
    private IItem interfaceItemActual;
    private DatosItem datosItemActual;
    

    private void OnTriggerEnter(Collider other)
    {
        itemActual = other.gameObject;
        interfaceItemActual = other.GetComponent<IItem>();
        if(interfaceItemActual != null)
        {
            datosItemActual = interfaceItemActual.ObtenerDatosItem();
            ValidarItem(interfaceItemActual); 
        }
    }

    private void ValidarItem(IItem item)
    {
        if(datosItemActual.tipoReciclaje == tipoCaneca)
        {
            Debug.Log("Item Valido");
            AgregarItem();
        }
        else
        {
            Debug.Log("Item Invalido");
        }
    }

    private void AgregarItem()
    {
        residuosRecolectados.Add(itemActual);
        cantidad = residuosRecolectados.Count;
    }


}
