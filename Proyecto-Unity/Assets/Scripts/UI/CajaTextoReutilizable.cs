using UnityEngine;
using TMPro;
using System.Collections;

public class CajaTextoReutilizable : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI mensajeTMP;     // asignar en el inspector

    [Header("Configuración")]
    public float tiempoCierreAutomatico = 3f;  // 0 = manual

    private CanvasGroup canvasGroup;
    private Coroutine rutinaOcultar;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = gameObject.AddComponent<CanvasGroup>();

        OcultarInstantaneo();
    }

    /// <summary>
    /// Muestra el recuadro con el texto dado.
    /// </summary>
    public void Mostrar(string mensaje)
    {
        mensajeTMP.text = mensaje;
        MostrarInstantaneo();

        if (rutinaOcultar != null)
            StopCoroutine(rutinaOcultar);

        if (tiempoCierreAutomatico > 0)
            rutinaOcultar = StartCoroutine(OcultarAutomatico());
    }

    private IEnumerator OcultarAutomatico()
    {
        yield return new WaitForSeconds(tiempoCierreAutomatico);
        Ocultar();
    }

    /// <summary>
    /// Oculta el recuadro.
    /// </summary>
    public void Ocultar()
    {
        OcultarInstantaneo();
    }

    private void MostrarInstantaneo()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void OcultarInstantaneo()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}