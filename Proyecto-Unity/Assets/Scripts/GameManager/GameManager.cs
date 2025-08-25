using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
using TMPro;

public class GameManager : MonoBehaviour
{
    // ───────────────────────────────────────
    // 1. REFERENCIAS SERIALIZADAS
    // ───────────────────────────────────────
    [Header("Referencias")]
    [SerializeField]
    private Transform camara;
    [SerializeField] private TMP_FontAsset fuenteRotulo;

    // ───────────────────────────────────────
    // 2. CAMPOS PRIVADOS INTERNOS
    // ───────────────────────────────────────
    private CamaraOrbital camaraOrbital;
    private Vector3 ultimaPosicionCamara;       // Registro ultima posicion de la camara antes de mover a punto fijo
    private Quaternion ultimaRotacionCamara;    // Registro ultima rotacion de la camara antes de mover a punto fijo
    private Modulo moduloActivo;                // Referencia al modulo actualmente activo

    // ───────────────────────────────────────
    // 3. MÉTODOS UNITY
    // ───────────────────────────────────────
    void Start()
    {
        //Application.targetFrameRate = 60;
        if (camara != null)
        {
            camaraOrbital = camara.GetComponent<CamaraOrbital>();
            if (camaraOrbital == null)
            {
                camaraOrbital = camara.GetComponentInChildren<CamaraOrbital>();
            }
        }
        // mensajje de prueba
        //GestorCajaTexto.Instancia.MostrarMensaje("¡Prueba exitosa!", 2f);
        GenerarRotulosModulos();
    }

    // ───────────────────────────────────────
    // 4. MÉTODOS PÚBLICOS
    // ───────────────────────────────────────

    /// <summary>
    /// Mueve de forma inmediata a un punto fijo
    /// </summary>
    public void MoverACamaraFija(Transform Destino)
    {
        ultimaPosicionCamara = camara.position;
        ultimaRotacionCamara = camara.rotation;
        camara.position = Destino.position;
        camara.rotation = Destino.rotation;

    }

    /// <summary>
    /// Interpola la posicion y rotacion de la camara a la transformada dada
    /// </summary>
    public IEnumerator MoverCamara(Transform valorPosicion, float duracion)
    {
        float tiempo = 0f;

        Vector3 inicioPos = camara.position;
        Quaternion inicioRot = camara.rotation;

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;

            camara.position = Vector3.Lerp(inicioPos, valorPosicion.position, t);
            camara.rotation = Quaternion.Slerp(inicioRot, valorPosicion.rotation, t);

            yield return null;
        }

        camara.position = valorPosicion.position;
        camara.rotation = valorPosicion.rotation;
    }
    public void BloquearCamara()
    {
        if (camaraOrbital != null)
        {
            ultimaPosicionCamara = camara.position;
            ultimaRotacionCamara = camara.rotation;
            camaraOrbital.enabled = false;
        }
    }
    public void DesbloquearCamara()
    {
        if (camaraOrbital != null)
            camaraOrbital.enabled = true;
    }

    /// <summary>
    /// Permite que la camara vuelva a su posicion original y permite rotar alrededor de la sala
    /// </summary>
    public void RestaurarCamara()
    {
        camara.position = ultimaPosicionCamara;
        camara.rotation = ultimaRotacionCamara;

       
    }
    public void RegistrarModuloActivo(Modulo nuevoModulo)
    {
        if (moduloActivo != null && moduloActivo != nuevoModulo)
        {
            moduloActivo.DetenerProcesos();
            RestaurarCamara();
        }
        moduloActivo = nuevoModulo;
    }


    // ───────────────────────────────────────
    // 5. TEXTO FLOTANTE DE MODULOS
    // ───────────────────────────────────────
    private void GenerarRotulosModulos()
    {
        CrearRotulo<ModuloSeparacion>("Módulo de separación", Color.white);
        CrearRotulo<ModuloCompra>("Módulo de compra", Color.white);
        CrearRotulo<ModuloCrafteo>("Módulo de crafteo", Color.gray, bloquear: true);
    }

    private void CrearRotulo<T>(string texto, Color color, bool bloquear = false) where T : MonoBehaviour
    {
        T modulo = FindObjectOfType<T>();
        if (modulo == null) return;

        GameObject rotulo = new GameObject("RotuloModulo");
        rotulo.transform.SetParent(modulo.transform, false);
        rotulo.transform.localPosition = new Vector3(0f, 1.2f, 0f);

        TextMeshPro tm = rotulo.AddComponent<TextMeshPro>();
        tm.font = fuenteRotulo;          // <─ Asignas la fuente
        tm.text = texto;
        tm.color = color;
        tm.alignment = TextAlignmentOptions.Center;
        tm.fontSize = 2f;

        rotulo.AddComponent<FaceCamera>();

        if (bloquear)
        {
            BoxCollider col = modulo.GetComponent<BoxCollider>();
            if (col != null) col.enabled = false;

            foreach (Renderer r in modulo.GetComponentsInChildren<Renderer>())
            {
                foreach (Material m in r.materials)
                {
                    m.color = Color.gray;
                }
            }
        }
    }
    
}
