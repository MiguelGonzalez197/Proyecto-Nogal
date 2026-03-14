using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class GestorDatos : MonoBehaviour
{
    public static GestorDatos instancia;

    [SerializeField]
    private RegistroJugador registroJugador;

    private float tiempoOcurrido;
    string urlHojaCalculo = "https://script.google.com/macros/s/AKfycbwuNA_g_0YpFvUqm4WJgcH56Ff644ypVCS3gY3CzosRakPIXpPLeonX1L7VxAzIe5Bc4Q/exec";

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

    void Start()
    {
        RegistrarDatosJugador("Test", 10);
        RegistrarExitoTutorial(true);
        StartCoroutine(Enviar(registroJugador));
    }

    void Update()
    {
        RegistrarTiempo();

        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("Boton presionado para guardar");
            GuardarDatos();
        }
    }

    void OnApplicationPause(bool pause)
    {
        //if (pause) GuardarDatos();
    }

    void OnApplicationQuit()
    {
        //GuardarDatos();

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
        StartCoroutine(Enviar(registroJugador));
        //RegistradorReciclaje.RegistrarDatos(registroJugador); ;
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
