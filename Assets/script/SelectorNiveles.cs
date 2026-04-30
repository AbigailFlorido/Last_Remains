using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para cambiar de nivel

public class SelectorNiveles : MonoBehaviour
{
    // Esta función la llamaremos desde los botones
    public void CargarNivel(string nombreNivel)
    {
        SceneManager.LoadScene(nombreNivel);
    }
}