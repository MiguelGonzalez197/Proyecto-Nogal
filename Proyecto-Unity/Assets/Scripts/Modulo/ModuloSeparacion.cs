using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModuloSeparacion : Modulo
{
    [Header("Referencias Modulo")]
    [SerializeField]
    private Transform puntoCamaraCanecas;
    [SerializeField]
    private Transform spawnItems;
    [SerializeField]
    private Transform posicionItemCanecas;

    [Header("Prefabs Modulo")]
    [SerializeField]
    private List<Bolsa> prefabsBolsas = new List<Bolsa>();

    [Header("Valores Modulo")]
    [SerializeField]
    protected Vector3 rotacionCamaraMesa = new Vector3(10.35f, 90f, 0f);
    [Range(1,5)]
    [SerializeField]
    private int numeroBolsasDisponibles = 3;
    [SerializeField]
    private float velocidadItem;

    private List<GameObject> residuosBolsa = new List<GameObject>();
    private List<Vector3> spawnsContenido = new List<Vector3>();
    private Queue<GameObject> referenciasResiduosBolsa = new Queue<GameObject>();

    private Bolsa bolsaActual;
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
        if(bolsasHechas != numeroBolsasDisponibles)
        {
            int indexPrefab = Random.Range(0, prefabsBolsas.Count);
            bolsaActual = Instantiate(prefabsBolsas[0], spawnItems.position, Quaternion.identity);
        }
    }

    public void AbrirBolsa()
    {
        if (bolsaActual == null) return;
        residuosBolsa = bolsaActual.ObtenerContenido();
        StartCoroutine(DestruirBolsa());
    }

    public void MoverCamara()
    {
        if(bEstaLaCamaraALaIzquierda)
        {
            StartCoroutine(gameManager.MoverCamara(Quaternion.Euler(rotacionCamaraCanecas), puntoCamaraCanecas.position, duracionRotacionCamara));
            //SiguienteItem();
        }
        else
        {
            StartCoroutine(gameManager.MoverCamara(Quaternion.Euler(rotacionCamaraMesa), puntoCamaraMesa.position, duracionRotacionCamara));
        }
        bEstaLaCamaraALaIzquierda = !bEstaLaCamaraALaIzquierda;
    }

    private void InicializarUbicacionesContenido()
    {
        spawnsContenido.Add(spawnItems.position);
        spawnsContenido.Add(spawnItems.position + spawnItems.forward * 0.5f + spawnItems.up * 0.3f);
        spawnsContenido.Add(spawnItems.position + spawnItems.forward * -0.5f - spawnItems.up * 0.3f);
        spawnsContenido.Add(spawnItems.position + spawnItems.right * 0.5f);
        spawnsContenido.Add(spawnItems.position + spawnItems.right * -0.5f);
    }

    private void SpawnearContenido()
    {
        int cantidad = spawnsContenido.Count;
        for (int i = 0; i < cantidad; i++)
        {
            GameObject instancia = Instantiate(residuosBolsa[i], spawnsContenido[i], Quaternion.identity);
            referenciasResiduosBolsa.Enqueue(instancia);
        }
    }

    private void SiguienteItem()
    {
        GameObject instancia = referenciasResiduosBolsa.Dequeue();
        IItem Item = instancia.GetComponent<IItem>();
        if(Item != null)
        {
            Item.MoverHaciaPosicion(posicionItemCanecas, velocidadItem);
        }
        
    }

    private IEnumerator DestruirBolsa()
    {
        yield return new WaitForSeconds(1f);
        Destroy(bolsaActual.gameObject);
        SpawnearContenido();
    }
}
