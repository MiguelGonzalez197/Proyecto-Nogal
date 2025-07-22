using UnityEngine;
using System.Collections.Generic;

public class ModuloCompra : Modulo
{
    [SerializeField]
    private AnimacionesRecicladora recicladora;

    protected override void Start()
    {
        base.Start();
    }


    public override void Interactuar()
    {
        base.Interactuar();
        if(recicladora != null)
        {
            recicladora.SaludarModulo();
        }
    }
}
