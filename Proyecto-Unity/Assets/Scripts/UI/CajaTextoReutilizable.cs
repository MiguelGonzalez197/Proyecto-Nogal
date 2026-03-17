using System.Collections;
using TMPro;
using UnityEngine;

public class CajaTextoReutilizable : MonoBehaviour
{
    [Header("Referencias UI")]
    public TextMeshProUGUI mensajeTMP;

    [Header("Configuración")]
    public float tiempoCierreAutomatico = 3f;

    [HideInInspector] public bool destruirAlOcultar = true;

    private CanvasGroup canvasGroup;
    private Coroutine rutinaOcultar;
    private float tiempoCierrePredeterminado;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }

        tiempoCierrePredeterminado = tiempoCierreAutomatico;
        OcultarInstantaneo();
    }

    public void Mostrar(string mensaje)
    {
        if (mensajeTMP == null) return;

        mensajeTMP.text = mensaje;
        MostrarInstantaneo();

        if (rutinaOcultar != null)
        {
            StopCoroutine(rutinaOcultar);
        }

        if (tiempoCierreAutomatico > 0f)
        {
            rutinaOcultar = StartCoroutine(OcultarAutomatico());
        }
    }

    public void Ocultar()
    {
        if (rutinaOcultar != null)
        {
            StopCoroutine(rutinaOcultar);
            rutinaOcultar = null;
        }

        OcultarInstantaneo();

        if (destruirAlOcultar)
        {
            Destroy(gameObject);
        }
    }

    public void RestablecerDuracionPredeterminada()
    {
        tiempoCierreAutomatico = tiempoCierrePredeterminado;
    }

    private IEnumerator OcultarAutomatico()
    {
        yield return new WaitForSeconds(tiempoCierreAutomatico);
        Ocultar();
    }

    private void MostrarInstantaneo()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    private void OcultarInstantaneo()
    {
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
