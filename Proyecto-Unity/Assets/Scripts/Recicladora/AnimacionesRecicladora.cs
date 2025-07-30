using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimacionesRecicladora : MonoBehaviour
{

    // ───────────────────────────────────────
    // 1. REFERENCIAS SERIALIZADAS
    // ───────────────────────────────────────
    [SerializeField]
    private float intervalosAnimaciones = 10f;              // Intervalos de tiempo en los que la recicladora debe realizar una animacion

    [Header("Lista Animaciones")]
    [SerializeField]
    List<string> animaciones = new List<string>();         // Lista de animaciones disponibles agregadas en el inspector

    // ───────────────────────────────────────
    // 2. CAMPOS PRIVADOS INTERNOS
    // ───────────────────────────────────────
    private Animator animatorRecicladora;
    private string animacionActual;                       

    // ───────────────────────────────────────
    // 3. MÉTODOS UNITY
    // ───────────────────────────────────────
    void Start()
    {
        animatorRecicladora = GetComponent<Animator>();
        StartCoroutine(IniciarAnimacion("Saludando",0.5f));
    }

    // ───────────────────────────────────────
    // 4. MÉTODOS PÚBLICOS
    // ───────────────────────────────────────

    //Animation event
    public void VolverAInactiva()
    {
        if (animatorRecicladora != null)
        {
            animatorRecicladora.CrossFade("Inactiva", 0.2f);
            ElegirSiguienteAnimacion();
        }
    }

    public void SaludarModulo()
    {
        IniciarAnimacionModulo("SaludandoDos");
    }

    public void CompraExitosa()
    {
        IniciarAnimacionModulo("Emocionada");
    }

    public void CompraFallida()
    {
        IniciarAnimacionModulo("GestoNo");
    }

    // ───────────────────────────────────────
    // 5. MÉTODOS PRIVADOS
    // ───────────────────────────────────────
    private void IniciarAnimacionModulo(string animacion)
    {
        StopCoroutine(IniciarAnimacion(animacionActual, intervalosAnimaciones));
        if (animatorRecicladora != null)
        {
            animatorRecicladora.CrossFade(animacion, 0.1f);
        }
    }

    private void ElegirSiguienteAnimacion()
    {
        if (animaciones.Count <= 1) return;

        int indexAnimacion;

        //Evita que se repita la animacion
        do
        {
            indexAnimacion = Random.Range(0, animaciones.Count);
        }
        while (animacionActual == animaciones[indexAnimacion]);

        animacionActual = animaciones[indexAnimacion];

        StartCoroutine(IniciarAnimacion(animacionActual, intervalosAnimaciones));
    }


    private IEnumerator IniciarAnimacion(string animacion, float tiempo)
    {
        yield return new WaitForSeconds(tiempo);
        if(animatorRecicladora != null)
        {
            animatorRecicladora.CrossFade(animacion, 0.2f);
        }
    }
}
