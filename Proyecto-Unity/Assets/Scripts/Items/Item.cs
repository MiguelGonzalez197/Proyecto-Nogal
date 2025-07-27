using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour , IItem
{
    public event System.Action EnItemDestruido;

    [SerializeField]
    protected DatosItem datosItem;
    [SerializeField]
    protected bool bPuedeRotar;
    [SerializeField]
    private float velocidadRotacion;

    protected Rigidbody rigidbodyItem;

    private Vector3 rotacion;
    private Vector3 nuevaRotacion;

    protected virtual void Start()
    {
        rigidbodyItem = GetComponent<Rigidbody>();
    }

    protected virtual void Update()
    {
        if (!bPuedeRotar) return;
        rotacion = new Vector3(0f, velocidadRotacion * Time.deltaTime, 0f);
        nuevaRotacion = transform.eulerAngles + rotacion;
        transform.eulerAngles = nuevaRotacion;
    }

    private void OnDestroy()
    {
        if (EnItemDestruido != null)
        {
            EnItemDestruido();
        }
    }

    public DatosItem ObtenerDatosItem() { return datosItem; }

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
