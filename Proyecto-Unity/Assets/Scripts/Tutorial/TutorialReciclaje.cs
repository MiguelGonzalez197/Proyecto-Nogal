using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialReciclaje : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private string escenaPrincipal = "SampleScene";
    [SerializeField] private float duracionMensaje = 5f;
    private const string PREF_TUTORIAL = "TutorialCompletado";

    void Start()
    {
        //PlayerPrefs.SetInt(PREF_TUTORIAL, 0); /* < ------Para volver a ejecutar el tutorial incluso habiendoselo saltado quitar los comentariocs*/
        //PlayerPrefs.Save();
        if (PlayerPrefs.GetInt(PREF_TUTORIAL, 1) == 1)
        {
            
            SceneManager.LoadScene(escenaPrincipal);
            return;
            

        }
        StartCoroutine(MostrarTutorial());
    }

    public void SaltarTutorial()
    {
        PlayerPrefs.SetInt(PREF_TUTORIAL, 1);
        PlayerPrefs.Save();
        SceneManager.LoadScene(escenaPrincipal);
    }

    private IEnumerator MostrarTutorial()
    {
        GestorCajaTexto.Instancia.MostrarMensaje(
            "El reciclaje es el proceso de recoger y transformar los residuos para darles una segunda vida, reduciendo la cantidad de basura que llega a los rellenos sanitarios y el uso de recursos naturales.",
            duracionMensaje);
        yield return new WaitForSeconds(duracionMensaje);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "Separar correctamente los residuos desde casa permite que los materiales aprovechables lleguen limpios a los centros de reciclaje y disminuye la contaminación en nuestro entorno.",
            duracionMensaje);
        yield return new WaitForSeconds(duracionMensaje);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "La caneca verde representa los residuos orgánicos biodegradables, como restos de comida y cáscaras, que pueden convertirse en abono natural mediante compostaje.",
            duracionMensaje);
        yield return new WaitForSeconds(duracionMensaje);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "La caneca blanca se usa para los materiales aprovechables no orgánicos como papeles limpios, plásticos, metales o vidrios que pueden reciclarse o reutilizarse.",
            duracionMensaje);
        yield return new WaitForSeconds(duracionMensaje);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "La caneca negra se reserva para los residuos no aprovechables, aquellos que están contaminados, sucios o deteriorados y que terminarán en un relleno sanitario.",
            duracionMensaje);
        yield return new WaitForSeconds(duracionMensaje);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "Ejemplos para la caneca verde: manzana, pan, brócoli, hamburguesa, sándwich de pollo, huevos, lechuga o arroz; todos son restos que se descomponen fácilmente.",
            duracionMensaje);
        yield return new WaitForSeconds(duracionMensaje);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "Ejemplos para la caneca blanca: libro, botella de plástico limpia, botella de vidrio en buen estado, martillo, palanca, caja de cartón, pocillo o sartén sin óxido; son materiales que pueden reciclarse o reutilizarse.",
            duracionMensaje);
        yield return new WaitForSeconds(duracionMensaje);
        GestorCajaTexto.Instancia.MostrarMensaje(
            "Ejemplos para la caneca negra: botella sucia, lata de comida sucia, lata de pintura, caneca de pintura, caja de pizza sucia o lata oxidada; al estar contaminados no pueden aprovecharse.",
            duracionMensaje);
        yield return new WaitForSeconds(duracionMensaje);
        //PlayerPrefs.SetInt(PREF_TUTORIAL, 1);
        //PlayerPrefs.Save();
        //SceneManager.LoadScene(escenaPrincipal);
    }

    public static void MostrarMensajeResiduo(GameObject item)
    {
        if (PlayerPrefs.GetInt(PREF_TUTORIAL, 0) == 1) return;
        string nombre = item.name.Replace("(Clone)", "").Trim();
        string mensaje;
        
        switch (item.tag)
        {
            case "aprovechableOrganico":
                mensaje = nombre + " es un residuo orgánico que se descompone y puede convertirse en abono, por eso debe ir en la caneca verde.";
                
                break;

            case "aprovechable":
                mensaje = nombre + " está limpio y en buen estado, puede reciclarse o reutilizarse; deposítalo en la caneca blanca.";
                
                break;
           
                

            case "noAprovechable":
                mensaje = nombre + " está sucio, contaminado u oxidado y no se recicla; bótalo en la caneca negra.";
                
                break;
                
            default:
                return;
        }

        GestorCajaTexto.Instancia.MostrarMensaje(mensaje, 3f);
    }
}
