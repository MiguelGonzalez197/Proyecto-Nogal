using System.Collections;
using UnityEngine;

public interface IItem 
{
    DatosItem ObtenerDatosItem();

    void MoverHaciaPosicion(Transform posicion, float duracion);

    void TerminarInteraccionItem();

    event System.Action EnItemDestruido;
}
