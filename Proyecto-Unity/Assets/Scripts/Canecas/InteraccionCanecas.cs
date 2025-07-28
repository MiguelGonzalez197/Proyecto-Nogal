using UnityEngine;

public class InteraccionCanecas : MonoBehaviour , IInteractuable
{
    // ───────────────────────────────────────
    // 2. CAMPOS PRIVADOS INTERNOS
    // ───────────────────────────────────────
    private ModuloSeparacion moduloSeparacion;                          // Referencia al modulo donde se encuentra la caneca

    // ───────────────────────────────────────
    // 3. MÉTODOS UNITY
    // ───────────────────────────────────────
    void Start()
    {
        moduloSeparacion = GetComponentInParent<ModuloSeparacion>();
    }

    // ───────────────────────────────────────
    // 4. MÉTODOS PÚBLICOS
    // ───────────────────────────────────────

    /// <summary>
    /// Permite saber a que ubicacion mover el item a reciclar
    /// </summary>
    public void Interactuar()
    {
        if(moduloSeparacion != null)
        {
            StartCoroutine(moduloSeparacion.MoverItem(transform));
            
        }
    }
}
