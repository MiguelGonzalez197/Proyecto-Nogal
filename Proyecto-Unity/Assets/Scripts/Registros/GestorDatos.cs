using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GestorDatos : MonoBehaviour
{
    public static GestorDatos instancia;

    [SerializeField]
    private RegistroJugador registroJugador;

    [SerializeField]
    private string urlHojaCalculo = "https://script.google.com/macros/s/AKfycbwuNA_g_0YpFvUqm4WJgcH56Ff644ypVCS3gY3CzosRakPIXpPLeonX1L7VxAzIe5Bc4Q/exec";

    private readonly HashSet<IInteractuable> interactuablesRegistrados = new HashSet<IInteractuable>();
    private Coroutine rutinaEnvio;
    private bool hayEnvioPendiente;
    private float tiempoInicioSesion;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        registroJugador.tutorialCompletado = false;
        registroJugador.sesionId = System.Guid.NewGuid().ToString();
        tiempoInicioSesion = Time.realtimeSinceStartup;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            GuardarDatos();
        }
    }

    private void OnApplicationQuit()
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

    public void RegistrarExitoTutorial(bool tutorialCompletado)
    {
        registroJugador.tutorialCompletado = tutorialCompletado;
    }

    public void RegistrarInteractuableGuardado(IInteractuable interactuable)
    {
        if (interactuable == null || interactuablesRegistrados.Contains(interactuable)) return;

        interactuablesRegistrados.Add(interactuable);
        interactuable.EnTerminarInteraccion += GuardarDatos;
    }

    public bool CompletoTutorial()
    {
        return registroJugador.tutorialCompletado;
    }

    private void GuardarDatos()
    {
        ActualizarTiempoSesion();

        if (rutinaEnvio != null)
        {
            hayEnvioPendiente = true;
            return;
        }

        rutinaEnvio = StartCoroutine(Enviar(registroJugador));
    }

    private void ActualizarTiempoSesion()
    {
        float tiempoOcurrido = Time.realtimeSinceStartup - tiempoInicioSesion;
        int minutos = Mathf.FloorToInt(tiempoOcurrido / 60f);
        int segundos = Mathf.FloorToInt(tiempoOcurrido % 60f);
        registroJugador.tiempoSesion = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    private IEnumerator Enviar(RegistroJugador datosSesion)
    {
        string json = JsonUtility.ToJson(datosSesion);

        WWWForm form = new WWWForm();
        form.AddField("data", json);

        using (UnityWebRequest request = UnityWebRequest.Post(urlHojaCalculo, form))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Datos enviados: " + request.downloadHandler.text);
            }
            else
            {
                Debug.LogError(request.error);
            }
        }

        rutinaEnvio = null;

        if (hayEnvioPendiente)
        {
            hayEnvioPendiente = false;
            GuardarDatos();
        }
    }
}
