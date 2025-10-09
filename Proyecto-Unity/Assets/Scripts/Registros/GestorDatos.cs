using UnityEngine;

public class GestorDatos : MonoBehaviour
{
    public static GestorDatos instancia;

    [SerializeField]
    private RegistroJugador registroJugador;

    private float tiempoOcurrido;

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject); // Mantiene el objeto al cambiar de escena
            //RecyclingLogger.Log($"Session started for {playerName}");
        }
        else
        {
            Destroy(gameObject); // Evita duplicados
        }

        registroJugador.tutorialCompletado = false;
    }


    void Update()
    {
        RegistrarTiempo();
    }

    void OnApplicationPause(bool pause)
    {
        if (pause) GuardarDatos();
    }

    void OnApplicationQuit()
    {
        GuardarDatos();
    }

    public void RegistrarDatosJugador(string nombre, int edad)
    {
        registroJugador.nombreJugador = nombre;
        registroJugador.edadJugador = edad;
    }

    public void RegistrarReciclaje(Inventario inventario)
    {
        registroJugador.aciertosTotales = inventario.ObtenerAciertos();
        registroJugador.desaciertosTotales = inventario.ObtenerDesaciertos();
        registroJugador.dineroRecolectado = inventario.ObtenerDinero();
    }

    public void RegistrarExitoTutorial(bool bTutorialCompletado)
    {
        registroJugador.tutorialCompletado = bTutorialCompletado;
    }

    public bool CompletoTutorial()
    {
        return registroJugador.tutorialCompletado;
    }

    private void GuardarDatos()
    {
        RegistradorReciclaje.RegistrarDatos(registroJugador); ;
    }

    private void RegistrarTiempo()
    {
        tiempoOcurrido += Time.deltaTime;
        int minutos = Mathf.FloorToInt(tiempoOcurrido / 60);
        int segundos = Mathf.FloorToInt(tiempoOcurrido % 60);
        registroJugador.tiempoSesion = string.Format("{0:00}:{1:00}", minutos, segundos);
    }
}
