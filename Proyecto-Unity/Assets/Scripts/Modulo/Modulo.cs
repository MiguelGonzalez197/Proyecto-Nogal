
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modulo : MonoBehaviour , IInteractuable
{
    [Header("Referencias")]
    [SerializeField]
    protected GameManager gameManager;
    [SerializeField]
    protected List<Transform> posicionesCamara = new List<Transform>();
    [SerializeField]
    protected List<Transform> posicionesItems = new List<Transform>();
    [SerializeField]
    protected GameObject canvaModulo;

    [Header("Valores")]
    [Range(0, 2)]
    [SerializeField]
    protected float duracionMovimientoCamara = 0.5f;

    protected BoxCollider boxCollider;

    protected virtual void Start()
    {
        
        boxCollider = GetComponent<BoxCollider>();
        
    }

    public virtual void Interactuar()
    {
        if(gameManager != null)
        {
            gameManager.MoverACamaraFija(posicionesCamara[0]);
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

    protected IEnumerator VolverCamara()
    {
        Debug.Log("Volviendo Camara");
        yield return new WaitForSeconds(1.5f);
        gameManager.RestaurarCamara();
        ActivarCanva(false);
        ActivarCollider(true);
    }
}
