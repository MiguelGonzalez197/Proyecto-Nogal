using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public event System.Action EnInventarioActualizado;

    private const int DineroMaximo = 100000;

    [Header("Economia")]
    [SerializeField]
    private int dinero = 0;
    [SerializeField]
    private int ganancias = 2000;
    [SerializeField]
    private int perdidasPenitencias = 4000;

    [Header("Estadisticas")]
    [SerializeField] private int aciertos = 0;
    [SerializeField] private int desaciertos = 0;

    [Header("Residuos recolectados")]
    [SerializeField] private List<DatosItem> aprovechables = new List<DatosItem>();
    [SerializeField] private List<DatosItem> noAprovechables = new List<DatosItem>();
    [SerializeField] private List<DatosItem> organicosAprovechables = new List<DatosItem>();

    public int ObtenerDinero() { return dinero; }
    public int ObtenerAprovechables() { return aprovechables.Count; }
    public int ObtenerOrganicos() { return organicosAprovechables.Count; }
    public int ObtenerAciertos() { return aciertos; }
    public int ObtenerDesaciertos() { return desaciertos; }
    public bool TieneAprovechables(int cantidad) { return aprovechables.Count >= cantidad; }
    public bool TieneOrganicosAprovechables(int cantidad) { return organicosAprovechables.Count >= cantidad; }

    public void AgregarItemInventario(DatosItem datosItem)
    {
        if (!AgregarItemSegunTipo(datosItem)) return;
        NotificarCambioInventario();
    }

    public void AgregarDinero()
    {
        dinero = Mathf.Clamp(dinero + ganancias, 0, DineroMaximo);
        NotificarCambioInventario();
    }

    public void RegistrarClasificacionCorrecta(DatosItem datosItem)
    {
        if (!AgregarItemSegunTipo(datosItem)) return;

        dinero = Mathf.Clamp(dinero + ganancias, 0, DineroMaximo);
        aciertos++;
        NotificarCambioInventario();
    }

    public void RegistrarClasificacionIncorrecta()
    {
        dinero = Mathf.Clamp(dinero - perdidasPenitencias, 0, DineroMaximo);
        desaciertos++;
        NotificarCambioInventario();
    }

    public void DisminuirDineroError()
    {
        dinero = Mathf.Clamp(dinero - perdidasPenitencias, 0, DineroMaximo);
        NotificarCambioInventario();
    }

    public void DisminuirDinero(int cantidad)
    {
        dinero = Mathf.Clamp(dinero - cantidad, 0, DineroMaximo);
        NotificarCambioInventario();
    }

    public void AsegurarDinero(int cantidadMinima)
    {
        int dineroAnterior = dinero;
        dinero = Mathf.Max(dinero, cantidadMinima);

        if (dinero != dineroAnterior)
        {
            NotificarCambioInventario();
        }
    }

    public void DisminuirAprovechables(int cantidad)
    {
        EliminarItems(aprovechables, cantidad);
    }

    public void DisminuirOrganicosAprovechables(int cantidad)
    {
        EliminarItems(organicosAprovechables, cantidad);
    }

    public void RegistrarAcierto()
    {
        aciertos++;
        NotificarCambioInventario();
    }

    public void RegistrarDesacierto()
    {
        desaciertos++;
        NotificarCambioInventario();
    }

    private bool AgregarItemSegunTipo(DatosItem datosItem)
    {
        switch (datosItem.tipoReciclaje)
        {
            case ETipoReciclaje.Aprovechable:
                aprovechables.Add(datosItem);
                return true;
            case ETipoReciclaje.NoAprovechable:
                noAprovechables.Add(datosItem);
                return true;
            case ETipoReciclaje.OrganicoAprovechable:
                organicosAprovechables.Add(datosItem);
                return true;
            default:
                return false;
        }
    }

    private void EliminarItems(List<DatosItem> items, int cantidad)
    {
        if (cantidad <= 0 || items.Count == 0) return;

        int cantidadAEliminar = Mathf.Min(cantidad, items.Count);
        items.RemoveRange(items.Count - cantidadAEliminar, cantidadAEliminar);
        NotificarCambioInventario();
    }

    private void NotificarCambioInventario()
    {
        RegistrarInventario();

        if (EnInventarioActualizado != null)
        {
            EnInventarioActualizado();
        }
    }

    private void RegistrarInventario()
    {
        if (GestorDatos.instancia != null && GestorDatos.instancia.CompletoTutorial())
        {
            GestorDatos.instancia.RegistrarReciclaje(this);
        }
    }
}
