using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GestorGameOver : MonoBehaviour
{
    public float segundosParaRegresar = 3f; // Tiempo que esperará

    void Start()
    {
        // Inicia el contador apenas carga esta escena
        StartCoroutine(VolverAlMenu());
    }

    IEnumerator VolverAlMenu()
    {
        yield return new WaitForSeconds(segundosParaRegresar);
        SceneManager.LoadScene("MenuPrincipal"); // Pon el nombre exacto de tu escena de menú
    }
}