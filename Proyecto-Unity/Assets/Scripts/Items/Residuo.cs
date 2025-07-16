using System.Collections;
using UnityEngine;

public class Residuo : MonoBehaviour , IItem
{
    public event System.Action EnItemDestruido;

    [SerializeField]
    private DatosItem DatosResiduo;

    [SerializeField]
    private float velocidadRotacion;

    private Vector3 rotacion;
    private Vector3 nuevaRotacion;
    private Rigidbody rigidbodyItem;

    void Start()
    {
        rigidbodyItem = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rotacion = new Vector3(0f, velocidadRotacion * Time.deltaTime, 0f);
        nuevaRotacion = transform.eulerAngles + rotacion;
        transform.eulerAngles = nuevaRotacion;
    }

    private void OnDestroy()
    {
        if(EnItemDestruido != null)
        {
            EnItemDestruido();
        }
    }

    public DatosItem ObtenerDatosItem() { return DatosResiduo; }

    public void MoverHaciaPosicion(Transform posicion, float duracion)
    {
        StartCoroutine(InterpolarPosicion(posicion, duracion));
    }

    public void TerminarInteraccionItem()
    {
        if(rigidbodyItem != null)
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
