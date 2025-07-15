using System.Collections;
using UnityEngine;

public class Residuo : MonoBehaviour , IItem
{
    [SerializeField]
    private DatosItem DatosResiduo;

    [SerializeField]
    private float velocidadRotacion;

    Vector3 rotacion;
    Vector3 nuevaRotacion;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        rotacion = new Vector3(0f, velocidadRotacion * Time.deltaTime, 0f);
        nuevaRotacion = transform.eulerAngles + rotacion;
        transform.eulerAngles = nuevaRotacion;
        //transform.eulerAngles += velocidadRotacion;
    }

    public DatosItem ObtenerDatosItem() { return DatosResiduo; }

    public void MoverHaciaPosicion(Transform posicion, float duracion)
    {
        StartCoroutine(InterpolarPosicion(posicion, duracion));
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
    
}
