using UnityEngine;

public class ControladorCamara : MonoBehaviour
{
    [SerializeField]
    private Transform Camara;               //Referencia Camara

    private CamaraOrbital camaraOrbital;    //Referencia script CamaraOrbital

    void Start()
    {
        camaraOrbital = GetComponentInChildren<CamaraOrbital>();
    }

    void Update()
    {
        
    }

    public void MoverACamaraFija(Transform Destino)
    {

    }
    

}
