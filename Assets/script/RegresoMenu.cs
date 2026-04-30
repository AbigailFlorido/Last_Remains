using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class RegresoMenu : MonoBehaviour
{
    [Tooltip("Tiempo en segundos antes de regresar al menú")]
    public float tiempoEspera = 3f;

    void Start()
    {
        // Inicia la cuenta atrás al arrancar la escena
        StartCoroutine(EsperarYRegresar());
    }

    IEnumerator EsperarYRegresar()
    {
        yield return new WaitForSeconds(tiempoEspera);

        // Carga la escena de tu menú. 
        // Asegúrate de que el nombre coincida exactamente con tu archivo de escena.
        SceneManager.LoadScene("Menu");
    }
}