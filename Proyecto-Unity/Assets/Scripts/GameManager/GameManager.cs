using System.Collections;
using UnityEngine;

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
    }

    void Update()
    {
        InteraccionTactilMovil();
        InteraccionTactilPC();
    }

    public void MoverACamaraFija(Transform Destino)
    {
        if(camaraOrbital != null)
        {
            Debug.Log("Moviendo");
            ultimaPosicionCamara = camara.position;
            ultimaRotacionCamara = camara.rotation;

            camaraOrbital.enabled = false;

            camara.position = Destino.position;
            camara.rotation = Destino.rotation;
        }
        
    }

    public IEnumerator RotarCamara(Quaternion valor, float duracion)
    {
        float tiempo = 0f;

        Quaternion inicioRot = camara.rotation;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;

            camara.rotation = Quaternion.Slerp(inicioRot, valor, t);

            yield return null;
        }

        camara.rotation = valor;
    }

    public void RestaurarCamara()
    {
        camara.position = ultimaPosicionCamara;
        camara.rotation = ultimaRotacionCamara;

        camaraOrbital.enabled = true;
    }

    private void InteraccionTactilPC()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray rayo = Camera.main.ScreenPointToRay(Input.mousePosition);
            ProcesarRaycast(rayo);
        }
    }

    private void InteraccionTactilMovil()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Ray rayo = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            ProcesarRaycast(rayo);
        }
    }

    private void ProcesarRaycast(Ray rayo)
    {
        if (Physics.Raycast(rayo, out RaycastHit impacto))
        {
            // Busca un componente que implemente la interfaz IInteractuable
            var interactuable = impacto.collider.GetComponent<IInteractuable>();
            if (interactuable != null)
            {
                interactuable.Interactuar();
            }
        }
    }
}
