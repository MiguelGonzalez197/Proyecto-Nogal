using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct DatosItem
{
    public ETipoItem tipoItem;
    public ETipoReciclaje tipoReciclaje;
}

[System.Serializable]
public class ObjetoCrafteable
{
    public GameObject objetoACraftear;
    public Button botonObjeto;
    public int aprovechablesNecesarios;
    public int organicosAprovechablesNecesarios;
    public Sprite imagenCrafteable;
}

[System.Serializable]
public class ObjetoComprable
{
    public GameObject objetoAVenta;
    public Button botonObjeto;
    public int precio;
    public Sprite imagenCrafteable;
}

