using UnityEngine;
using UnityEngine.SceneManagement;

public class InteraccionTactil : MonoBehaviour
{
    [Header("Valores")]
    [SerializeField] private float maxDistanciaTap = 10f;
    [SerializeField] private float maxDuracionTap = 0.3f;
    [SerializeField] private Camera camaraRaycast;

    private Vector2 posicionInicioToque;
    private float tiempoInicioToque;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        CachearCamara();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.touchSupported && Input.touchCount > 0)
        {
            InteraccionTactilMovil();
            return;
        }

        InteraccionTactilPC();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CachearCamara();
    }

    private void InteraccionTactilMovil()
    {
        if (Input.touchCount != 1) return;

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
        if (camaraRaycast == null)
        {
            CachearCamara();
            if (camaraRaycast == null) return;
        }

        Ray rayo = camaraRaycast.ScreenPointToRay(posicionPantalla);

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

    private void CachearCamara()
    {
        if (camaraRaycast == null)
        {
            camaraRaycast = Camera.main;
        }
    }
}
