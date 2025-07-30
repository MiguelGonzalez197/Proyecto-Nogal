using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuInicio : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private string escenaJuego = "SampleScene";
    [SerializeField] private GameObject panelAjustes;

    void Start()
    {
        if (panelAjustes != null)
            panelAjustes.SetActive(false);
    }

    void update()
    {

    }

    public void Jugar()
    {
        if (!string.IsNullOrEmpty(escenaJuego))
            SceneManager.LoadScene(escenaJuego);
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
}
