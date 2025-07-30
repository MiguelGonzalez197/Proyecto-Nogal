using UnityEngine;
using UnityEngine.UI;


public class ControlVolumen : MonoBehaviour
{
    [SerializeField] private Slider deslizadorVolumen;

    void Awake()
    {
        if (deslizadorVolumen == null)
            deslizadorVolumen = GetComponent<Slider>();

        if (deslizadorVolumen != null)
        {
           
            deslizadorVolumen.onValueChanged.AddListener(CambiarVolumen);
        }
    }

    void Start()
    {
        if (deslizadorVolumen != null)
        {
            deslizadorVolumen.value = AudioListener.volume;
        }
    }

   
    public void CambiarVolumen(float valor)
    {
        AudioListener.volume = valor;
    }
}
