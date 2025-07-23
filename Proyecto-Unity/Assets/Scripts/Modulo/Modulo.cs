
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Modulo : MonoBehaviour , IInteractuable
{
    // ───────────────────────────────────────
    // 1. REFERENCIAS SERIALIZADAS
    // ───────────────────────────────────────

    [Header("Referencias")]
    [SerializeField]
    protected GameManager gameManager;                                    // GamaManager principal para manejar movimientos de camara, inventario o estadisticas
    [SerializeField]
    protected List<Transform> posicionesCamara = new List<Transform>();   // Posiciones donde la camara se puede mover
    [SerializeField]
    protected List<Transform> posicionesItems = new List<Transform>();    // Posiciones donde los items de cada modulo se pueden generar o mover
    [SerializeField]
    protected GameObject canvaModulo;                                     // Interfaz propia de cada modulo

    [Header("Valores")]
    [Range(0, 2)]
    [SerializeField]
    protected float duracionMovimientoCamara = 0.5f;                      

    // ───────────────────────────────────────
    // 2. CAMPOS PRIVADOS INTERNOS
    // ───────────────────────────────────────

    protected BoxCollider boxCollider;                                     // Referencia al collider del modulo

    // ───────────────────────────────────────
    // 3. MÉTODOS UNITY
    // ───────────────────────────────────────

    protected virtual void Start()
    {
        
        boxCollider = GetComponent<BoxCollider>();
        
    }

    // ───────────────────────────────────────
    // 4. MÉTODOS PÚBLICOS
    // ───────────────────────────────────────

    /// <summary>
    /// Establece la posicion de la camara al interactuar con el modulo
    /// Activa el canva establecido al modulo
    /// </summary>
    public virtual void Interactuar()
    {
        if(gameManager != null || posicionesCamara.Count > 0)
        {
            gameManager.MoverACamaraFija(posicionesCamara[0]);
        }

        ActivarCanva(true);
        ActivarCollider(false);
    }

    /** Callback Boton */
    public void SalirModuloCallback()
    {
        gameManager.RestaurarCamara();
        ActivarCanva(false);
        ActivarCollider(true);
    }

    // ───────────────────────────────────────
    // 5. MÉTODOS PROTEGIDOS
    // ───────────────────────────────────────

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
            boxCollider.enabled = bActivar;
        }
    }

    protected IEnumerator SalirModulo()
    {
        yield return new WaitForSeconds(1.5f);
        gameManager.RestaurarCamara();
        ActivarCanva(false);
        ActivarCollider(true);
    }
}
