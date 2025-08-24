using UnityEngine;

public class ResetStats : MonoBehaviour
{
    private const string PREF_TUTORIAL = "TutorialCompletado";
    private const string PREF_NOMBRE_JUGADOR = "NombreJugador";

    [ContextMenu("Resetear datos del jugador")]
    private void Resetear()
    {
        PlayerPrefs.DeleteKey(PREF_TUTORIAL);
        PlayerPrefs.DeleteKey(PREF_NOMBRE_JUGADOR);
        PlayerPrefs.Save();

        Debug.Log("Datos del jugador reiniciados.");
    }
}

