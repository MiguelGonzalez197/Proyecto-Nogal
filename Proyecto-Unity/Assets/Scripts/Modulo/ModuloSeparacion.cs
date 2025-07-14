using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class ModuloSeparacion : Modulo
{
    [Header("Referencias")]
    [SerializeField]
    private Transform spawnItems;

    [Header("Prefaps")]
    [SerializeField]
    private List<GameObject> bolsas = new List<GameObject>();

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Interactuar()
    {
        if(spawnItems != null && bolsas != null)
        {
            foreach (GameObject bolsa in bolsas)
            {
                Instantiate(bolsa, spawnItems.position, Quaternion.identity);
            }
        }

        base.Interactuar();
    }
}
