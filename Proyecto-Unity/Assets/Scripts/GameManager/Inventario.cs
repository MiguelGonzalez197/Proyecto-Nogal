using UnityEngine;
using System.Collections.Generic;

public class Inventario : MonoBehaviour
{
    [Header("Economia")]
    [SerializeField]
    private int dinero = 10000;
    [SerializeField]
    private int ganancias = 2000;
    [SerializeField]
    private int perdidasPenitencias = 4000;

    [Header("Residuos recolectados")]
    [SerializeField]
    List<DatosItem> aprovechables = new List<DatosItem>();
    [SerializeField]
    List<DatosItem> noAprovechables = new List<DatosItem>();
    [SerializeField]
    List<DatosItem> organicosAprovechables = new List<DatosItem>();

    public int ObtenerDinero() { return dinero; }

    public void AgregarItemInventario(DatosItem datosItem)
    {
        switch(datosItem.tipoReciclaje)
        {
            case ETipoReciclaje.Aprovechable:
                aprovechables.Add(datosItem);
                break;
            case ETipoReciclaje.NoAprovechable:
                noAprovechables.Add(datosItem);
                break;
            case ETipoReciclaje.OrganicoAprovechable:
                organicosAprovechables.Add(datosItem);
                break;
        }
    }

    public void AgregarDinero()
    {
        Debug.Log("Agregando dinero");
        dinero += ganancias;
    }

    public void DisminuirDineroError()
    {
        dinero -= perdidasPenitencias;
        dinero = Mathf.Clamp(dinero, 0, 100000);
    }

    public void DisminuirDineroCompra(int cantidad)
    {
        dinero -= cantidad;
        dinero = Mathf.Clamp(dinero, 0, 100000);
    }
}
