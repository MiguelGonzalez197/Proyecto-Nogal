using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EdadJugadorInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputEdad;
    [SerializeField] private Button confirmarButton;
    [SerializeField] private GameObject panel;

    private void Awake()
    {
        bool tieneEdad = PlayerPrefs.HasKey("EdadJugador");
        if (panel != null)
            panel.SetActive(!tieneEdad);
    }

    private void Start()
    {
        if (confirmarButton != null)
            confirmarButton.onClick.AddListener(GuardarEdad);
    }

    private void GuardarEdad()
    {
        if (inputEdad == null) return;

        string edadTexto = inputEdad.text.Trim();

        // Validamos que sea un número
        if (int.TryParse(edadTexto, out int edad) && edad > 0)
        {
            PlayerPrefs.SetInt("EdadJugador", edad);
            PlayerPrefs.Save();

            if (panel != null)
                panel.SetActive(false);

            Debug.Log("Edad guardada: " + edad);
        }
        else
        {
            Debug.LogWarning("Edad no válida. Por favor, ingresa un número.");
        }
    }
}
