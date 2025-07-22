using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimacionesRecicladora : MonoBehaviour
{
    [SerializeField]
    private float intervalosAnimaciones = 10f;

    [Header("Lista Animaciones")]
    [SerializeField]
    List<string> animaciones = new List<string>();

    private Animator animatorRecicladora;
    string animacionActual;

    void Start()
    {
        animatorRecicladora = GetComponent<Animator>();
        StartCoroutine(IniciarAnimacion("Saludando",0.5f));
    }

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
        StopCoroutine(IniciarAnimacion(animacionActual, intervalosAnimaciones));
        if (animatorRecicladora != null)
        {
            animatorRecicladora.CrossFade("SaludandoDos", 0.1f);
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
