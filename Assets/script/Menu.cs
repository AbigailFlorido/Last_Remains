using UnityEngine;
using UnityEngine.SceneManagement;
public class Menu : MonoBehaviour
{
    public void Jugar()
    {
        SceneManager.LoadScene("xd");
    }
    public void Salir()
    {
        Debug.Log("Sali del juego");
        Application.Quit();
    }
}
