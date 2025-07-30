using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour , IItem
{

    // ───────────────────────────────────────
    // 1. REFERENCIAS SERIALIZADAS
    // ───────────────────────────────────────
    [SerializeField]
    protected DatosItem datosItem;
    [SerializeField]
    protected bool bPuedeRotar;
    [SerializeField]
    private float velocidadRotacion;
    
    /** <Eventos> */
    public event System.Action EnItemDestruido;
    /** </Eventos> */

    // ───────────────────────────────────────
    // 2. CAMPOS PRIVADOS INTERNOS
    // ───────────────────────────────────────
    protected Rigidbody rigidbodyItem;
    private Vector3 rotacion;
    private Vector3 nuevaRotacion;

    // ───────────────────────────────────────
    // 3. MÉTODOS UNITY
    // ───────────────────────────────────────
    protected virtual void Start()
    {
        rigidbodyItem = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        ProcesarRotacionItem();
    }



    private void OnDestroy()
    {
        if (EnItemDestruido != null)
        {
            EnItemDestruido();
        }
    }

    // ───────────────────────────────────────
    // 4. MÉTODOS PÚBLICOS
    // ───────────────────────────────────────

    /** <Getters> */
    public DatosItem ObtenerDatosItem() { return datosItem; }
    /** </Getters> */

    public virtual void MoverHaciaPosicion(Transform posicion, float duracion)
    {
        StartCoroutine(InterpolarPosicion(posicion, duracion));
    }

    public void TerminarInteraccionItem()
    {
        if (rigidbodyItem != null)
        {
            rigidbodyItem.useGravity = true;
            StartCoroutine(DestruirItem());
        }
    }

    // ───────────────────────────────────────
    // 5. MÉTODOS PRIVADOS
    // ───────────────────────────────────────
    private void ProcesarRotacionItem()
    {
        if (!bPuedeRotar) return;
        rotacion = new Vector3(0f, velocidadRotacion * Time.deltaTime, 0f);
        nuevaRotacion = transform.eulerAngles + rotacion;
        transform.eulerAngles = nuevaRotacion;
    }

    private IEnumerator InterpolarPosicion(Transform posicion, float duracion)
    {
        float tiempo = 0f;

        Vector3 inicioPos = transform.position;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;

            transform.position = Vector3.Lerp(inicioPos, posicion.position, t);

            yield return null;
        }

        transform.position = posicion.position;
    }

    private IEnumerator DestruirItem()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
