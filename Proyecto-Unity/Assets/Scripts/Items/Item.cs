using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour , IItem
{
    public event System.Action EnItemDestruido;

    [SerializeField]
    protected DatosItem datosItem;

    protected Rigidbody rigidbodyItem;

    protected virtual void Start()
    {
        rigidbodyItem = GetComponent<Rigidbody>();
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
