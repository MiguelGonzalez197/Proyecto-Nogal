using UnityEngine;

public class InteraccionTactil : MonoBehaviour
{
    [Header("Valores")]
    [SerializeField] 
    float maxDistanciaTap = 10f;  // en píxeles
    [SerializeField] 
    float maxDuracionTap = 0.3f;  // en segundos

    private Vector2 posicionInicioToque;
    private float tiempoInicioToque;

    

    void Update()
    {
        InteraccionTactilMovil();
        InteraccionTactilPC();
    }

    private void InteraccionTactilMovil()
    {
        if (Input.touchCount == 1)
        {
            Touch toque = Input.GetTouch(0);

            if (toque.phase == TouchPhase.Began)
            {
                posicionInicioToque = toque.position;
                tiempoInicioToque = Time.time;
            }

            if (toque.phase == TouchPhase.Ended)
            {
                float duracion = Time.time - tiempoInicioToque;
                float distancia = Vector2.Distance(toque.position, posicionInicioToque);

                if (duracion < maxDuracionTap && distancia < maxDistanciaTap)
                {
                    ProcesarRaycast(toque.position);
                }
            }
        }
    }

    private void InteraccionTactilPC()
    {
        if (Input.GetMouseButtonDown(0))
        {
            posicionInicioToque = Input.mousePosition;
            tiempoInicioToque = Time.time;
        }

        if (Input.GetMouseButtonUp(0))
        {
            float duracion = Time.time - tiempoInicioToque;
            float distancia = Vector2.Distance(Input.mousePosition, posicionInicioToque);

            if (duracion < maxDuracionTap && distancia < maxDistanciaTap)
            {
                ProcesarRaycast(Input.mousePosition);
            }
        }
    }

    private void ProcesarRaycast(Vector2 posicionPantalla)
    {
        Ray rayo = Camera.main.ScreenPointToRay(posicionPantalla);

        if (Physics.Raycast(rayo, out RaycastHit impacto))
        {
            IInteractuable interactuable = impacto.collider.GetComponent<IInteractuable>();
            if (interactuable != null)
            {
                interactuable.Interactuar();
            }
        }
    }
}
