using System.Collections;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static readonly int ColorPropertyId = Shader.PropertyToID("_Color");
    private static readonly int BaseColorPropertyId = Shader.PropertyToID("_BaseColor");

    [Header("Referencias")]
    [SerializeField] private Transform camara;
    [SerializeField] private TMP_FontAsset fuenteRotulo;

    private CamaraOrbital camaraOrbital;
    private Vector3 ultimaPosicionCamara;
    private Quaternion ultimaRotacionCamara;
    private Modulo moduloActivo;
    private MaterialPropertyBlock materialPropertyBlock;

    private void Awake()
    {
        Input.simulateMouseWithTouches = true;
        materialPropertyBlock = new MaterialPropertyBlock();
    }

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        if (camara != null)
        {
            camaraOrbital = GetComponentInChildren<CamaraOrbital>();
            if (camaraOrbital == null)
            {
                camaraOrbital = camara.GetComponentInChildren<CamaraOrbital>();
            }
        }

        GenerarRotulosModulos();
    }

    public void MoverACamaraFija(Transform destino)
    {
        if (camara == null || destino == null) return;

        ultimaPosicionCamara = camara.position;
        ultimaRotacionCamara = camara.rotation;
        camara.position = destino.position;
        camara.rotation = destino.rotation;
    }

    public IEnumerator MoverCamara(Transform valorPosicion, float duracion)
    {
        if (camara == null || valorPosicion == null) yield break;

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
        if (camara == null || camaraOrbital == null) return;

        ultimaPosicionCamara = camara.position;
        ultimaRotacionCamara = camara.rotation;
        camaraOrbital.enabled = false;
    }

    public void DesbloquearCamara()
    {
        if (camaraOrbital != null)
        {
            camaraOrbital.enabled = true;
        }
    }

    public void RestaurarCamara()
    {
        if (camara == null) return;

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

    private void GenerarRotulosModulos()
    {
        CrearRotulo<ModuloSeparacion>("Módulo de separación", Color.white);
        CrearRotulo<ModuloCompra>("Módulo de compra", Color.white);
        CrearRotulo<ModuloCrafteo>("Módulo de crafteo", Color.gray, true);
    }

    private void CrearRotulo<T>(string texto, Color color, bool bloquear = false) where T : MonoBehaviour
    {
        T modulo = FindObjectOfType<T>();
        if (modulo == null) return;

        GameObject rotulo = new GameObject("RotuloModulo");
        rotulo.transform.SetParent(modulo.transform, false);
        rotulo.transform.localPosition = new Vector3(0f, 1.2f, 0f);

        TextMeshPro textMesh = rotulo.AddComponent<TextMeshPro>();
        textMesh.font = fuenteRotulo;
        textMesh.text = texto;
        textMesh.color = color;
        textMesh.alignment = TextAlignmentOptions.Center;
        textMesh.fontSize = 2f;

        rotulo.AddComponent<FaceCamera>();

        if (!bloquear) return;

        BoxCollider colliderModulo = modulo.GetComponent<BoxCollider>();
        if (colliderModulo != null)
        {
            colliderModulo.enabled = false;
        }

        AplicarBloqueoVisual(modulo.GetComponentsInChildren<Renderer>(true), Color.gray);
    }

    private void AplicarBloqueoVisual(Renderer[] renderers, Color color)
    {
        if (renderers == null) return;

        for (int i = 0; i < renderers.Length; i++)
        {
            Renderer rendererActual = renderers[i];
            if (rendererActual == null) continue;

            materialPropertyBlock.Clear();

            if (!RendererAceptaColor(rendererActual.sharedMaterials, color))
            {
                continue;
            }

            rendererActual.SetPropertyBlock(materialPropertyBlock);
        }
    }

    private bool RendererAceptaColor(Material[] materiales, Color color)
    {
        bool colorAplicado = false;

        for (int i = 0; i < materiales.Length; i++)
        {
            Material material = materiales[i];
            if (material == null) continue;

            if (material.HasProperty(BaseColorPropertyId))
            {
                materialPropertyBlock.SetColor(BaseColorPropertyId, color);
                colorAplicado = true;
            }

            if (material.HasProperty(ColorPropertyId))
            {
                materialPropertyBlock.SetColor(ColorPropertyId, color);
                colorAplicado = true;
            }
        }

        return colorAplicado;
    }
}
