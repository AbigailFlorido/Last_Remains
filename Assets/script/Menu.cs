using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("SelectorNiveles");
    }
    public void Salir()
    {
        Debug.Log("Sali del juego");
        Application.Quit();
    }
}
