using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GestorDatos : MonoBehaviour
{
    public static GestorDatos instancia;

    [SerializeField]
    private RegistroJugador registroJugador;

    [SerializeField]
    private List<IInteractuable> listaInteractuables = new List<IInteractuable>();

    private float tiempoOcurrido;
    string urlHojaCalculo = "https://script.google.com/macros/s/AKfycbwuNA_g_0YpFvUqm4WJgcH56Ff644ypVCS3gY3CzosRakPIXpPLeonX1L7VxAzIe5Bc4Q/exec";

    void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        registroJugador.tutorialCompletado = false;

        registroJugador.sesionId = System.Guid.NewGuid().ToString();
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

    public void RegistrarInteractuableGuardado(IInteractuable interactuable)
    {
        interactuable.EnTerminarInteraccion += GuardarDatos;
    }

    public bool CompletoTutorial()
    {
        return registroJugador.tutorialCompletado;
    }

    private void GuardarDatos()
    {
        StartCoroutine(Enviar(registroJugador));
    }

    private void RegistrarTiempo()
    {
        tiempoOcurrido += Time.deltaTime;
        int minutos = Mathf.FloorToInt(tiempoOcurrido / 60);
        int segundos = Mathf.FloorToInt(tiempoOcurrido % 60);
        registroJugador.tiempoSesion = string.Format("{0:00}:{1:00}", minutos, segundos);
    }

    private IEnumerator Enviar(RegistroJugador registroJugador)
    {
        string json = JsonUtility.ToJson(registroJugador);
        Debug.Log(json);

        WWWForm form = new WWWForm();
        form.AddField("data", json);  // <-- enviamos el JSON como campo de formulario

        UnityWebRequest request = UnityWebRequest.Post(urlHojaCalculo, form);

        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
            Debug.Log("Datos enviados: " + request.downloadHandler.text);
        else
            Debug.LogError(request.error);
    }

}
