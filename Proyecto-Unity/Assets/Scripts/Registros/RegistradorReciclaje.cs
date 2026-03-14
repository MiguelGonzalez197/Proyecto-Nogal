using System;
using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class RegistradorReciclaje : MonoBehaviour
{
    //private static string rutaArchivoRegistro;

    //static RegistradorReciclaje()
    //{
    //    string ruta = Path.Combine(Application.persistentDataPath, "RegistrosReciclaje");
    //    if (!Directory.Exists(ruta))
    //        Directory.CreateDirectory(ruta);

    //    string timestamp = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
    //    rutaArchivoRegistro = Path.Combine(ruta, $"RegistroReciclaje_{timestamp}.txt");

    //    File.AppendAllText(rutaArchivoRegistro, $"--- Nueva sesion iniciada: {DateTime.Now} ---\n");
    //}

    //public static void Registro(string message)
    //{
    //    File.AppendAllText(rutaArchivoRegistro, $"{DateTime.Now:HH:mm:ss} - {message}\n");
    //}

    public static void RegistrarDatos(RegistroJugador registroJugador)
    {
        //if (registroJugador.nombreJugador == null) return;

        //File.AppendAllText(rutaArchivoRegistro, $"Nombre jugador: {registroJugador.nombreJugador}\n");
        //File.AppendAllText(rutaArchivoRegistro, $"Edad jugador: {registroJugador.edadJugador}\n");

        //if (!registroJugador.tutorialCompletado)
        //{
        //    File.AppendAllText(rutaArchivoRegistro, $"\nTutorial completado: No\n\n");
        //    File.AppendAllText(rutaArchivoRegistro, $"--- Sesion terminada: {DateTime.Now} ---\n");
        //    return;
        //}

        //File.AppendAllText(rutaArchivoRegistro, $"\nTutorial completado: Si\n\n");
        //File.AppendAllText(rutaArchivoRegistro, $"Residuos reciclados correctamente: {registroJugador.aciertosTotales}\n");
        //File.AppendAllText(rutaArchivoRegistro, $"Residuos reciclados incorrectamente: {registroJugador.desaciertosTotales}\n");
        //File.AppendAllText(rutaArchivoRegistro, $"Dinero recolectado: {registroJugador.dineroRecolectado}\n");
        //File.AppendAllText(rutaArchivoRegistro, $"Tiempo total de sesion: {registroJugador.tiempoSesion}\n");
        //File.AppendAllText(rutaArchivoRegistro, $"--- Sesion terminada: {DateTime.Now} ---\n");

    }

    string url = "TU_URL_AQUI";

    public void EnviarDatos(RegistroJugador registroJugador)
    {
        StartCoroutine(Enviar(registroJugador));
    }

    private IEnumerator Enviar(RegistroJugador registroJugador)
    {
        string json = JsonUtility.ToJson(registroJugador);

        UnityWebRequest request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(json);

        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();

        Debug.Log(request.downloadHandler.text);
    }

}
