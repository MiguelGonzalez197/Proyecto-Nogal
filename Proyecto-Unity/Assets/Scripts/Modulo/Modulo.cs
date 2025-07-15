using System.Collections;
using UnityEngine;

public class Modulo : MonoBehaviour , IInteractuable
{
    [Header("Referencias")]
    [SerializeField]
    protected GameManager gameManager;
    [SerializeField] 
    protected Transform puntoCamaraMesa;
    [SerializeField]
    protected GameObject canvaModulo;

    [Header("Valores")]
    [SerializeField]
    protected Vector3 rotacionCamaraCanecas = new Vector3(45f, 180f, 0f);
    [Range(0, 2)]
    [SerializeField]
    protected float duracionRotacionCamara = 0.5f;

    protected BoxCollider boxCollider;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider>();
    }

    public virtual void Interactuar()
    {
        if(gameManager != null)
        {
            gameManager.MoverACamaraFija(puntoCamaraMesa);
        }

        ActivarCanva(true);
        ActivarCollider(false);
        //StartCoroutine(RotarCamara());
    }

    protected void ActivarCanva(bool bActivar)
    {
        if(canvaModulo != null)
        {
            canvaModulo.SetActive(bActivar);
        }
        
    }

    protected void ActivarCollider(bool bActivar)
    {
        if(boxCollider != null)
        {
            Debug.Log("Desactivando");
            boxCollider.enabled = bActivar;
        }
    }

    private IEnumerator RotarCamara(Vector3 rotacionCamara)
    {
        Debug.Log("Volviendo Camara");
        yield return new WaitForSeconds(10f);

    }

    private IEnumerator VolverCamara()
    {
        Debug.Log("Volviendo Camara");
        yield return new WaitForSeconds(10f);
        gameManager.RestaurarCamara();
    }
}
