using UnityEngine;
using System.Collections.Generic;

public class Bolsa : Item
{

    [Header("Contenido")]
    [SerializeField]
    private List<GameObject> residuos = new List<GameObject>();

    [Header("Prefabs")]
    [SerializeField]
    private List<GameObject> prefabsAprovechables = new List<GameObject>();
    [SerializeField]
    private List<GameObject> prefabsNoAprovechables = new List<GameObject>();
    [SerializeField]
    private List<GameObject> prefabsOrganicosAprovechables = new List<GameObject>();


    protected override void Start()
    {
        base.Start();
        rigidbodyItem.useGravity = false;
        CrearContenido();
    }

    public List<GameObject> ObtenerContenido()
    {
        return residuos;
    }

    public override void MoverHaciaPosicion(Transform posicion, float duracion)
    {
        
        base.MoverHaciaPosicion(posicion, duracion);
    }


    private void CrearContenido()
    {
        for (int i = 0; i < residuos.Count; i++)
        {
            residuos[i] = EscogerResiduoParaBolsa(datosItem.tipoReciclaje);
        }
    }

    private GameObject EscogerResiduoParaBolsa(ETipoReciclaje tipoBolsa)
    {
        switch (tipoBolsa)
        {
            case ETipoReciclaje.Aprovechable:
                return EscogerDeLista(prefabsAprovechables);

            case ETipoReciclaje.NoAprovechable:
                return EscogerResiduoAleatorioDeCualquierLista();

            case ETipoReciclaje.OrganicoAprovechable:
                return EscogerDeLista(prefabsOrganicosAprovechables);

            default:
                return null;
        }
    }

    private GameObject EscogerDeLista(List<GameObject> lista)
    {
        if (lista == null || lista.Count == 0)
            return null;

        int index = Random.Range(0, lista.Count);
        return lista[index];
    }

    private GameObject EscogerResiduoAleatorioDeCualquierLista()
    {
        List<GameObject> todosLosPrefabs = new List<GameObject>();
        todosLosPrefabs.AddRange(prefabsAprovechables);
        todosLosPrefabs.AddRange(prefabsNoAprovechables);
        todosLosPrefabs.AddRange(prefabsOrganicosAprovechables);

        return EscogerDeLista(todosLosPrefabs);
    }
}
