using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuloSeparacion : Modulo
{
    [Space]
    [Header("Prefabs Modulo Separacion")]
    [SerializeField]
    private List<Bolsa> prefabsBolsas = new List<Bolsa>();

    [Header("Valores Modulo Separacion")]
    [Range(1,5)]
    [SerializeField]
    private int numeroBolsasDisponibles = 3;

    private List<GameObject> prefabsResiduosBolsa = new List<GameObject>();
    private List<Vector3> spawnsContenido = new List<Vector3>();
    private Queue<GameObject> referenciasResiduosBolsa = new Queue<GameObject>();

    private Bolsa bolsaActual;
    private GameObject itemActual;
    private IItem interfaceItemActual;
    private int bolsasHechas = 0;
    private bool bEstaLaCamaraALaIzquierda = true;

    protected override void Start()
    {
        base.Start();
        InicializarUbicacionesContenido();
    }

    public override void Interactuar()
    {
        base.Interactuar();
        bolsasHechas = 0;
        SpawnearBolsa();
    }

    public void SpawnearBolsa()
    {
        if (bolsasHechas != numeroBolsasDisponibles)
        {
            int random = Random.Range(0, 100); // De 0 a 99

            int indexPrefab;
            if (random < 50)         // 0–49 (50%)
                indexPrefab = 0;
            else if (random < 80)    // 50–79 (30%)
                indexPrefab = 1;
            else                     // 80–99 (20%)
                indexPrefab = 2;
            bolsaActual = Instantiate(prefabsBolsas[indexPrefab], posicionesItems[0].position, Quaternion.identity);
        }
        else
        {
            StartCoroutine(VolverCamara());
        }
    }

    public void AbrirBolsa()
    {
        if (bolsaActual == null) return;
        prefabsResiduosBolsa = bolsaActual.ObtenerContenido();
        StartCoroutine(DestruirBolsa());
    }

    public void MoverCamara()
    {
        if (posicionesCamara.Count < 2) return;
        if(bEstaLaCamaraALaIzquierda)
        {
            StartCoroutine(gameManager.MoverCamara(posicionesCamara[1], duracionMovimientoCamara));
            SiguienteItem();
        }
        else
        {
            StartCoroutine(gameManager.MoverCamara(posicionesCamara[0], duracionMovimientoCamara));
        }
        bEstaLaCamaraALaIzquierda = !bEstaLaCamaraALaIzquierda;
    }

    public IEnumerator MoverItem(Transform posicion)
    {
        if (interfaceItemActual != null)
        {
            interfaceItemActual.MoverHaciaPosicion(posicion, duracionMovimientoCamara);
            yield return new WaitForSeconds(1f);
            interfaceItemActual.TerminarInteraccionItem();
        }
        
    }

    private void InicializarUbicacionesContenido()
    {
        Transform puntoSpawnItems = posicionesItems[0];
        spawnsContenido.Add(puntoSpawnItems.position);
        spawnsContenido.Add(puntoSpawnItems.position + puntoSpawnItems.forward * 0.5f + puntoSpawnItems.up * 0.3f);
        spawnsContenido.Add(puntoSpawnItems.position + puntoSpawnItems.forward * -0.5f - puntoSpawnItems.up * 0.3f);
        spawnsContenido.Add(puntoSpawnItems.position + puntoSpawnItems.right * 0.5f);
        spawnsContenido.Add(puntoSpawnItems.position + puntoSpawnItems.right * -0.5f);
    }

    private void SpawnearContenido()
    {
        int cantidad = spawnsContenido.Count;
        for (int i = 0; i < cantidad; i++)
        {
            GameObject instancia = Instantiate(prefabsResiduosBolsa[i], spawnsContenido[i], Quaternion.identity);
            referenciasResiduosBolsa.Enqueue(instancia);
            interfaceItemActual = instancia.GetComponent<IItem>();
            if (interfaceItemActual == null) return;
            interfaceItemActual.EnItemDestruido += SiguienteItem;
        }
    }

    private void SiguienteItem()
    {
        if(referenciasResiduosBolsa.Count > 0)
        {
            itemActual = referenciasResiduosBolsa.Dequeue();
            interfaceItemActual = itemActual.GetComponent<IItem>();
            if (interfaceItemActual != null)
            {
                interfaceItemActual.MoverHaciaPosicion(posicionesItems[1], duracionMovimientoCamara);
            }
        }
        else
        {
            MoverCamara();
            SpawnearBolsa();
        }
        
        
    }

    
    private IEnumerator DestruirBolsa()
    {
        yield return new WaitForSeconds(1f);
        bolsasHechas++;
        Destroy(bolsaActual.gameObject);
        SpawnearContenido();
    }
}
