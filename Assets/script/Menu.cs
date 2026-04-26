using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("Demo");
    }
    public void Salir()
    {
        Debug.Log("Sali del juego");
        Application.Quit();
    }
}
