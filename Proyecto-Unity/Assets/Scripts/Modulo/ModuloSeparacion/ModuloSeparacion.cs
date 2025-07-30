using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ModuloSeparacion : Modulo
{
    // ───────────────────────────────────────
    // 1. REFERENCIAS SERIALIZADAS
    // ───────────────────────────────────────

    [Space]
    [Header("Referencias Modulo Separacion")]
    [SerializeField]
    private List<GameObject> botonesCanva = new List<GameObject>();

    [Header("Prefabs Modulo Separacion")]
    [SerializeField]
    private List<Bolsa> prefabsBolsas = new List<Bolsa>();                          

    [Header("Valores Modulo Separacion")]
    [Range(1,5)]
    [SerializeField]
    private int numeroBolsasDisponibles = 3;                                        // Establece un limite de bolsas por cada vez que se interactua con el modulo

    // ───────────────────────────────────────
    // 2. CAMPOS PRIVADOS INTERNOS
    // ───────────────────────────────────────

    private List<GameObject> prefabsResiduosBolsa = new List<GameObject>();         // Copia de los prefabs generados por la bolsa
    private List<Vector3> spawnsContenido = new List<Vector3>();                    // Lista de posiciones para cada contenido de las bolsas
    private Queue<GameObject> referenciasResiduosBolsa = new Queue<GameObject>();   // Fila FIFO con referencias de los items generados

    private Bolsa bolsaActual;                                                      // Referencia al componente de la bolsa generada
    private GameObject itemActual;                                                  // Referencia al item con el que se esta tratando
    private IItem interfaceItemActual;                                              // Interface del item actual
    private int bolsasHechas = 0;                                                   // Contador de cada bolsa realizada
    private bool bEstaLaCamaraALaIzquierda = true;                                  // Booleano que permite saber si la camara esta en la posicion 1 o la posicion 2

    // ───────────────────────────────────────
    // 3. MÉTODOS UNITY
    // ───────────────────────────────────────

    protected override void Start()
    {
        base.Start();
        InicializarUbicacionesContenido();
    }

    // ───────────────────────────────────────
    // 4. MÉTODOS PÚBLICOS
    // ───────────────────────────────────────

    public override void Interactuar()
    {
        base.Interactuar();
        bolsasHechas = 0;
        SpawnearBolsa();
    }

    /// <summary>
    /// Permite generar los prefabs puestos de las bolsas, las bolsas negras (las no aprovechables) tienen un 50% de probabilidad de salir, las bolsas
    /// blancas (las aprovechables) un 30% y las bolsas verdes un 20%
    /// </summary>
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
            bolsaActual = Instantiate(prefabsBolsas[indexPrefab], spawnsContenido[2], Quaternion.identity);
        }
        else
        {
            StartCoroutine(SalirModulo());
        }
    }

    /// <summary>
    /// Callback boton canva
    /// </summary>
    public void AbrirBolsa()
    {
        if (bolsaActual == null) return;

        DatosItem datosItem = bolsaActual.ObtenerDatosItem();
        if (!EsBolsaNegra(datosItem))
        {
            Debug.Log("No era necesario abrirla");
        }

        prefabsResiduosBolsa = bolsaActual.ObtenerContenido();
        StartCoroutine(DestruirBolsa());
        
    }

    /// <summary>
    /// Callback boton canva
    /// </summary>
    public void BotarBolsa()
    {
        if (bolsaActual == null) return;

        DatosItem datosItem = bolsaActual.ObtenerDatosItem();
        if (EsBolsaNegra(datosItem))
        {
            Debug.Log("Era una bolsa No aprovechable, no tienes que botarlo completo");
        }

        RegistrarEnFilaReferenciaResiduosBolsa(bolsaActual.gameObject);
        SuscribirItemADestruccion(bolsaActual.gameObject);
        ActivarBotonMoverCamara(true);
        ActivarBotonesMesa(false);
    }

    public void MoverCamara()
    {
        if (posicionesCamara.Count < 2) return;
        if(bEstaLaCamaraALaIzquierda)
        {
            ActivarBotonMoverCamara(false);
            StartCoroutine(gameManager.MoverCamara(posicionesCamara[1], duracionMovimientoCamara));
            SiguienteItem();
        }
        else
        {
            ActivarBotonesMesa(true);
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

    // ───────────────────────────────────────
    // 5. MÉTODOS PRIVADOS
    // ───────────────────────────────────────

    /// <summary>
    /// Estableces los puntos de aparacion del contenido de las bolsas
    /// </summary>
    private void InicializarUbicacionesContenido()
    {
        Transform puntoSpawnItems = posicionesItems[0];
        spawnsContenido.Add(puntoSpawnItems.position);
        spawnsContenido.Add(puntoSpawnItems.position + puntoSpawnItems.forward * 0.5f + puntoSpawnItems.up * 0.3f);
        spawnsContenido.Add(puntoSpawnItems.position + puntoSpawnItems.forward * -0.5f - puntoSpawnItems.up * 0.3f);
        spawnsContenido.Add(puntoSpawnItems.position + puntoSpawnItems.right * 0.5f);
        spawnsContenido.Add(puntoSpawnItems.position + puntoSpawnItems.right * -0.5f);
    }

    private void SpawnearContenidoBolsa()
    {
        int cantidad = spawnsContenido.Count;
        for (int i = 0; i < cantidad; i++)
        {
            GameObject instancia = Instantiate(prefabsResiduosBolsa[i], spawnsContenido[i], Quaternion.identity);
            RegistrarEnFilaReferenciaResiduosBolsa(instancia);
            SuscribirItemADestruccion(instancia);
        }
    }

    private void SiguienteItem()
    {
        if(referenciasResiduosBolsa.Count > 0)
        {
            itemActual = referenciasResiduosBolsa.Dequeue();
            ObtenerInterfaceItem(itemActual);
            if (interfaceItemActual != null)
            {
                interfaceItemActual.MoverHaciaPosicion(posicionesItems[1], duracionMovimientoCamara);
            }
        }
        else
        {
            bolsasHechas++;
            MoverCamara();
            SpawnearBolsa();
        }
        
        
    }

    private void RegistrarEnFilaReferenciaResiduosBolsa(GameObject instancia)
    {
        referenciasResiduosBolsa.Enqueue(instancia);
    }

    private void SuscribirItemADestruccion(GameObject instancia)
    {
        ObtenerInterfaceItem(instancia);
        if (interfaceItemActual == null) return;
        interfaceItemActual.EnItemDestruido += SiguienteItem;
    }

    private void ObtenerInterfaceItem(GameObject instancia)
    {
        interfaceItemActual = instancia.GetComponent<IItem>();
    }

    private void ActivarBotonesMesa(bool bActivar)
    {
        if(botonesCanva.Count > 1)
        {
            botonesCanva[0].SetActive(bActivar);     // Boton abrir bolsa
            botonesCanva[1].SetActive(bActivar);     // Boton botar bolsa
        }
    }

    private void ActivarBotonMoverCamara(bool bActivar)
    {
        if (botonesCanva.Count > 2)
        {
            botonesCanva[2].SetActive(bActivar);
        }
    }

    private IEnumerator DestruirBolsa()
    {
        yield return new WaitForSeconds(1f);
        Destroy(bolsaActual.gameObject);
        SpawnearContenidoBolsa();
        ActivarBotonMoverCamara(true);
        ActivarBotonesMesa(false);
    }

    private bool EsBolsaNegra(DatosItem datosItem)
    {
        return datosItem.tipoReciclaje == ETipoReciclaje.NoAprovechable;
    }
}
