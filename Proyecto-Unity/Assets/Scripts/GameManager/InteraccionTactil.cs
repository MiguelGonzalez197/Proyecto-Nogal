using UnityEngine;

public class InteraccionTactil : MonoBehaviour
{
    // ───────────────────────────────────────
    // 1. REFERENCIAS SERIALIZADAS
    // ───────────────────────────────────────
    [Header("Valores")]
    [SerializeField]
    float maxDistanciaTap = 10f;  // En píxeles cual es la cantidad maxima para mover la pantalla y contar como tap
    [SerializeField]
    float maxDuracionTap = 0.3f;  // En segundos cual es el maximo que debe durar la interacion con la pantallar para contar como tap

    // ───────────────────────────────────────
    // 2. CAMPOS PRIVADOS INTERNOS
    // ───────────────────────────────────────
    private Vector2 posicionInicioToque;
    private float tiempoInicioToque;

    // ───────────────────────────────────────
    // 3. MÉTODOS UNITY
    // ───────────────────────────────────────
    void Update()
    {
        InteraccionTactilMovil();
        InteraccionTactilPC();
    }

    // ───────────────────────────────────────
    // 4. MÉTODOS PRIVADOS
    // ───────────────────────────────────────
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

                if (PuedeRealizarInteraccion(duracion, distancia))
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

            if (PuedeRealizarInteraccion(duracion, distancia))
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

    private bool PuedeRealizarInteraccion(float duracion, float distancia)
    {
        return duracion < maxDuracionTap && distancia < maxDistanciaTap;
    }
}
