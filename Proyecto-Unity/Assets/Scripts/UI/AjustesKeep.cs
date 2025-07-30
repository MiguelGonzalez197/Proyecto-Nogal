using UnityEngine;

public class AjustesKeep : MonoBehaviour
{
    [SerializeField] private GameObject panelAjustes;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (panelAjustes != null)
            panelAjustes.SetActive(false);
    }

    public void ToggleAjustes()
    {
        if (panelAjustes != null)
            panelAjustes.SetActive(!panelAjustes.activeSelf);
    }
}
