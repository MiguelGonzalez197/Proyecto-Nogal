using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    private const string PREF_TUTORIAL = "TutorialCompletado";
    private const string PREF_NOMBRE_JUGADOR = "NombreJugador";
    private const string PREF_EDAD_JUGADOR = "EdadJugador";

    [Header("Configuracion")]
    [SerializeField] private string escenaJuego = "TutorialReciclaje";
    [SerializeField] private GameObject panelAjustes;
    [SerializeField] private bool solicitarDatosJugadorEnCadaPartida = true;

    [Header("Referencia pantalla de carga")]
    [SerializeField] private GameObject pantallaCarga;
    [SerializeField] private GameObject menuPrincipal;

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        if (panelAjustes != null)
            panelAjustes.SetActive(false);
    }


    public void Jugar()
    {
        if (pantallaCarga == null || menuPrincipal == null) return;
        if (!string.IsNullOrEmpty(escenaJuego))
        {
            if (solicitarDatosJugadorEnCadaPartida)
            {
                PlayerPrefs.DeleteKey(PREF_TUTORIAL);
                PlayerPrefs.DeleteKey(PREF_NOMBRE_JUGADOR);
                PlayerPrefs.DeleteKey(PREF_EDAD_JUGADOR);
                PlayerPrefs.Save();
            }

            pantallaCarga.SetActive(true);
            menuPrincipal.SetActive(false);
            StartCoroutine(CargarNivel());
            //SceneManager.LoadScene(escenaJuego);
        }
            
    }

    public void AbrirAjustes()
    {
        if (panelAjustes != null)
            panelAjustes.SetActive(true);
    }

    public void CerrarAjustes()
    {
        if (panelAjustes != null)
            panelAjustes.SetActive(false);
    }

    public void Salir()
    {
        Application.Quit();
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }

    private IEnumerator CargarNivel()
    {
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(escenaJuego);

        while (!loadOperation.isDone)
        {
            yield return null;
        }
    }
}
