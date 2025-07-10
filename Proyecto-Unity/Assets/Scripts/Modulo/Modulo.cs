using System.Collections;
using UnityEngine;

public class Modulo : MonoBehaviour , IInteractuable
{
    [Header("Referencias")]
    [SerializeField]
    private GameManager GameManager;
    [SerializeField] 
    private Transform PuntoCamara;

    [Header("Valores")]
    [SerializeField]
    private Vector3 RotacionCamara = new Vector3(0f, 180f, 0f);
    [Range(0, 2)]
    [SerializeField]
    private float duracionRotacionCamara = 0.5f;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void Interactuar()
    {
        Debug.Log("Interactuando");
        if(GameManager != null)
        {
            GameManager.MoverACamaraFija(PuntoCamara);
            StartCoroutine(RotarCamara());
            StartCoroutine(VolverCamara());
        }
    }

    private IEnumerator RotarCamara()
    {
        Debug.Log("Volviendo Camara");
        yield return new WaitForSeconds(2f);
        StartCoroutine(GameManager.RotarCamara(Quaternion.Euler(RotacionCamara), duracionRotacionCamara));
    }

    private IEnumerator VolverCamara()
    {
        Debug.Log("Volviendo Camara");
        yield return new WaitForSeconds(10f);
        GameManager.RestaurarCamara();
    }
}
