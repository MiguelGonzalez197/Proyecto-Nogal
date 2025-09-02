using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private string escenaJuego = "TutorialReciclaje";
    [SerializeField] private GameObject panelAjustes;

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
