using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField]
    private Transform camara;                   // Referencia Camara

    private CamaraOrbital camaraOrbital;        // Referencia script CamaraOrbital
    private Vector3 ultimaPosicionCamara;       // Registro ultima posicion de la camara antes de mover a punto fijo
    private Quaternion ultimaRotacionCamara;    // Registro ultima rotacion de la camara antes de mover a punto fijo

    void Start()
    {
        camaraOrbital = GetComponentInChildren<CamaraOrbital>();
        // mensajje de prueba
        GestorCajaTexto.Instancia.MostrarMensaje("¡Prueba exitosa!", 2f);
    }

    void Update()
    {

    }

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

    public IEnumerator MoverCamara(Quaternion valorRotacion, Vector3 valorPosicion, float duracion)
    {
        float tiempo = 0f;

        Vector3 inicioPos = camara.position;
        Quaternion inicioRot = camara.rotation;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;

            camara.position = Vector3.Lerp(inicioPos, valorPosicion, t);
            camara.rotation = Quaternion.Slerp(inicioRot, valorRotacion, t);

            yield return null;
        }

        camara.position = valorPosicion;
        camara.rotation = valorRotacion;
    }

    public void RestaurarCamara()
    {
        camara.position = ultimaPosicionCamara;
        camara.rotation = ultimaRotacionCamara;

        camaraOrbital.enabled = true;
    }

    
}
