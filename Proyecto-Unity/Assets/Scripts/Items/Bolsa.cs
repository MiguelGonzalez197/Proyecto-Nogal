using UnityEngine;
using System.Collections.Generic;

public class Bolsa : MonoBehaviour
{
    [SerializeField]
    private DatosItem DatosBolsa;

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

    void Start()
    {
        CrearContenido();
    }

    public List<GameObject> ObtenerContenido()
    {
        return residuos;
    }

    private void CrearContenido()
    {
        for (int i = 0; i < residuos.Count; i++)
        {
            int tipo = Random.Range(0, 3);
            switch (tipo)
            {
                case (int)ETipoReciclaje.Aprovechable:
                    residuos[i] = EscogerAprovechables();
                    break;
                case (int)ETipoReciclaje.NoAprovechable:
                    residuos[i] = EscogerNoAprovechables();
                    break;
                case (int)ETipoReciclaje.OrganicoAprovechable:
                    residuos[i] = EscogerOrganicosAprovechables();
                    break;
            }
        }
    }

    private GameObject EscogerAprovechables()
    {
        if(prefabsAprovechables != null)
        {
            int numeroPrefab = Random.Range(0, prefabsAprovechables.Count);

            return prefabsAprovechables[numeroPrefab];
        }

        return null;
    }

    private GameObject EscogerNoAprovechables()
    {
        if (prefabsNoAprovechables != null)
        {
            int numeroPrefab = Random.Range(0, prefabsNoAprovechables.Count);

            return prefabsNoAprovechables[numeroPrefab];
        }

        return null;
    }

    private GameObject EscogerOrganicosAprovechables()
    {
        if (prefabsOrganicosAprovechables != null)
        {
            int numeroPrefab = Random.Range(0, prefabsOrganicosAprovechables.Count);

            return prefabsOrganicosAprovechables[numeroPrefab];
        }

        return null;
    }
}
