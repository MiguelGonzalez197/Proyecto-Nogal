using UnityEngine;

/// <summary>
/// Mantiene el objeto siempre mirando hacia la cámara principal.
/// </summary>
public class FaceCamera : MonoBehaviour
{
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        if (cam != null)
        {
            transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
        }
    }
}
