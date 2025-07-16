using System.Collections;
using UnityEngine;

public class Residuo : Item
{

    [SerializeField]
    private float velocidadRotacion;

    private Vector3 rotacion;
    private Vector3 nuevaRotacion;

    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        rotacion = new Vector3(0f, velocidadRotacion * Time.deltaTime, 0f);
        nuevaRotacion = transform.eulerAngles + rotacion;
        transform.eulerAngles = nuevaRotacion;

    }


   

}
