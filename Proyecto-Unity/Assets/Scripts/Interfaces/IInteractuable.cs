using UnityEngine;

public interface IInteractuable
{
    void Interactuar();

    event System.Action EnTerminarInteraccion;
}
