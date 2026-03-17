using System;
using UnityEngine;

public static class CrafteoVisualHelper
{
    private const string NombreSpritePlaceholder = "IconoItem";
    private static Sprite spritePlano;

    public static string ObtenerNombre(ObjetoCrafteable objeto)
    {
        if (objeto != null && objeto.objetoACraftear != null && !string.IsNullOrWhiteSpace(objeto.objetoACraftear.name))
        {
            return objeto.objetoACraftear.name.Trim();
        }

        return "Receta reciclada";
    }

    public static string ObtenerIniciales(ObjetoCrafteable objeto)
    {
        string nombre = ObtenerNombre(objeto);
        string[] partes = nombre.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        string iniciales = string.Empty;

        for (int i = 0; i < partes.Length; i++)
        {
            string parte = partes[i];
            if (string.IsNullOrEmpty(parte) || !char.IsLetterOrDigit(parte[0]))
            {
                continue;
            }

            iniciales += char.ToUpperInvariant(parte[0]);
            if (iniciales.Length >= 2)
            {
                return iniciales;
            }
        }

        if (iniciales.Length == 0)
        {
            int longitud = Mathf.Min(2, nombre.Length);
            iniciales = nombre.Substring(0, longitud).ToUpperInvariant();
        }

        return iniciales;
    }

    public static string ObtenerResumenMateriales(ObjetoCrafteable objeto)
    {
        if (objeto == null)
        {
            return "Sin receta";
        }

        return string.Format("{0} apro. | {1} org.", objeto.aprovechablesNecesarios, objeto.organicosAprovechablesNecesarios);
    }

    public static string ObtenerEtiquetaEstado(ObjetoCrafteable objeto, Inventario inventario)
    {
        return PuedeCraftear(objeto, inventario) ? "Listo para craftear" : "Faltan materiales";
    }

    public static string ObtenerEstadoMateriales(ObjetoCrafteable objeto, Inventario inventario)
    {
        if (objeto == null)
        {
            return "Selecciona una receta para ver sus detalles.";
        }

        if (inventario == null)
        {
            return "Inventario no disponible.";
        }

        int faltantesAprovechables = Mathf.Max(0, objeto.aprovechablesNecesarios - inventario.ObtenerAprovechables());
        int faltantesOrganicos = Mathf.Max(0, objeto.organicosAprovechablesNecesarios - inventario.ObtenerOrganicos());

        if (faltantesAprovechables == 0 && faltantesOrganicos == 0)
        {
            return "Tienes todos los materiales para completar esta receta.";
        }

        if (faltantesAprovechables > 0 && faltantesOrganicos > 0)
        {
            return string.Format("Te faltan {0} aprovechables y {1} organicos aprovechables.", faltantesAprovechables, faltantesOrganicos);
        }

        if (faltantesAprovechables > 0)
        {
            return string.Format("Te faltan {0} aprovechables para completar la receta.", faltantesAprovechables);
        }

        return string.Format("Te faltan {0} organicos aprovechables para completar la receta.", faltantesOrganicos);
    }

    public static bool TieneImagenValida(Sprite sprite)
    {
        if (sprite == null)
        {
            return false;
        }

        string nombreSprite = sprite.name ?? string.Empty;
        string nombreTextura = sprite.texture != null ? sprite.texture.name : string.Empty;

        return nombreSprite.IndexOf(NombreSpritePlaceholder, StringComparison.OrdinalIgnoreCase) < 0
            && nombreTextura.IndexOf(NombreSpritePlaceholder, StringComparison.OrdinalIgnoreCase) < 0;
    }

    public static bool PuedeCraftear(ObjetoCrafteable objeto, Inventario inventario)
    {
        if (objeto == null || inventario == null)
        {
            return false;
        }

        return inventario.TieneAprovechables(objeto.aprovechablesNecesarios)
            && inventario.TieneOrganicosAprovechables(objeto.organicosAprovechablesNecesarios);
    }

    public static Color ObtenerColorBase(ObjetoCrafteable objeto)
    {
        string nombre = ObtenerNombre(objeto).ToLowerInvariant();

        if (nombre.Contains("botella"))
        {
            return new Color(0.28f, 0.63f, 0.72f, 1f);
        }

        if (nombre.Contains("carton") || nombre.Contains("caja") || nombre.Contains("papel"))
        {
            return new Color(0.73f, 0.56f, 0.36f, 1f);
        }

        if (nombre.Contains("olla") || nombre.Contains("metal") || nombre.Contains("lata"))
        {
            return new Color(0.56f, 0.50f, 0.43f, 1f);
        }

        Color[] paleta = new[]
        {
            new Color(0.29f, 0.54f, 0.47f, 1f),
            new Color(0.55f, 0.48f, 0.31f, 1f),
            new Color(0.43f, 0.58f, 0.72f, 1f),
            new Color(0.67f, 0.44f, 0.33f, 1f)
        };

        int hash = 17;
        for (int i = 0; i < nombre.Length; i++)
        {
            hash = (hash * 31) + nombre[i];
        }

        int indice = Mathf.Abs(hash) % paleta.Length;
        return paleta[indice];
    }

    public static Color ObtenerColorEstado(bool puedeCraftear)
    {
        return puedeCraftear
            ? new Color(0.20f, 0.52f, 0.29f, 1f)
            : new Color(0.73f, 0.45f, 0.12f, 1f);
    }

    public static Sprite ObtenerSpritePlano()
    {
        if (spritePlano == null)
        {
            Texture2D texturaBase = Texture2D.whiteTexture;
            spritePlano = Sprite.Create(
                texturaBase,
                new Rect(0f, 0f, texturaBase.width, texturaBase.height),
                new Vector2(0.5f, 0.5f));
        }

        return spritePlano;
    }
}
