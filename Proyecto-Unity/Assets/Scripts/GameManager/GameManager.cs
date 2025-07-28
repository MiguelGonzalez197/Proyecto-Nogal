using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    // ───────────────────────────────────────
    // 1. REFERENCIAS SERIALIZADAS
    // ───────────────────────────────────────
    [Header("Referencias")]
    [SerializeField]
    private Transform camara;

    // ───────────────────────────────────────
    // 2. CAMPOS PRIVADOS INTERNOS
    // ───────────────────────────────────────
    private CamaraOrbital camaraOrbital;        
    private Vector3 ultimaPosicionCamara;       // Registro ultima posicion de la camara antes de mover a punto fijo
    private Quaternion ultimaRotacionCamara;    // Registro ultima rotacion de la camara antes de mover a punto fijo

    // ───────────────────────────────────────
    // 3. MÉTODOS UNITY
    // ───────────────────────────────────────
    void Start()
    {
        //Application.targetFrameRate = 60;
        camaraOrbital = GetComponentInChildren<CamaraOrbital>();
        // mensajje de prueba
        //GestorCajaTexto.Instancia.MostrarMensaje("¡Prueba exitosa!", 2f);
    }

    // ───────────────────────────────────────
    // 4. MÉTODOS PÚBLICOS
    // ───────────────────────────────────────

    /// <summary>
    /// Mueve de forma inmediata a un punto fijo
    /// </summary>
    public void MoverACamaraFija(Transform Destino)
    {
        if(camaraOrbital != null)
        {
            ultimaPosicionCamara = camara.position;
            ultimaRotacionCamara = camara.rotation;

            camaraOrbital.enabled = false;

            camara.position = Destino.position;
            camara.rotation = Destino.rotation;
        }
        
    }

    /// <summary>
    /// Interpola la posicion y rotacion de la camara a la transformada dada
    /// </summary>
    public IEnumerator MoverCamara(Transform valorPosicion, float duracion)
    {
        float tiempo = 0f;

        Vector3 inicioPos = camara.position;
        Quaternion inicioRot = camara.rotation;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;

            camara.position = Vector3.Lerp(inicioPos, valorPosicion.position, t);
            camara.rotation = Quaternion.Slerp(inicioRot, valorPosicion.rotation, t);

            yield return null;
        }

        camara.position = valorPosicion.position;
        camara.rotation = valorPosicion.rotation;
    }

    /// <summary>
    /// Permite que la camara vuelva a su posicion original y permite rotar alrededor de la sala
    /// </summary>
    public void RestaurarCamara()
    {
        camara.position = ultimaPosicionCamara;
        camara.rotation = ultimaRotacionCamara;

        camaraOrbital.enabled = true;
    }

    
}
