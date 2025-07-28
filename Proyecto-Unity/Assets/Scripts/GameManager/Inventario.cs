using UnityEngine;
using System.Collections.Generic;

public class Inventario : MonoBehaviour
{
    // ───────────────────────────────────────
    // 1. REFERENCIAS SERIALIZADAS
    // ───────────────────────────────────────
    [Header("Economia")]
    [SerializeField]
    private int dinero = 10000;                                             // Cantidad de dinero obtenida
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


    // ───────────────────────────────────────
    // 4. MÉTODOS PÚBLICOS
    // ───────────────────────────────────────

    /** <Getters> */
    public int ObtenerDinero() { return dinero; }
    public int ObtenerAprovechables() { return aprovechables.Count; }
    public int ObtenerOrganicos() { return aprovechables.Count; }
    /** </Getters> */

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

    public void DisminuirDinero(int cantidad)
    {
        dinero -= cantidad;
        dinero = Mathf.Clamp(dinero, 0, 100000);
    }

    public void DisminuirAprovechables(int cantidad)
    {
        if (aprovechables.Count < 0) return;
        for(int i = 0; i<cantidad; i++)
        {
            aprovechables.Remove(aprovechables[aprovechables.Count - 1]);
        }
    }

    public void DisminuirOrganicosAprovechables(int cantidad)
    {
        if (aprovechables.Count < 0) return;
        for (int i = 0; i < cantidad; i++)
        {
            organicosAprovechables.Remove(organicosAprovechables[organicosAprovechables.Count - 1]);
        }
    }


}
