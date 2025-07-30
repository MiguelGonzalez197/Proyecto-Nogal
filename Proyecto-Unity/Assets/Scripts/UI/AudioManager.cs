using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ReproductorMusica : MonoBehaviour
{
    [Header("Configuracion")]
    [SerializeField] private AudioClip cancion;

    private static ReproductorMusica instancia;

    private AudioSource audioSource;

    void Awake()
    {
        if (instancia != null && instancia != this)
        {
            Destroy(gameObject);
            return;
        }

        instancia = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        if (cancion != null)
            audioSource.clip = cancion;
    }

    void Start()
    {
        if (audioSource.clip != null)
            audioSource.Play();
    }
}