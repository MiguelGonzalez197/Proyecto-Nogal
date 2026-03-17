using UnityEngine;
using System.Collections.Generic;

public class Bolsa : Item
{
    private readonly List<GameObject> prefabsMixtos = new List<GameObject>();
    private bool prefabsMixtosInicializados;

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
        bPuedeRotar = false;
        base.Start();
        if (rigidbodyItem != null)
        {
            rigidbodyItem.useGravity = false;
        }
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
        InicializarPrefabsMixtos();
        return EscogerDeLista(prefabsMixtos);
    }

    private void InicializarPrefabsMixtos()
    {
        if (prefabsMixtosInicializados) return;

        prefabsMixtos.Clear();
        prefabsMixtos.AddRange(prefabsAprovechables);
        prefabsMixtos.AddRange(prefabsNoAprovechables);
        prefabsMixtos.AddRange(prefabsOrganicosAprovechables);
        prefabsMixtosInicializados = true;
    }
}
