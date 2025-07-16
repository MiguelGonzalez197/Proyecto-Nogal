using UnityEngine;

public class InteraccionCanecas : MonoBehaviour , IInteractuable
{
    private ModuloSeparacion moduloSeparacion;

    void Start()
    {
        moduloSeparacion = GetComponentInParent<ModuloSeparacion>();
    }

    public void Interactuar()
    {
        if(moduloSeparacion != null)
        {
            StartCoroutine(moduloSeparacion.MoverItem(transform));
            
        }
    }
}
