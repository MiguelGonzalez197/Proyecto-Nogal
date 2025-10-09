using System;
using System.IO;
using UnityEngine;

public class RegistradorReciclaje : MonoBehaviour
{
    private static string rutaArchivoRegistro;

    static RegistradorReciclaje()
    {
        string ruta = Path.Combine(Application.persistentDataPath, "RegistrosReciclaje");
        if (!Directory.Exists(ruta))
            Directory.CreateDirectory(ruta);

        string timestamp = DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss");
        rutaArchivoRegistro = Path.Combine(ruta, $"RegistroReciclaje_{timestamp}.txt");

        File.AppendAllText(rutaArchivoRegistro, $"--- Nueva sesion iniciada: {DateTime.Now} ---\n");
    }

    public static void Registro(string message)
    {
        File.AppendAllText(rutaArchivoRegistro, $"{DateTime.Now:HH:mm:ss} - {message}\n");
    }

    public static void RegistrarDatos(RegistroJugador registroJugador)
    {
        if (registroJugador.nombreJugador == null) return;

        File.AppendAllText(rutaArchivoRegistro, $"Nombre jugador: {registroJugador.nombreJugador}\n");
        File.AppendAllText(rutaArchivoRegistro, $"Edad jugador: {registroJugador.edadJugador}\n");

        if (!registroJugador.tutorialCompletado)
        {
            File.AppendAllText(rutaArchivoRegistro, $"\nTutorial completado: No\n\n");
            File.AppendAllText(rutaArchivoRegistro, $"--- Sesion terminada: {DateTime.Now} ---\n");
            return;
        }

        File.AppendAllText(rutaArchivoRegistro, $"\nTutorial completado: Si\n\n");
        File.AppendAllText(rutaArchivoRegistro, $"Residuos reciclados correctamente: {registroJugador.aciertosTotales}\n");
        File.AppendAllText(rutaArchivoRegistro, $"Residuos reciclados incorrectamente: {registroJugador.desaciertosTotales}\n");
        File.AppendAllText(rutaArchivoRegistro, $"Dinero recolectado: {registroJugador.dineroRecolectado}\n");
        File.AppendAllText(rutaArchivoRegistro, $"Tiempo total de sesion: {registroJugador.tiempoSesion}\n");
        File.AppendAllText(rutaArchivoRegistro, $"--- Sesion terminada: {DateTime.Now} ---\n");

    }
}
