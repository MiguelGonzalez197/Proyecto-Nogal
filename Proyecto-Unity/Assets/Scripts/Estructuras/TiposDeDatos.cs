using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct DatosItem
{
    public ETipoItem tipoItem;
    public ETipoReciclaje tipoReciclaje;
}

[System.Serializable]
public struct DatosCompra
{
    public GameObject objetoAVenta;
    public Button botonObjeto;
    public int precio;
}